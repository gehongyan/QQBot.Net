namespace QQBot.Net.Webhooks.AspNet;

/// <summary>
///     提供默认 ASP.NET Webhook 传输客户端。
/// </summary>
public static class DefaultAspNetWebhookProvider
{
    /// <summary>
    ///     获取默认 ASP.NET Webhook 传输提供程序。
    /// </summary>
    public static readonly WebhookProvider Instance = () => new AspNetWebhookClient();
}
