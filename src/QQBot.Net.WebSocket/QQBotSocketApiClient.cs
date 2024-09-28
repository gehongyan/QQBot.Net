using System.IO.Compression;
using System.Text;
#if DEBUG_PACKETS
using System.Diagnostics;
using System.Text.Encodings.Web;
#endif
using System.Text.Json;
using QQBot.API.Gateway;
using QQBot.API.Rest;
using QQBot.Net.Queue;
using QQBot.Net.Rest;
using QQBot.Net.WebSockets;
using QQBot.WebSocket;

namespace QQBot.API;

internal class QQBotSocketApiClient : QQBotRestApiClient
{
    public event Func<GatewayOpCode, Task> SentGatewayMessage
    {
        add => _sentGatewayMessageEvent.Add(value);
        remove => _sentGatewayMessageEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<GatewayOpCode, Task>> _sentGatewayMessageEvent = new();

    public event Func<GatewayOpCode, int?, string?, object?, Task> ReceivedGatewayEvent
    {
        add => _receivedGatewayEvent.Add(value);
        remove => _receivedGatewayEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<GatewayOpCode, int?, string?, object?, Task>> _receivedGatewayEvent = new();

    public event Func<Exception, Task> Disconnected
    {
        add => _disconnectedEvent.Add(value);
        remove => _disconnectedEvent.Remove(value);
    }

    private readonly AsyncEvent<Func<Exception, Task>> _disconnectedEvent = new();

    private readonly bool _isExplicitUrl;
    private CancellationTokenSource? _connectCancellationToken;
    private string? _gatewayUrl;

#if DEBUG_PACKETS
    private readonly JsonSerializerOptions _debugJsonSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
#endif

    public ConnectionState ConnectionState { get; private set; }

    /// <summary>
    ///     Sets the gateway URL used for identifies.
    /// </summary>
    /// <remarks>
    ///     If a custom URL is set, setting this property does nothing.
    /// </remarks>
    public string? GatewayUrl
    {
        set
        {
            // Makes the sharded client not override the custom value.
            if (_isExplicitUrl)
                return;
            _gatewayUrl = value;
        }
    }

    internal IWebSocketClient WebSocketClient { get; }

    public QQBotSocketApiClient(RestClientProvider restClientProvider,
        WebSocketProvider webSocketProvider,
        AccessEnvironment accessEnvironment, string userAgent, string? url = null,
        RetryMode defaultRetryMode = RetryMode.AlwaysRetry,
        JsonSerializerOptions? serializerOptions = null,
        Func<IRateLimitInfo, Task>? defaultRatelimitCallback = null)
        : base(restClientProvider, accessEnvironment, userAgent,
            defaultRetryMode, serializerOptions, defaultRatelimitCallback)
    {
        _gatewayUrl = url;
        if (url != null)
            _isExplicitUrl = true;
        WebSocketClient = webSocketProvider();
        WebSocketClient.SetKeepAliveInterval(TimeSpan.Zero);
        WebSocketClient.TextMessage += OnTextMessage;
        WebSocketClient.BinaryMessage += OnBinaryMessage;
        WebSocketClient.Closed += async ex =>
        {
#if DEBUG_PACKETS
            Debug.WriteLine(ex);
#endif
            await DisconnectAsync().ConfigureAwait(false);
            await _disconnectedEvent.InvokeAsync(ex).ConfigureAwait(false);
        };
    }

    private async Task OnBinaryMessage(byte[] data, int index, int count)
    {
        await using MemoryStream decompressed = new();
        using MemoryStream compressed = data[0] == 0x78
            ? new MemoryStream(data, index + 2, count - 2)
            : new MemoryStream(data, index, count);
        await using DeflateStream decompressor = new(compressed, CompressionMode.Decompress);
        await decompressor.CopyToAsync(decompressed);
        decompressed.Position = 0;

        GatewaySocketFrame? gatewaySocketFrame = JsonSerializer
            .Deserialize<GatewaySocketFrame>(decompressed, _serializerOptions);
        if (gatewaySocketFrame is not null)
        {
#if DEBUG_PACKETS
            string raw = Encoding.Default.GetString(decompressed.ToArray()).TrimEnd('\n');
            string parsed = JsonSerializer
                .Serialize(gatewaySocketFrame.Payload, _debugJsonSerializerOptions)
                .TrimEnd('\n');
            Debug.WriteLine($"""
                [{DateTimeOffset.Now:HH:mm:ss}] <- [{gatewaySocketFrame.OpCode}] : #{gatewaySocketFrame.Sequence}
                [Raw] {raw}
                [Parsed] {parsed}
                """);
#endif
            await _receivedGatewayEvent
                .InvokeAsync(gatewaySocketFrame.OpCode, gatewaySocketFrame.Sequence, gatewaySocketFrame.Type, gatewaySocketFrame.Payload)
                .ConfigureAwait(false);
        }
    }

    private async Task OnTextMessage(string message)
    {
        GatewaySocketFrame? gatewaySocketFrame = JsonSerializer.Deserialize<GatewaySocketFrame>(message, _serializerOptions);
        if (gatewaySocketFrame is null)
            return;
#if DEBUG_PACKETS
        string parsed = JsonSerializer
            .Serialize(gatewaySocketFrame.Payload, _debugJsonSerializerOptions)
            .TrimEnd('\n');
        Debug.WriteLine($"""
            [{DateTimeOffset.Now:HH:mm:ss}] <- [{gatewaySocketFrame.OpCode}] : #{gatewaySocketFrame.Sequence}
            [Raw] {message}
            [Parsed] {parsed}
            """);
#endif
        await _receivedGatewayEvent
            .InvokeAsync(gatewaySocketFrame.OpCode, gatewaySocketFrame.Sequence, gatewaySocketFrame.Type, gatewaySocketFrame.Payload)
            .ConfigureAwait(false);
    }

    internal override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _connectCancellationToken?.Dispose();
                WebSocketClient?.Dispose();
            }

            _isDisposed = true;
        }

