using QQBot.API;
using QQBot.Logging;
using QQBot.Rest;
using QQBot.WebSocket;

namespace QQBot.Webhook;

/// <summary>
///     表示一个基于 Webhook 网关的 QQ Bot 客户端。
/// </summary>
public abstract class QQBotWebhookClient : QQBotSocketClient
{
    private readonly SemaphoreSlim _stateLock;
    private bool _isDisposed;

    private protected Logger WebhookLogger { get; }


    /// <summary>
    ///     获取 Webhook 传输应使用的取消令牌。
    /// </summary>
    protected CancellationToken TransportCancellationToken => Connection.CancellationToken;

    /// <summary>
    ///     使用指定配置初始化一个 <see cref="QQBotWebhookClient"/> 实例。
    /// </summary>
    /// <param name="config">Webhook 客户端配置。</param>
    protected QQBotWebhookClient(QQBotWebhookConfig config)
        : this(config, CreateApiClient(config))
    {
    }

    private protected QQBotWebhookClient(QQBotWebhookConfig config, QQBotWebhookApiClient client)
        : base(config, client, null, null)
    {
        WebhookLogger = LogManager.CreateLogger("Webhook");
        ApiClient.WebhookValidation += OnWebhookValidationAsync;

        _stateLock = new SemaphoreSlim(1, 1);

        ConnectionManager connectionManager = new(
            _stateLock,
            WebhookLogger,
            config.ConnectionTimeout,
            OnConnectingAsync,
            OnDisconnectingAsync,
            handler => ApiClient.Disconnected += handler);
        connectionManager.Connected += () => TimedInvokeAsync(_connectedEvent, nameof(Connected));
        connectionManager.Disconnected += (ex, _) =>
            TimedInvokeAsync(_disconnectedEvent, nameof(Disconnected), ex);
        Connection = connectionManager;
    }

    internal override ConnectionManager Connection { get; }

    internal new QQBotWebhookApiClient ApiClient => base.ApiClient as QQBotWebhookApiClient
        ?? throw new InvalidOperationException("The API client is not a Webhook-based client.");

    internal new QQBotWebhookConfig BaseConfig => base.BaseConfig as QQBotWebhookConfig
        ?? throw new InvalidOperationException("The configuration is not a Webhook configuration.");

    private async Task OnConnectingAsync()
    {
        await WebhookLogger.DebugAsync("Initializing the Webhook client.").ConfigureAwait(false);
        await FetchRequiredDataAsync().ConfigureAwait(false);
    }


    /// <summary>
    ///     当平台验证 Webhook 回调地址时调用。
    /// </summary>
    protected virtual Task OnWebhookValidationAsync() =>
        WebhookLogger.DebugAsync("Received Webhook validation request.");

    private Task OnDisconnectingAsync(Exception ex)
    {
        ResetCounter();
        return Task.CompletedTask;
    }

    private static QQBotWebhookApiClient CreateApiClient(QQBotWebhookConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.Secret))
            throw new InvalidOperationException("The bot secret is required.");

        return new QQBotWebhookApiClient(
            config.RestClientProvider,
            config.WebSocketProvider,
            config.WebhookProvider,
            config.AccessEnvironment,
            QQBotConfig.UserAgent,
            config.Secret,
            config.GatewayHost,
            defaultRatelimitCallback: config.DefaultRatelimitCallback);
    }

    internal override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                try
                {
                    StopAsync().GetAwaiter().GetResult();
                }
                catch (NotSupportedException)
                {
                    // Hosted transports do not support manual stopping.
                }
                ApiClient.Dispose();
                _stateLock.Dispose();
            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }
}
