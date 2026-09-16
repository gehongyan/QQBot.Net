namespace QQBot;

/// <summary>
///     表示机器人全局自定义菜单中二级菜单项的类型。
/// </summary>
public enum SubMenuItemType
{
    /// <summary>
    ///     发送消息项，用户点击后将内容填入聊天输入框。
    /// </summary>
    SendMessage,

    /// <summary>
    ///     链接跳转项。
    /// </summary>
    Link
}
