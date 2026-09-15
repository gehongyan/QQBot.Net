namespace QQBot.Net.Webhooks;

/// <summary>
///     表示一个通用的 QQ Bot Webhook 传输客户端。
/// </summary>
public interface IWebhookClient : IDisposable
{
    /// <summary>
    ///     当接收到 Webhook 请求时引发。
    /// </summary>
    event Func<WebhookRequest, Task<WebhookResponse>>? Request;

    /// <summary>
    ///     处理一个 Webhook 请求。
    /// </summary>
    /// <param name="request">Webhook 请求。</param>
    /// <returns>Webhook 响应。</returns>
    Task<WebhookResponse> HandleRequestAsync(WebhookRequest request);
}
