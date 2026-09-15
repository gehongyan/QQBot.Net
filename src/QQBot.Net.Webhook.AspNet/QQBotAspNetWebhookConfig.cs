using QQBot.Net.Webhooks.AspNet;

namespace QQBot.Webhook.AspNet;

/// <summary>
///     表示 QQ Bot ASP.NET Webhook 客户端配置。
/// </summary>
public class QQBotAspNetWebhookConfig : QQBotWebhookConfig
{
    /// <summary>
    ///     获取或设置机器人 AppId。
    /// </summary>
    public int? AppId { get; set; }

    /// <summary>
    ///     获取或设置登录前是否验证机器人密钥格式。
    /// </summary>
    public bool ValidateToken { get; set; } = true;

    /// <summary>
    ///     获取或设置 Webhook 路由模式。
    /// </summary>
    public string RoutePattern { get; set; } = "/qqbot";

    /// <summary>
    ///     初始化一个 <see cref="QQBotAspNetWebhookConfig"/> 实例。
    /// </summary>
    public QQBotAspNetWebhookConfig()
        : base(DefaultAspNetWebhookProvider.Instance)
    {
    }
}
