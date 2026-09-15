using System.Net;

namespace QQBot.Net.Webhooks;

/// <summary>
///     表示一个 QQ Bot Webhook 响应。
/// </summary>
/// <param name="StatusCode">HTTP 状态码。</param>
/// <param name="Body">响应体；如果没有响应体，则为 <see langword="null"/>。</param>
public readonly record struct WebhookResponse(HttpStatusCode StatusCode, string? Body = null)
{
    /// <summary>
    ///     获取表示成功且无响应体的响应。
    /// </summary>
    public static WebhookResponse NoContent { get; } = new(HttpStatusCode.NoContent);
}
