namespace QQBot.Net.Webhooks.HttpListener;

/// <summary>
///     提供默认 HttpListener Webhook 传输客户端。
/// </summary>
public static class DefaultHttpListenerWebhookProvider
{
    /// <summary>
    ///     获取默认 HttpListener Webhook 传输提供程序。
    /// </summary>
    public static readonly WebhookProvider Instance = () => new HttpListenerWebhookClient();
}
