namespace QQBot.Net.Webhooks;

/// <summary>
///     表示一个 QQ Bot Webhook 请求。
/// </summary>
/// <param name="Body">HTTP 请求体。</param>
/// <param name="Signature">请求头 X-Signature-Ed25519 的值。</param>
/// <param name="Timestamp">请求头 X-Signature-Timestamp 的值。</param>
/// <param name="AppId">请求头 X-Bot-Appid 的值。</param>
public readonly record struct WebhookRequest(
    string Body,
    string? Signature,
    string? Timestamp,
    string? AppId);
