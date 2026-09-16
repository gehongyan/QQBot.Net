namespace QQBot;

/// <summary>
///     表示机器人全局自定义菜单中的一个二级菜单项。
/// </summary>
public class SubMenuItem
{
    /// <summary>
    ///     获取此菜单项的名称，最多 14 个字符（一个中文汉字算 2 个字符）。
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     获取此菜单项的类型。
    /// </summary>
    public SubMenuItemType Type { get; }

    /// <summary>
    ///     获取用户点击后填入聊天输入框的内容；仅当类型为 <see cref="SubMenuItemType.SendMessage"/> 时有效。
    /// </summary>
    public string? SendMessage { get; }

    /// <summary>
    ///     获取用户点击后跳转的链接；仅当类型为 <see cref="SubMenuItemType.Link"/> 时有效。
    /// </summary>
    public string? Link { get; }

    private SubMenuItem(string name, SubMenuItemType type, string? sendMessage, string? link)
    {
        Name = name;
        Type = type;
        SendMessage = sendMessage;
        Link = link;
    }

    /// <summary>
    ///     创建一个发送消息类型的二级菜单项。
    /// </summary>
    /// <param name="name"> 菜单项名称。 </param>
    /// <param name="message"> 用户点击后填入聊天输入框的内容。 </param>
    public static SubMenuItem CreateSendMessage(string name, string message) =>
        new(name, SubMenuItemType.SendMessage, message, null);

    /// <summary>
    ///     创建一个链接跳转类型的二级菜单项。
    /// </summary>
    /// <param name="name"> 菜单项名称。 </param>
    /// <param name="url"> 用户点击后跳转的链接，必须以 <c>https://</c> 开头。 </param>
    public static SubMenuItem CreateLink(string name, string url) =>
        new(name, SubMenuItemType.Link, null, url);
}
