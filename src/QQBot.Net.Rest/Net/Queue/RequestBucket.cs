using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using QQBot.Net.Rest;
#if DEBUG_LIMITS
using System.Diagnostics;
#endif

namespace QQBot.Net.Queue;

internal class RequestBucket
{
    private const int MinimumSleepTimeMs = 750;

    private readonly object _lock;
    private readonly RequestQueue _queue;
    private int _semaphore;
    private DateTimeOffset? _resetTick;
    private RequestBucket? _redirectBucket;
    private readonly JsonSerializerOptions _serializerOptions;

    public BucketId Id { get; private set; }
    public int WindowCount { get; private set; }
    public DateTimeOffset LastAttemptAt { get; private set; }

    public RequestBucket(RequestQueue queue, IRequest request, BucketId id)
    {
        _serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
        _queue = queue;
        Id = id;

        _lock = new object();

        if (request.Options.IsClientBucket)
            WindowCount = ClientBucket.Get(request.Options.BucketId ?? throw new InvalidOperationException("Client bucket is not set.")).WindowCount;
        else if (request.Options.IsGatewayBucket)
            WindowCount = GatewayBucket.Get(request.Options.BucketId ?? throw new InvalidOperationException("Gateway bucket is not set.")).WindowCount;
        else
            WindowCount = 117; // TODO: Preemptive rate limit

        _semaphore = WindowCount;
        _resetTick = null;
        LastAttemptAt = DateTimeOffset.UtcNow;
    }

    private static int nextId = 0;

    public async Task<Stream> SendAsync(RestRequest request)
    {
        int id = Interlocked.Increment(ref nextId);
#if DEBUG_LIMITS
        Debug.WriteLine($"[{id}] Start");
#endif
        LastAttemptAt = DateTimeOffset.UtcNow;
        while (true)
        {
            await _queue.EnterGlobalAsync(id, request).ConfigureAwait(false);
            await EnterAsync(id, request).ConfigureAwait(false);
            if (_redirectBucket != null)
                return await _redirectBucket.SendAsync(request);

#if DEBUG_LIMITS
            Debug.WriteLine($"[{id}] Sending...");
#endif
            RestResponse response = default;
            RateLimitInfo info = default;
            try
            {
                response = await request.SendAsync().ConfigureAwait(false);
                info = new RateLimitInfo(response.Headers, request.Endpoint);

                request.Options.ExecuteRatelimitCallback(info);

                if (response.StatusCode is not (HttpStatusCode.OK or HttpStatusCode.Accepted or HttpStatusCode.NoContent))
                    switch (response.StatusCode)
                    {
                        case (HttpStatusCode)429:
                            if (info.IsGlobal)
                            {
#if DEBUG_LIMITS
                                Debug.WriteLine($"[{id}] (!) 429 [Global]");
#endif
                                _queue.PauseGlobal(info);
                            }
                            else
                            {
#if DEBUG_LIMITS
                                Debug.WriteLine($"[{id}] (!) 429");
#endif
                            }

                            await _queue.RaiseRateLimitTriggered(Id, info, $"{request.Method} {request.Endpoint}").ConfigureAwait(false);
                            continue;                   //Retry
                        case HttpStatusCode.BadGateway: //502
#if DEBUG_LIMITS
                            Debug.WriteLine($"[{id}] (!) 502");
#endif
                            if ((request.Options.RetryMode & RetryMode.Retry502) == 0)
                                throw new HttpException(HttpStatusCode.BadGateway, request);

                            continue; //Retry
                        default:
                            API.QQBotError? error = null;
                            if (response.Stream != null)
                                try
                                {
                                    error = await JsonSerializer.DeserializeAsync<API.QQBotError>(response.Stream, _serializerOptions);
                                }
                                catch
                                {
                                    // ignored
                                }

                            throw new HttpException(
                                response.StatusCode,
                                request,
                                error?.Code,
                                error?.Message,
                                error?.ErrorCode,
                                error?.TraceId
                            );
                    }
                else
                {
#if DEBUG_LIMITS
                    Debug.WriteLine($"[{id}] Success");
#endif
                    return response.Stream;
                }
            }
            //catch (HttpException) { throw; } //Pass through
            catch (TimeoutException)
            {
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Timeout");
#endif
                if ((request.Options.RetryMode & RetryMode.RetryTimeouts) == 0)
                    throw;

                await Task.Delay(500).ConfigureAwait(false);
                continue; //Retry
            }
            /*catch (Exception)
            {
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Error");
#endif
                if ((request.Options.RetryMode & RetryMode.RetryErrors) == 0)
                    throw;

                await Task.Delay(500);
                continue; //Retry
            }*/
            finally
            {
                UpdateRateLimit(id, request, info, response.StatusCode == (HttpStatusCode)429);
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Stop");
#endif
            }
        }
    }

