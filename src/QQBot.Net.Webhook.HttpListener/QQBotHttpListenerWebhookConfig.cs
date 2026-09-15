using QQBot.Net.Webhooks.HttpListener;

namespace QQBot.Webhook.HttpListener;

/// <summary>
///     表示 QQ Bot HttpListener Webhook 客户端配置。
/// </summary>
public class QQBotHttpListenerWebhookConfig : QQBotWebhookConfig
{
    /// <summary>
    ///     初始化一个 <see cref="QQBotHttpListenerWebhookConfig"/> 实例。
    /// </summary>
    public QQBotHttpListenerWebhookConfig()
        : base(DefaultHttpListenerWebhookProvider.Instance)
    {
    }

    /// <summary>
    ///     获取或设置监听传入 Webhook 请求的 URI 前缀。
    /// </summary>
    public IReadOnlyCollection<string>? UriPrefixes { get; set; }

    /// <summary>
    ///     获取或设置监听器意外关闭后的自动重启间隔。
    /// </summary>
    public TimeSpan AutoRestartInterval { get; set; } = TimeSpan.FromSeconds(5);
}
