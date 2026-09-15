using QQBot.Net.Webhooks;
using QQBot.WebSocket;

namespace QQBot.Webhook;

/// <summary>
///     表示一个用于 <see cref="QQBotWebhookClient"/> 的配置类。
/// </summary>
public abstract class QQBotWebhookConfig : QQBotSocketConfig
{
    /// <summary>
    ///     获取或设置用于验证 Webhook 请求及生成验证响应签名的机器人密钥。
    /// </summary>
    public string? Secret { get; set; }

    /// <summary>
    ///     获取或设置用于创建 Webhook 传输客户端的委托。
    /// </summary>
    public WebhookProvider WebhookProvider { get; set; }

    /// <summary>
    ///     获取或设置客户端是否尝试自动登录。
    /// </summary>
    public bool AutoLogin { get; set; } = true;

    /// <summary>
    ///     获取或设置客户端停止时是否尝试自动退出登录。
    /// </summary>
    public bool AutoLogout { get; set; }

    /// <summary>
    ///     使用指定传输提供程序初始化配置。
    /// </summary>
    /// <param name="webhookProvider">Webhook 传输提供程序。</param>
    protected QQBotWebhookConfig(WebhookProvider webhookProvider)
    {
        WebhookProvider = webhookProvider;
        HandlerTimeout = 1000;
    }
}
