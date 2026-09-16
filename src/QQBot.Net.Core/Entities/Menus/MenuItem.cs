using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示机器人全局自定义菜单中的一个一级菜单项。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MenuItem
{
    /// <summary>
    ///     获取此菜单项的名称，最多 10 个字符（一个中文汉字算 2 个字符）。
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     获取此菜单项的类型。
    /// </summary>
    public MenuItemType Type { get; }

    /// <summary>
    ///     获取用户点击后填入聊天输入框的内容；仅当类型为 <see cref="MenuItemType.SendMessage"/> 时有效。
    /// </summary>
    public string? SendMessage { get; }

    /// <summary>
    ///     获取用户点击后跳转的链接；仅当类型为 <see cref="MenuItemType.Link"/> 时有效。
    /// </summary>
    public string? Link { get; }

    /// <summary>
    ///     获取开关配置；仅当类型为 <see cref="MenuItemType.Switch"/> 时有效。
    /// </summary>
    public MenuSwitch? Switch { get; }

    /// <summary>
    ///     获取子菜单项列表，最多 5 个；仅当类型为 <see cref="MenuItemType.Menu"/> 时有效。
    /// </summary>
    public IReadOnlyCollection<SubMenuItem> SubMenuItems { get; }

    private MenuItem(string name, MenuItemType type, string? sendMessage, string? link,
        MenuSwitch? @switch, IReadOnlyCollection<SubMenuItem> subMenuItems)
    {
        Name = name;
        Type = type;
        SendMessage = sendMessage;
        Link = link;
        Switch = @switch;
        SubMenuItems = subMenuItems;
    }

    /// <summary>
    ///     创建一个发送消息类型的菜单项。
    /// </summary>
    /// <param name="name"> 菜单项名称。 </param>
    /// <param name="message"> 用户点击后填入聊天输入框的内容。 </param>
    public static MenuItem CreateSendMessage(string name, string message) =>
        new(name, MenuItemType.SendMessage, message, null, null, []);

    /// <summary>
    ///     创建一个链接跳转类型的菜单项。
    /// </summary>
    /// <param name="name"> 菜单项名称。 </param>
    /// <param name="url"> 用户点击后跳转的链接，必须以 <c>https://</c> 开头。 </param>
    public static MenuItem CreateLink(string name, string url) =>
        new(name, MenuItemType.Link, null, url, null, []);

    /// <summary>
    ///     创建一个开关类型的菜单项。
    /// </summary>
    /// <param name="name"> 菜单项名称。 </param>
    /// <param name="switchId"> 开关的唯一标识。 </param>
    /// <param name="isDefaultOn"> 开关的初始状态是否为打开。 </param>
    public static MenuItem CreateSwitch(string name, string switchId, bool isDefaultOn = false) =>
        new(name, MenuItemType.Switch, null, null, new MenuSwitch(switchId, isDefaultOn), []);

    /// <summary>
    ///     创建一个含子菜单的折叠菜单项。
    /// </summary>
    /// <param name="name"> 菜单项名称。 </param>
    /// <param name="subMenuItems"> 子菜单项列表，最多 5 个。 </param>
    public static MenuItem CreateSubMenu(string name, IEnumerable<SubMenuItem> subMenuItems) =>
        new(name, MenuItemType.Menu, null, null, null, subMenuItems.ToArray());

    private string DebuggerDisplay => $"{Name} ({Type})";
}
