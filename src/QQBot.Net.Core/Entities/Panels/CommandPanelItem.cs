namespace QQBot;

/// <summary>
///     表示指令面板中的一个元素。
/// </summary>
public class CommandPanelItem
{
    /// <summary>
    ///     获取此元素的名称，最多 14 个字符（一个中文汉字算 2 个字符）。
    /// </summary>
    /// <remarks>
    ///     当类型为 <see cref="CommandPanelItemType.Command"/> 时，用户点击后该内容会填入聊天输入框；
    ///     当类型为 <see cref="CommandPanelItemType.Link"/> 时，仅用于面板展示。
    /// </remarks>
    public string Name { get; }

    /// <summary>
    ///     获取此元素的描述，最多 30 个字符，在面板中展示给用户。
    /// </summary>
    public string? Description { get; }

    /// <summary>
    ///     获取此元素的类型。
    /// </summary>
    public CommandPanelItemType Type { get; }

    /// <summary>
    ///     获取此元素是否仅管理员可操作。
    /// </summary>
    public bool OnlyAdmin { get; }

    /// <summary>
    ///     获取用户点击后跳转的链接；仅当类型为 <see cref="CommandPanelItemType.Link"/> 时有效。
    /// </summary>
    public string? Link { get; }

    private CommandPanelItem(string name, string? description, CommandPanelItemType type,
        bool onlyAdmin, string? link)
    {
        Name = name;
        Description = description;
        Type = type;
        OnlyAdmin = onlyAdmin;
        Link = link;
    }

    /// <summary>
    ///     创建一个指令类型的面板元素。
    /// </summary>
    /// <param name="name"> 元素名称，用户点击后将填入聊天输入框。 </param>
    /// <param name="description"> 元素描述。 </param>
    /// <param name="onlyAdmin"> 是否仅管理员可操作。 </param>
    public static CommandPanelItem CreateCommand(string name, string? description = null, bool onlyAdmin = false) =>
        new(name, description, CommandPanelItemType.Command, onlyAdmin, null);

    /// <summary>
    ///     创建一个链接跳转类型的面板元素。
    /// </summary>
    /// <param name="name"> 元素名称，用于面板展示。 </param>
    /// <param name="url"> 用户点击后跳转的链接，必须以 <c>https://</c> 开头。 </param>
    /// <param name="description"> 元素描述。 </param>
    /// <param name="onlyAdmin"> 是否仅管理员可操作。 </param>
    public static CommandPanelItem CreateLink(string name, string url, string? description = null, bool onlyAdmin = false) =>
        new(name, description, CommandPanelItemType.Link, onlyAdmin, url);
}