    public async Task SendAsync(WebSocketRequest request)
    {
        int id = Interlocked.Increment(ref nextId);
#if DEBUG_LIMITS
        Debug.WriteLine($"[{id}] Start");
#endif
        LastAttemptAt = DateTimeOffset.UtcNow;
        while (true)
        {
            await _queue.EnterGlobalAsync(id, request).ConfigureAwait(false);
            await EnterAsync(id, request).ConfigureAwait(false);

#if DEBUG_LIMITS
            Debug.WriteLine($"[{id}] Sending...");
#endif
            try
            {
                await request.SendAsync().ConfigureAwait(false);
                return;
            }
            catch (TimeoutException)
            {
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Timeout");
#endif
                if ((request.Options.RetryMode & RetryMode.RetryTimeouts) == 0)
                    throw;

                await Task.Delay(500).ConfigureAwait(false);
                continue; //Retry
            }
            /*catch (Exception)
            {
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Error");
#endif
                if ((request.Options.RetryMode & RetryMode.RetryErrors) == 0)
                    throw;

                await Task.Delay(500);
                continue; //Retry
            }*/
            finally
            {
                UpdateRateLimit(id, request, default, false);
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Stop");
#endif
            }
        }
    }

    internal async Task TriggerAsync(int id, IRequest request)
    {
#if DEBUG_LIMITS
        Debug.WriteLine($"[{id}] Trigger Bucket");
#endif
        await EnterAsync(id, request).ConfigureAwait(false);
        UpdateRateLimit(id, request, default, false);
    }

    private async Task EnterAsync(int id, IRequest request)
    {
        int windowCount;
        DateTimeOffset? resetAt;
        bool isRateLimited = false;

        while (true)
        {
            if (_redirectBucket != null) break;

            if (DateTimeOffset.UtcNow > request.TimeoutAt || request.Options.CancellationToken.IsCancellationRequested)
            {
                if (!isRateLimited)
                    throw new TimeoutException();
                ThrowRetryLimit(request);
            }

            lock (_lock)
            {
                windowCount = WindowCount;
                resetAt = _resetTick;
            }

            DateTimeOffset? timeoutAt = request.TimeoutAt;
            int semaphore = Interlocked.Decrement(ref _semaphore);
            if (windowCount >= 0 && semaphore < 0)
            {
                if (!isRateLimited)
                {
                    bool ignoreRatelimit = false;
                    isRateLimited = true;
                    switch (request)
                    {
                        case RestRequest restRequest:
                            await _queue.RaiseRateLimitTriggered(Id, null, $"{restRequest.Method} {restRequest.Endpoint}").ConfigureAwait(false);
                            break;
                        case WebSocketRequest webSocketRequest:
                            if (webSocketRequest.IgnoreLimit)
                            {
                                ignoreRatelimit = true;
                                break;
                            }

                            await _queue.RaiseRateLimitTriggered(Id, null, Id.Endpoint).ConfigureAwait(false);
                            break;
                        default:
                            throw new InvalidOperationException("Unknown request type");
                    }

                    if (ignoreRatelimit)
                    {
#if DEBUG_LIMITS
                        Debug.WriteLine($"[{id}] Ignoring ratelimit");
#endif
                        break;
                    }
                }

                ThrowRetryLimit(request);

                if (resetAt.HasValue && resetAt > DateTimeOffset.UtcNow)
                {
                    if (resetAt > timeoutAt)
                        ThrowRetryLimit(request);

                    int millis = (int)Math.Ceiling((resetAt.Value - DateTimeOffset.UtcNow).TotalMilliseconds);
#if DEBUG_LIMITS
                    Debug.WriteLine($"[{id}] Sleeping {millis} ms (Pre-emptive)");
#endif
                    if (millis > 0) await Task.Delay(millis, request.Options.CancellationToken).ConfigureAwait(false);
                }
                else
                {
                    if ((timeoutAt - DateTimeOffset.UtcNow)?.TotalMilliseconds < MinimumSleepTimeMs)
                        ThrowRetryLimit(request);
#if DEBUG_LIMITS
                    Debug.WriteLine($"[{id}] Sleeping {MinimumSleepTimeMs}* ms (Pre-emptive)");
#endif
                    await Task.Delay(MinimumSleepTimeMs, request.Options.CancellationToken).ConfigureAwait(false);
                }

                continue;
            }
#if DEBUG_LIMITS
            Debug.WriteLine($"[{id}] Entered Semaphore ({semaphore}/{WindowCount} remaining)");
#endif
            break;
        }
    }

