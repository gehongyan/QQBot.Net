using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class MediaUploadHelper
{
    // QQ media upload protocol: md5_10m is the MD5 of the first 10,002,432 bytes, not 10 MiB.
    private const int QqMediaUploadMd5PrefixByteCount = 10_002_432;
    private const int BytesPerKibibyte = 1_024;
    private const int HashReadBufferKibibytes = 128;
    private const int HashReadBufferByteCount = HashReadBufferKibibytes * BytesPerKibibyte;
    // QQ recommends a timeout of at least five seconds for media upload requests.
    private const int MinimumMediaUploadRequestTimeoutSeconds = 5;
    private const int MillisecondsPerSecond = 1_000;
    private const int MinimumMediaUploadRequestTimeoutMilliseconds =
        MinimumMediaUploadRequestTimeoutSeconds * MillisecondsPerSecond;
    private static readonly TimeSpan MinimumMediaUploadRequestTimeout =
        TimeSpan.FromSeconds(MinimumMediaUploadRequestTimeoutSeconds);
    private static readonly HttpClient UploadClient = new() { Timeout = Timeout.InfiniteTimeSpan };

    public static async Task<IMediaUploadSession> PrepareAsync(IMediaUploadChannel channel, BaseQQBotClient client,
        MediaUploadDescriptor file, RequestOptions? options)
    {
        ArgumentNullException.ThrowIfNull(file);
        options = CreateMediaUploadRequestOptions(options);
        PrepareMediaUploadParams args = new()
        {
            FileType = file.Type,
            FileSize = file.Length.ToString(CultureInfo.InvariantCulture),
            FileName = file.FileName,
            Md5 = file.Md5,
            Sha1 = file.Sha1,
            Md5First10MiB = file.Md5First10MiB
        };
        PrepareMediaUploadResponse response = channel switch
        {
            IUserChannel userChannel => await client.ApiClient
                .PrepareUserMediaUploadAsync(userChannel.Id, args, options).ConfigureAwait(false),
            IGroupChannel groupChannel => await client.ApiClient
                .PrepareGroupMediaUploadAsync(groupChannel.Id, args, options).ConfigureAwait(false),
            _ => throw new NotSupportedException("The channel does not support media uploads.")
        };
        return CreateSession(channel, client, file, response);
    }

    public static async Task<MediaUploadResult> UploadAsync(IMediaUploadChannel channel, BaseQQBotClient client,
        MediaUploadSource source, IProgress<MediaUploadProgress>? progress, MediaUploadOptions? uploadOptions,
        RequestOptions? options)
    {
        ArgumentNullException.ThrowIfNull(source);
        options = CreateMediaUploadRequestOptions(options);
        if (source.Kind == MediaUploadSourceKind.Uri)
        {
            if (source.Uri is null)
                throw new InvalidOperationException("The URI media upload source is invalid.");
            return await UploadUriAsync(channel, client, source, options).ConfigureAwait(false);
        }

        CancellationToken cancellationToken = options.CancellationToken;
        await using UploadInput input = await UploadInput.CreateAsync(source, cancellationToken).ConfigureAwait(false);
        MediaUploadDescriptor descriptor = await CreateDescriptorAsync(source, input.Stream, cancellationToken)
            .ConfigureAwait(false);
        input.Stream.Position = input.StartPosition;

        RestMediaUploadSession session = (RestMediaUploadSession)await PrepareAsync(channel, client, descriptor, options)
            .ConfigureAwait(false);
        int concurrency = session.Configuration.Concurrency;
        if (uploadOptions?.MaximumConcurrency is { } maximumConcurrency)
        {
            if (maximumConcurrency <= 0)
                throw new ArgumentOutOfRangeException(nameof(uploadOptions), "MaximumConcurrency must be greater than zero.");
            concurrency = Math.Min(concurrency, maximumConcurrency);
        }
        concurrency = Math.Max(1, concurrency);

        long confirmedBytes = 0;
        int confirmedPartCount = 0;
        List<Task> activeUploads = [];
        foreach (MediaUploadPart part in session.Parts.OrderBy(x => x.Index))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (part.Length > int.MaxValue)
                throw new NotSupportedException("A media upload part is too large to buffer.");

            byte[] buffer = GC.AllocateUninitializedArray<byte>((int)part.Length);
            await ReadExactlyAsync(input.Stream, buffer, cancellationToken).ConfigureAwait(false);
            string md5 = Convert.ToHexString(MD5.HashData(buffer)).ToLowerInvariant();
            activeUploads.Add(UploadAndConfirmPartAsync(session, part, buffer, md5, descriptor.Length,
                session.Parts.Count, session.Configuration, progress,
                () => Interlocked.Add(ref confirmedBytes, part.Length),
                () => Interlocked.Increment(ref confirmedPartCount), options, cancellationToken));

            if (activeUploads.Count >= concurrency)
            {
                Task completed = await Task.WhenAny(activeUploads).ConfigureAwait(false);
                activeUploads.Remove(completed);
                await completed.ConfigureAwait(false);
            }
        }
        await Task.WhenAll(activeUploads).ConfigureAwait(false);
        return await session.CompleteAsync(null, options).ConfigureAwait(false);
    }

    public static async Task ConfirmPartAsync(IMediaUploadChannel channel, BaseQQBotClient client,
        string uploadId, MediaUploadPartCompletion completion, RequestOptions? options)
    {
        options = CreateMediaUploadRequestOptions(options);
        FinishMediaUploadPartParams args = new()
        {
            UploadId = uploadId,
            PartIndex = completion.Index,
            BlockSize = completion.Length.ToString(CultureInfo.InvariantCulture),
            Md5 = completion.Md5
        };
        switch (channel)
        {
            case IUserChannel userChannel:
                await client.ApiClient.PrepareUserMediaUploadPartAsync(userChannel.Id, args, options)
                    .ConfigureAwait(false);
                break;
            case IGroupChannel groupChannel:
                await client.ApiClient.PrepareGroupMediaUploadPartAsync(groupChannel.Id, args, options)
                    .ConfigureAwait(false);
                break;
            default:
                throw new NotSupportedException("The channel does not support media uploads.");
        }
    }

    public static async Task<MediaUploadResult> CompleteAsync(IMediaUploadChannel channel, BaseQQBotClient client,
        MediaUploadDescriptor descriptor, string uploadId, MediaUploadCompletionOptions completionOptions,
        RequestOptions? options)
    {
        options = CreateMediaUploadRequestOptions(options);
        SendAttachmentParams args = new()
        {
            FileType = descriptor.Type,
            ServerSendMessage = completionOptions.SendMessage,
            FileName = descriptor.FileName,
            UploadId = uploadId
        };
        SendAttachmentResponse response = channel switch
        {
            IUserChannel userChannel => await client.ApiClient.CreateUserAttachmentAsync(userChannel.Id, args, options)
                .ConfigureAwait(false),
            IGroupChannel groupChannel => await client.ApiClient.CreateGroupAttachmentAsync(groupChannel.Id, args, options)
                .ConfigureAwait(false),
            _ => throw new NotSupportedException("The channel does not support media uploads.")
        };
        return CreateMediaUploadResult(channel, descriptor.Type, descriptor.FileName, response);
    }

    private static async Task<MediaUploadResult> UploadUriAsync(IMediaUploadChannel channel, BaseQQBotClient client,
        MediaUploadSource source, RequestOptions options)
    {
        SendAttachmentParams args = new()
        {
            FileType = source.Type,
            Url = source.Uri!.OriginalString,
            FileName = source.FileName,
            ServerSendMessage = false
        };
        SendAttachmentResponse response = channel switch
        {
            IUserChannel userChannel => await client.ApiClient.CreateUserAttachmentAsync(userChannel.Id, args, options)
                .ConfigureAwait(false),
            IGroupChannel groupChannel => await client.ApiClient.CreateGroupAttachmentAsync(groupChannel.Id, args, options)
                .ConfigureAwait(false),
            _ => throw new NotSupportedException("The channel does not support media uploads.")
        };
        return CreateMediaUploadResult(channel, source.Type, source.FileName, response);
    }

    private static MediaUploadResult CreateMediaUploadResult(IMediaUploadChannel channel, AttachmentType type,
        string fileName, SendAttachmentResponse response)
    {
        MediaFileInfo mediaFileInfo = new()
        {
            FileId = response.FileUuid,
            AttachmentType = type,
            CreatedAt = DateTimeOffset.UtcNow,
            LifeTime = response.TimeToLive == 0 ? TimeSpan.Zero : TimeSpan.FromSeconds(response.TimeToLive),
            FileInfo = response.FileInfo
        };
        Uri? downloadUri = Uri.TryCreate(response.RawUrl, UriKind.Absolute, out Uri? uri) ? uri : null;
        FileAttachment attachment = channel switch
        {
            IUserChannel => new FileAttachment(mediaFileInfo, null, fileName),
            IGroupChannel => new FileAttachment(null, mediaFileInfo, fileName),
            _ => throw new NotSupportedException("The channel does not support media uploads.")
        };
        return new MediaUploadResult(attachment, mediaFileInfo, downloadUri);
    }

    private static RequestOptions CreateMediaUploadRequestOptions(RequestOptions? options)
    {
        RequestOptions result = RequestOptions.CreateOrClone(options);
        if (result.Timeout is { } timeout && timeout < MinimumMediaUploadRequestTimeoutMilliseconds)
            result.Timeout = MinimumMediaUploadRequestTimeoutMilliseconds;
        return result;
    }

    private static TimeSpan GetMediaUploadRetryTimeout(int? seconds)
    {
        TimeSpan timeout = TimeSpan.FromSeconds(Math.Max(1, seconds.GetValueOrDefault(300)));
        return timeout < MinimumMediaUploadRequestTimeout ? MinimumMediaUploadRequestTimeout : timeout;
    }

    private static RestMediaUploadSession CreateSession(IMediaUploadChannel channel, BaseQQBotClient client,
        MediaUploadDescriptor descriptor, PrepareMediaUploadResponse response)
    {
        if (!long.TryParse(response.BlockSize, NumberStyles.None, CultureInfo.InvariantCulture, out long blockSize)
            || blockSize <= 0)
            throw new InvalidOperationException("The upload prepare response contains an invalid block size.");

        List<MediaUploadPart> parts = [];
        foreach (PrepareMediaUploadPart part in response.Parts)
        {
            if (!long.TryParse(part.BlockSize, NumberStyles.None, CultureInfo.InvariantCulture, out long length)
                || length < 0 || !Uri.TryCreate(part.PresignedUrl, UriKind.Absolute, out Uri? uploadUri))
                throw new InvalidOperationException("The upload prepare response contains an invalid upload part.");
            parts.Add(new MediaUploadPart(part.Index, length, uploadUri));
        }
        MediaUploadConfiguration configuration = new(
            Math.Max(1, response.UploadConfiguration.Concurrency.GetValueOrDefault(1)),
            GetMediaUploadRetryTimeout(response.UploadConfiguration.RetryTimeout),
            TimeSpan.FromSeconds(Math.Max(0, response.UploadConfiguration.RetryDelay.GetValueOrDefault(1))));
        return new RestMediaUploadSession(channel, client, descriptor, response.UploadId, blockSize, parts, configuration);
    }

    private static async Task<MediaUploadDescriptor> CreateDescriptorAsync(MediaUploadSource source, Stream stream,
        CancellationToken cancellationToken)
    {
        using IncrementalHash md5 = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
        using IncrementalHash sha1 = IncrementalHash.CreateHash(HashAlgorithmName.SHA1);
        using IncrementalHash md5First = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
        long startPosition = stream.Position;
        long length = stream.Length - startPosition;
        int firstRemaining = QqMediaUploadMd5PrefixByteCount;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(HashReadBufferByteCount);
        try
        {
            int read;
            while ((read = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) > 0)
            {
                md5.AppendData(buffer, 0, read);
                sha1.AppendData(buffer, 0, read);
                int firstRead = Math.Min(firstRemaining, read);
                if (firstRead > 0)
                {
                    md5First.AppendData(buffer, 0, firstRead);
                    firstRemaining -= firstRead;
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
        return new MediaUploadDescriptor(source.Type, source.FileName, length,
            Convert.ToHexString(md5.GetHashAndReset()).ToLowerInvariant(),
            Convert.ToHexString(sha1.GetHashAndReset()).ToLowerInvariant(),
            Convert.ToHexString(md5First.GetHashAndReset()).ToLowerInvariant());
    }

    private static async Task UploadAndConfirmPartAsync(RestMediaUploadSession session, MediaUploadPart part,
        byte[] buffer, string md5, long totalBytes, int totalPartCount, MediaUploadConfiguration configuration,
        IProgress<MediaUploadProgress>? progress, Func<long> addConfirmedBytes, Func<int> addConfirmedPartCount,
        RequestOptions? options, CancellationToken cancellationToken)
    {
        await PutPartAsync(part.UploadUri, buffer, configuration, cancellationToken).ConfigureAwait(false);
        await session.ConfirmPartAsync(new MediaUploadPartCompletion(part.Index, part.Length, md5), options)
            .ConfigureAwait(false);
        long confirmedBytes = addConfirmedBytes();
        int confirmedPartCount = addConfirmedPartCount();
        progress?.Report(new MediaUploadProgress(totalBytes, confirmedBytes, confirmedPartCount, totalPartCount));
    }

    private static async Task PutPartAsync(Uri uploadUri, byte[] content, MediaUploadConfiguration configuration,
        CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Exception? lastException = null;
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            TimeSpan remaining = configuration.RetryTimeout - stopwatch.Elapsed;
            if (remaining <= TimeSpan.Zero)
                break;

            using CancellationTokenSource requestCancellation = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            requestCancellation.CancelAfter(remaining);
            try
            {
                using HttpRequestMessage request = new(HttpMethod.Put, uploadUri)
                {
                    Content = new ByteArrayContent(content)
                };
                using HttpResponseMessage response = await UploadClient
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, requestCancellation.Token)
                    .ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    return;
                lastException = new HttpRequestException($"Media upload part PUT failed with HTTP {(int)response.StatusCode}.");
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                lastException = new TimeoutException("Media upload part PUT timed out.");
            }
            catch (Exception ex)
            {
                lastException = ex;
            }

            if (configuration.RetryDelay > TimeSpan.Zero)
                await Task.Delay(configuration.RetryDelay, cancellationToken).ConfigureAwait(false);
        } while (stopwatch.Elapsed < configuration.RetryTimeout);

        throw new HttpRequestException("Media upload part PUT failed before the server retry timeout elapsed.", lastException);
    }

    private static async Task ReadExactlyAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        int offset = 0;
        while (offset < buffer.Length)
        {
            int read = await stream.ReadAsync(buffer.AsMemory(offset), cancellationToken).ConfigureAwait(false);
            if (read == 0)
                throw new EndOfStreamException("The media upload source ended before a complete upload part was read.");
            offset += read;
        }
    }

    private sealed class UploadInput : IAsyncDisposable
    {
        public Stream Stream { get; }
        public long StartPosition { get; }
        private readonly bool _disposeStream;

        private UploadInput(Stream stream, bool disposeStream)
        {
            Stream = stream;
            StartPosition = stream.Position;
            _disposeStream = disposeStream;
        }

        public static Task<UploadInput> CreateAsync(MediaUploadSource source, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return source.Kind switch
            {
                MediaUploadSourceKind.FilePath when source.FilePath is not null => Task.FromResult(
                    new UploadInput(File.OpenRead(source.FilePath), true)),
                MediaUploadSourceKind.Stream when source.Stream is { CanRead: true, CanSeek: true } stream => Task.FromResult(
                    new UploadInput(stream, false)),
                MediaUploadSourceKind.Memory => Task.FromResult(
                    new UploadInput(new MemoryStream(source.Memory.ToArray(), writable: false), true)),
                _ => throw new InvalidOperationException("The media upload source is invalid.")
            };
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposeStream)
                await Stream.DisposeAsync().ConfigureAwait(false);
        }
    }
}
