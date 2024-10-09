using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个键盘按钮。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class KeyboardButton
{
    /// <summary>
    ///     获取此按钮的 ID。
    /// </summary>
    /// <remarks>
    ///     每个 <see cref="KeyboardContent"/> 内的各个按钮的 ID 都应该是唯一的。
    /// </remarks>
    public string? Id { get; }

    /// <summary>
    ///     获取此按钮上的文本。
    /// </summary>
    public string Label { get; }

    /// <summary>
    ///     获取此按钮被点击后现实的文本。
    /// </summary>
    public string LabelVisited { get; }

    /// <summary>
    ///     获取此按钮的样式。
    /// </summary>
    public ButtonStyle Style { get; }

    /// <summary>
    ///     获取此按钮的动作类型。
    /// </summary>
    public ButtonAction Action { get; }

    /// <summary>
    ///     获取此按钮的权限。
    /// </summary>
    public ButtonPermission Permission { get; }

    /// <summary>
    ///     获取拥有操作此按钮的权限的所有用户的 ID。
    /// </summary>
    public IReadOnlyCollection<string>? AllowedUserIds { get; }

    /// <summary>
    ///     获取拥有操作此按钮的权限的所有角色的 ID。
    /// </summary>
    public IReadOnlyCollection<uint>? AllowedRoleIds { get; }

    /// <summary>
    ///     获取此按钮的数据。
    /// </summary>
    public string Data { get; }

    /// <summary>
    ///     获取此指令钮是否带引用回复。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="QQBot.KeyboardButton.Action"/> 为 <see cref="QQBot.ButtonAction.Command"/> 时有效。
    /// </remarks>
    public bool? IsCommandReply { get; }

    /// <summary>
    ///     获取此指令钮是否自动发送。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="QQBot.KeyboardButton.Action"/> 为 <see cref="QQBot.ButtonAction.Command"/> 时有效。
    /// </remarks>
    public bool? IsCommandAutoSend { get; }

    /// <summary>
    ///     获取此指定按钮的特殊操作。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="QQBot.KeyboardButton.Action"/> 为 <see cref="QQBot.ButtonAction.Command"/> 时有效。
    /// </remarks>
    public ButtonActionAnchor? ActionAnchor { get; }

    /// <summary>
    ///     获取客户端不支持此按钮时弹出的提示信息。
    /// </summary>
    public string UnsupportedVersionTip { get; }

    internal KeyboardButton(string? id, string label, string labelVisited, ButtonStyle style, ButtonAction action,
        ButtonPermission permission, IReadOnlyCollection<string>? allowedUserIds, IReadOnlyCollection<uint>? allowedRoleIds,
        string data, bool? isCommandReply, bool? isCommandAutoSend, ButtonActionAnchor? actionAnchor, string unsupportedVersionTip)
    {
        Id = id;
        Label = label;
        LabelVisited = labelVisited;
        Style = style;
        Action = action;
        Permission = permission;
        AllowedUserIds = allowedUserIds;
        AllowedRoleIds = allowedRoleIds;
        Data = data;
        IsCommandReply = isCommandReply;
        IsCommandAutoSend = isCommandAutoSend;
        ActionAnchor = actionAnchor;
        UnsupportedVersionTip = unsupportedVersionTip;
    }

    private string DebuggerDisplay => $"{Label} ({Action}{(Id is null ? "" : $", {Id}")})";
}