    private void UpdateRateLimit(int id, IRequest request, RateLimitInfo info, bool is429, bool redirected = false)
    {
        if (WindowCount == 0)
            return;

        lock (_lock)
        {
#if DEBUG_LIMITS
            Debug.WriteLine($"[{id}] Raw RateLimitInto: IsGlobal: {info.IsGlobal}, Limit: {info.Limit}, Remaining: {info.Remaining}, ResetAfter: {info.ResetAfter?.TotalSeconds}");
#endif
            if (redirected)
            {
                // we might still hit a real ratelimit if all tickets were already taken, can't do much about it since we didn't know they were the same
                Interlocked.Decrement(ref _semaphore);
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Decrease Semaphore");
#endif
            }

            bool hasQueuedReset = _resetTick != null;

            if (info.Bucket != null && !redirected)
            {
                (RequestBucket?, BucketId?) hashBucket = _queue.UpdateBucketHash(Id, info.Bucket);
                if (hashBucket.Item1 is not null && hashBucket.Item2 is not null)
                {
                    if (hashBucket.Item1 == this) //this bucket got promoted to a hash queue
                    {
                        Id = hashBucket.Item2;
#if DEBUG_LIMITS
                        Debug.WriteLine($"[{id}] Promoted to Hash Bucket ({hashBucket.Item2})");
#endif
                    }
                    else
                    {
                        // this request should be part of another bucket, this bucket will be disabled, redirect everything
                        _redirectBucket = hashBucket.Item1;
                        // update the hash bucket ratelimit
                        _redirectBucket.UpdateRateLimit(id, request, info, is429, true);
#if DEBUG_LIMITS
                        Debug.WriteLine($"[{id}] Redirected to {_redirectBucket.Id}");
#endif
                        return;
                    }
                }
            }

            if (info.Limit.HasValue && WindowCount != info.Limit.Value)
            {
                WindowCount = info.Limit.Value;
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Updated Limit to {WindowCount}");
#endif
            }

            if (info.Remaining.HasValue && _semaphore != info.Remaining.Value)
            {
                _semaphore = info.Remaining.Value;
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Updated Semaphore (Remaining) to {_semaphore}");
#endif
            }

            DateTimeOffset? resetTick = null;

            //Using X-Rate-Limit-Remaining causes a race condition
            /*if (info.Remaining.HasValue)
            {
                Debug.WriteLine($"[{id}] X-Rate-Limit-Remaining: " + info.Remaining.Value);
                _semaphore = info.Remaining.Value;
            }*/
            if (is429)
            {
                // Stop all requests until the QueueReset task is complete
                _semaphore = 0;

                // Read the Reset-After header
                resetTick = DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(info.ResetAfter?.TotalSeconds ?? 0));
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Reset-After: {info.ResetAfter} ({info.ResetAfter?.TotalMilliseconds} ms)");
#endif
            }
            //                 if (info.RetryAfter.HasValue)
            //                 {
            //                     //RetryAfter is more accurate than Reset, where available
            //                     resetTick = DateTimeOffset.UtcNow.AddSeconds(info.RetryAfter.Value);
            // #if DEBUG_LIMITS
            //                     Debug.WriteLine($"[{id}] Retry-After: {info.RetryAfter.Value} ({info.RetryAfter.Value} ms)");
            // #endif
            //                 }
            else if (info.ResetAfter.HasValue) // && (request.Options.UseSystemClock.HasValue && !request.Options.UseSystemClock.Value)
            {
                resetTick = DateTimeOffset.UtcNow.Add(info.ResetAfter.Value);
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Reset-After: {info.ResetAfter.Value} ({info.ResetAfter?.TotalMilliseconds} ms)");
#endif
            }
            //                 else if (info.Reset.HasValue)
            //                 {
            //                     resetTick = info.Reset.Value.AddSeconds(info.Lag?.TotalSeconds ?? 1.0);
            //
            //                     /* millisecond precision makes this unnecessary, retaining in case of regression
            //                     if (request.Options.IsReactionBucket)
            //                         resetTick = DateTimeOffset.Now.AddMilliseconds(250);
            // 					*/
            //
            //                     int diff = (int)(resetTick.Value - DateTimeOffset.UtcNow).TotalMilliseconds;
            // #if DEBUG_LIMITS
            //                     Debug.WriteLine($"[{id}] X-Rate-Limit-Reset: {info.Reset.Value.ToUnixTimeSeconds()} ({diff} ms, {info.Lag?.TotalMilliseconds} ms lag)");
            // #endif
            //                 }
            else if (request.Options.IsClientBucket && Id != null)
            {
                resetTick = DateTimeOffset.UtcNow.AddSeconds(ClientBucket.Get(Id).WindowSeconds);
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Client Bucket ({ClientBucket.Get(Id).WindowSeconds * 1000} ms)");
#endif
            }
            else if (request.Options.IsGatewayBucket && request.Options.BucketId != null)
            {
                resetTick = DateTimeOffset.UtcNow.AddSeconds(GatewayBucket.Get(request.Options.BucketId).WindowSeconds);
#if DEBUG_LIMITS
                    Debug.WriteLine($"[{id}] Gateway Bucket ({GatewayBucket.Get(request.Options.BucketId).WindowSeconds * 1000} ms)");
#endif
                if (!hasQueuedReset)
                {
                    _resetTick = resetTick;
                    LastAttemptAt = resetTick.Value;
#if DEBUG_LIMITS
                    Debug.WriteLine($"[{id}] Reset in {(int)Math.Ceiling((resetTick - DateTimeOffset.UtcNow).Value.TotalMilliseconds)} ms");
#endif
                    _ = QueueReset(id, (int)Math.Ceiling((_resetTick.Value - DateTimeOffset.UtcNow).TotalMilliseconds), request);
                }

                return;
            }

            if (resetTick == null)
            {
                WindowCount = -1; //No rate limit info, disable limits on this bucket
#if DEBUG_LIMITS
                    Debug.WriteLine($"[{id}] Disabled Semaphore");
#endif
                return;
            }

            if (!hasQueuedReset || resetTick > _resetTick)
            {
                _resetTick = resetTick;
                LastAttemptAt = resetTick.Value; //Make sure we don't destroy this until after it's been reset
#if DEBUG_LIMITS
                Debug.WriteLine($"[{id}] Reset in {(int)Math.Ceiling((resetTick - DateTimeOffset.UtcNow).Value.TotalMilliseconds)} ms");
#endif

                if (!hasQueuedReset)
                    _ = QueueReset(id, (int)Math.Ceiling((_resetTick.Value - DateTimeOffset.UtcNow).TotalMilliseconds), request);
            }
        }
    }

    private async Task QueueReset(int id, int millis, IRequest request)
    {
        if (_resetTick == null)
            return;

        while (true)
        {
            if (millis > 0)
                await Task.Delay(millis).ConfigureAwait(false);

            lock (_lock)
            {
                millis = (int)Math.Ceiling((_resetTick.Value - DateTimeOffset.UtcNow).TotalMilliseconds);
                if (millis <= 0) //Make sure we haven't gotten a more accurate reset time
                {
#if DEBUG_LIMITS
                    Debug.WriteLine($"[{id}] * Reset *");
#endif
                    _semaphore = WindowCount;
                    _resetTick = null;
                    return;
                }
            }
        }
    }

    private void ThrowRetryLimit(IRequest request)
    {
        if ((request.Options.RetryMode & RetryMode.RetryRatelimit) == 0)
            throw new RateLimitedException(request);
    }
}
