namespace QQBot;

/// <summary>
///     表示用户或群对机器人订阅消息模板的授权操作。
/// </summary>
public enum SubscribeMessageAuthorization
{
    /// <summary>
    ///     允许接收此模板的订阅消息推送。
    /// </summary>
    Allowed = 1,

    /// <summary>
    ///     拒绝接收此模板的订阅消息推送。
    /// </summary>
    Rejected = 2
}
