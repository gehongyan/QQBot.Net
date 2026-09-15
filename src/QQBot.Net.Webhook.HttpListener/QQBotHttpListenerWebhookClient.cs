using QQBot.Net.Webhooks.HttpListener;

namespace QQBot.Webhook.HttpListener;

/// <summary>
///     表示一个使用 <see cref="System.Net.HttpListener"/> 的 QQ Bot Webhook 客户端。
/// </summary>
public sealed class QQBotHttpListenerWebhookClient : QQBotWebhookClient
{
    /// <summary>
    ///     使用指定配置初始化客户端。
    /// </summary>
    /// <param name="config">HttpListener Webhook 配置。</param>
    public QQBotHttpListenerWebhookClient(QQBotHttpListenerWebhookConfig config)
        : base(config)
    {
    }

    internal new QQBotHttpListenerWebhookConfig BaseConfig =>
        base.BaseConfig as QQBotHttpListenerWebhookConfig
        ?? throw new InvalidOperationException("The configuration is not an HttpListener Webhook configuration.");

    /// <inheritdoc />
    public override async Task StartAsync()
    {
        if (ApiClient.WebhookClient is not IHttpListenerWebhookClient transport)
            throw new InvalidOperationException("The configured Webhook transport is not an HttpListener transport.");
        if (BaseConfig.UriPrefixes is not { Count: > 0 })
            throw new InvalidOperationException("At least one URI prefix is required.");

        await WebhookLogger.InfoAsync("Starting the QQ Bot Webhook client.").ConfigureAwait(false);
        await base.StartAsync().ConfigureAwait(false);
        await transport.StartAsync(BaseConfig.UriPrefixes, TransportCancellationToken).ConfigureAwait(false);
        transport.Closed += OnTransportClosedAsync;
        await WebhookLogger.InfoAsync("The QQ Bot Webhook client has started.").ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task StopAsync()
    {
        if (ApiClient.WebhookClient is not IHttpListenerWebhookClient transport)
            throw new InvalidOperationException("The configured Webhook transport is not an HttpListener transport.");
        transport.Closed -= OnTransportClosedAsync;
        await transport.StopAsync().ConfigureAwait(false);
        await base.StopAsync().ConfigureAwait(false);
    }

    private async Task OnTransportClosedAsync(Exception exception)
    {
        await WebhookLogger.ErrorAsync("The HTTP listener has been closed.", exception).ConfigureAwait(false);
        if (BaseConfig.AutoRestartInterval == Timeout.InfiniteTimeSpan)
            return;
        if (BaseConfig.AutoRestartInterval < TimeSpan.Zero)
            throw new InvalidOperationException("The HTTP listener was closed and automatic restart is disabled.", exception);

        await Task.Delay(BaseConfig.AutoRestartInterval, TransportCancellationToken).ConfigureAwait(false);
        if (ApiClient.WebhookClient is IHttpListenerWebhookClient transport
            && BaseConfig.UriPrefixes is { Count: > 0 })
            await transport.StartAsync(BaseConfig.UriPrefixes, TransportCancellationToken).ConfigureAwait(false);
    }
}