        base.Dispose(disposing);
    }

    public async Task ConnectAsync()
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await ConnectInternalAsync().ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    internal override async Task ConnectInternalAsync()
    {
        if (LoginState != LoginState.LoggedIn)
            throw new InvalidOperationException("The client must be logged in before connecting.");
        if (WebSocketClient == null)
            throw new NotSupportedException("This client is not configured with WebSocket support.");
        RequestQueue.ClearGatewayBuckets();
        ConnectionState = ConnectionState.Connecting;
        try
        {
            _connectCancellationToken?.Dispose();
            _connectCancellationToken = new CancellationTokenSource();
            WebSocketClient.SetCancellationToken(_connectCancellationToken.Token);

            if (!_isExplicitUrl || _gatewayUrl == null)
            {
                GetGatewayResponse gatewayResponse = await GetGatewayAsync().ConfigureAwait(false);
                _gatewayUrl = gatewayResponse.Url;
            }
#if DEBUG_PACKETS
            Debug.WriteLine("Connecting to gateway: " + _gatewayUrl);
#endif
            await WebSocketClient.ConnectAsync(_gatewayUrl).ConfigureAwait(false);
            ConnectionState = ConnectionState.Connected;
        }
        catch (Exception)
        {
            if (!_isExplicitUrl) _gatewayUrl = null;

            await DisconnectInternalAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task DisconnectAsync(Exception? ex = null)
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await DisconnectInternalAsync(ex).ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    internal override async Task DisconnectInternalAsync(Exception? ex = null)
    {
        if (WebSocketClient == null)
            throw new NotSupportedException("This client is not configured with WebSocket support.");
        if (ConnectionState == ConnectionState.Disconnected)
            return;
        ConnectionState = ConnectionState.Disconnecting;

        if (ex is GatewayReconnectException)
            await WebSocketClient.DisconnectAsync(4000).ConfigureAwait(false);
        else
            await WebSocketClient.DisconnectAsync().ConfigureAwait(false);

        try
        {
            _connectCancellationToken?.Cancel(false);
        }
        catch
        {
            // ignored
        }

        ConnectionState = ConnectionState.Disconnected;
    }

    public async Task SendHeartbeatAsync(int? lastSeq, RequestOptions? options = null)
    {
        RequestOptions requestOptions = RequestOptions.CreateOrClone(options);
        await SendGatewayAsync(GatewayOpCode.Heartbeat, lastSeq, requestOptions).ConfigureAwait(false);
    }

    public Task SendIdentifyAsync(int shardId = 0, int totalShards = 1,
        GatewayIntents gatewayIntents = GatewayIntents.All, RequestOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(AuthToken, nameof(AuthToken));
        ArgumentNullException.ThrowIfNull(AppId, nameof(AppId));
        RequestOptions requestOptions = RequestOptions.CreateOrClone(options);
        Dictionary<string, string> props = new()
        {
            ["$device"] = "QQBot.Net",
            ["$os"] = Environment.OSVersion.Platform.ToString(),
            ["$browser"] = "QQBot.Net"
        };
        IdentifyParams identifyParams = new()
        {
            Token = GetPrefixedToken(AuthTokenType, AppId.Value, AuthToken),
            Intents = (int)gatewayIntents,
            ShardingParams = [shardId, totalShards],
            Properties = props
        };
        requestOptions.BucketId = GatewayBucket.Get(GatewayBucketType.Identify).Id;
        return SendGatewayAsync(GatewayOpCode.Identify, identifyParams, requestOptions);
    }

    public async Task SendResumeAsync(Guid sessionId, int? lastSeq, RequestOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(AuthToken, nameof(AuthToken));
        ArgumentNullException.ThrowIfNull(AppId, nameof(AppId));
        RequestOptions requestOptions = RequestOptions.CreateOrClone(options);
        ResumeParams resumeParams = new()
        {
            Token = GetPrefixedToken(AuthTokenType, AppId.Value, AuthToken),
            SessionId = sessionId,
            Sequence = lastSeq ?? 0
        };
        await SendGatewayAsync(GatewayOpCode.Resume, resumeParams, requestOptions).ConfigureAwait(false);
    }

    public Task SendGatewayAsync(GatewayOpCode opCode, object? payload, RequestOptions options) =>
        SendGatewayInternalAsync(opCode, payload, options);

    private async Task SendGatewayInternalAsync(GatewayOpCode opCode, object? payload, RequestOptions options)
    {
        CheckState();
        GatewaySocketFrame frame = new()
        {
            OpCode = opCode,
            Payload = payload
        };
        string json = SerializeJson(frame);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        options.IsGatewayBucket = true;
        options.BucketId ??= GatewayBucket.Get(GatewayBucketType.Unbucketed).Id;
        bool ignoreLimit = opCode == GatewayOpCode.Heartbeat;
        await RequestQueue
            .SendAsync(new WebSocketRequest(WebSocketClient, bytes, true, ignoreLimit, options))
            .ConfigureAwait(false);
        await _sentGatewayMessageEvent.InvokeAsync(opCode).ConfigureAwait(false);

#if DEBUG_PACKETS
        string payloadString = JsonSerializer.Serialize(payload, _debugJsonSerializerOptions);
        Debug.WriteLine($"[{DateTimeOffset.Now:HH:mm:ss}] -> [{opCode}] {payloadString}".TrimEnd('\n'));
#endif
    }
}
