using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个键盘按钮。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class KeyboardButtonBuilder
{
    /// <summary>
    ///     获取或设置此按钮的 ID。
    /// </summary>
    /// <remarks>
    ///     每个 <see cref="KeyboardContent"/> 内的各个按钮的 ID 都应该是唯一的。
    /// </remarks>
    public string? Id { get; set; }

    /// <summary>
    ///     获取或设置此按钮上的文本。
    /// </summary>
    /// <remarks>
    ///     未设置时，将使用 <see cref="Id"/> 的值，此时 <see cref="Id"/> 必须被设置。
    /// </remarks>
    public string? Label { get; set; }

    /// <summary>
    ///     获取或设置此按钮被点击后现实的文本。
    /// </summary>
    /// <remarks>
    ///     为设置时，将使用 <see cref="Label"/> 的值，若 <see cref="Label"/> 也未设置，则使用
    ///     <see cref="Id"/> 的值，此时 <see cref="Id"/> 必须被设置。
    /// </remarks>
    public string? LabelVisited { get; set; }

    /// <summary>
    ///     获取或设置此按钮的样式。
    /// </summary>
    /// <remarks>
    ///     未设置时，将使用 <see cref="ButtonStyle.Blue"/>。
    /// </remarks>
    public ButtonStyle? Style { get; set; }

    /// <summary>
    ///     获取或设置此按钮的动作类型。
    /// </summary>
    /// <remarks>
    ///     未设置时，将根据以下规则尝试推断：
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 当 <see cref="IsCommandReply"/>、<see cref="IsCommandAutoSend"/> 或 <see cref="ActionAnchor"/>
    ///                 任一属性被设置时，将使用 <see cref="ButtonAction.Command"/>。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 当 <see cref="Data"/> 不为 <see langword="null"/> 时，将调用
    ///                 <see cref="Uri.TryCreate(string, UriKind, out Uri)"/> 方法，第一个参数为 <see cref="Data"/>
    ///                 属性的值，第二个参数为 <see cref="UriKind.Absolute"/>，若返回 <see langword="true"/>，则使用
    ///                 <see cref="ButtonAction.Jump"/>。
    ///         </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 其他情况下，使用 <see cref="ButtonAction.Callback"/>。
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public ButtonAction? Action { get; set; }

    /// <summary>
    ///     获取或设置此按钮的权限。
    /// </summary>
    /// <remarks>
    ///     未设置时，将使用 <see cref="ButtonPermission.Everyone"/>。
    /// </remarks>
    public ButtonPermission? Permission { get; set; }

    /// <summary>
    ///     获取或设置拥有操作此按钮的权限的所有用户的 ID。
    /// </summary>
    public List<string>? AllowedUserIds { get; set; }

    /// <summary>
    ///     获取或设置拥有操作此按钮的权限的所有角色的 ID。
    /// </summary>
    public List<uint>? AllowedRoleIds { get; set; }

    /// <summary>
    ///     获取或设置此按钮的数据。
    /// </summary>
    /// <remarks>
    ///     未设置时，将使用 <see cref="Id"/> 的值，若 <see cref="Id"/> 未设置，则使用 <see cref="Label"/>
    ///     的值，此时 <see cref="Label"/> 必须被设置。
    /// </remarks>
    public string? Data { get; set; }

    /// <summary>
    ///     获取或设置此指令钮是否带引用回复。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="QQBot.KeyboardButton.Action"/> 为 <see cref="QQBot.ButtonAction.Command"/> 时有效。
    /// </remarks>
    public bool? IsCommandReply { get; set; }

    /// <summary>
    ///     获取或设置此指令钮是否自动发送。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="QQBot.KeyboardButton.Action"/> 为 <see cref="QQBot.ButtonAction.Command"/> 时有效。
    /// </remarks>
    public bool? IsCommandAutoSend { get; set; }

    /// <summary>
    ///     获取或设置此指定按钮的特殊操作。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="QQBot.KeyboardButton.Action"/> 为 <see cref="QQBot.ButtonAction.Command"/> 时有效。
    /// </remarks>
    public ButtonActionAnchor? ActionAnchor { get; set; }

    /// <summary>
    ///     获取或设置客户端不支持此按钮时弹出的提示信息。
    /// </summary>
    /// <remarks>
    ///     未设置时，将使用 <see cref="Label"/> 的值，若 <see cref="Label"/> 也未设置，则使用
    ///     <see cref="Id"/> 的值，此时 <see cref="Id"/> 必须被设置。
    /// </remarks>
    public string? UnsupportedVersionTip { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardButtonBuilder"/> 类的新实例。
    /// </summary>
    public KeyboardButtonBuilder()
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardButtonBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="id"> 此按钮的 ID。 </param>
    /// <param name="label"> 按钮上的文本。 </param>
    /// <param name="labelVisited"> 按钮被点击后现实的文本。 </param>
    /// <param name="style"> 此按钮的样式。 </param>
    /// <param name="action"> 此按钮的动作类型。 </param>
    /// <param name="permission"> 此按钮的权限。 </param>
    /// <param name="allowedUserIds"> 拥有操作此按钮的权限的所有用户的 ID。 </param>
    /// <param name="allowedRoleIds"> 拥有操作此按钮的权限的所有角色的 ID。 </param>
    /// <param name="data"> 按钮的数据。 </param>
    /// <param name="isCommandReply"> 此指令钮是否带引用回复。 </param>
    /// <param name="isCommandAutoSend"> 此指令钮是否自动发送。 </param>
    /// <param name="actionAnchor"> 此指定按钮的特殊操作。 </param>
    /// <param name="unsupportedVersionTip"> 客户端不支持此按钮时弹出的提示信息。 </param>
    public KeyboardButtonBuilder(string? id = null, string? label = null, string? labelVisited = null,
        ButtonStyle? style = null, ButtonAction? action = null, ButtonPermission? permission = null,
        IEnumerable<string>? allowedUserIds = null, IEnumerable<uint>? allowedRoleIds = null, string? data = null,
        bool? isCommandReply = null, bool? isCommandAutoSend = null, ButtonActionAnchor? actionAnchor = null,
        string? unsupportedVersionTip = null)
    {
        Id = id;
        Label = label;
        LabelVisited = labelVisited;
        Style = style;
        Action = action;
        Permission = permission;
        AllowedUserIds = allowedUserIds?.ToList();
        AllowedRoleIds = allowedRoleIds?.ToList();
        Data = data;
        IsCommandReply = isCommandReply;
        IsCommandAutoSend = isCommandAutoSend;
        ActionAnchor = actionAnchor;
        UnsupportedVersionTip = unsupportedVersionTip;
    }

    /// <summary>
    ///     设置此按钮的 ID。
    /// </summary>
    /// <param name="id"> 此按钮的 ID。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithId(string id)
    {
        Id = id;
        return this;
    }

    /// <summary>
    ///     设置此按钮的文本。
    /// </summary>
    /// <param name="label"> 此按钮的文本。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithLabel(string label)
    {
        Label = label;
        return this;
    }

    /// <summary>
    ///     设置此按钮被点击后现实的文本。
    /// </summary>
    /// <param name="labelVisited"> 此按钮被点击后现实的文本。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithLabelVisited(string labelVisited)
    {
        LabelVisited = labelVisited;
        return this;
    }

    /// <summary>
    ///     设置此按钮的样式。
    /// </summary>
    /// <param name="style"> 此按钮的样式。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithStyle(ButtonStyle style)
    {
        Style = style;
        return this;
    }

    /// <summary>
    ///     设置此按钮的动作类型。
    /// </summary>
    /// <param name="action"> 此按钮的动作类型。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithAction(ButtonAction action)
    {
        Action = action;
        return this;
    }

    /// <summary>
    ///     设置此按钮的权限。
    /// </summary>
    /// <param name="permission"> 此按钮的权限。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithPermission(ButtonPermission permission)
    {
        Permission = permission;
        return this;
    }

    /// <summary>
    ///     设置拥有操作此按钮的权限的所有用户的 ID。
    /// </summary>
    /// <param name="allowedUserIds"> 拥有操作此按钮的权限的所有用户的 ID。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithAllowedUserIds(params string[] allowedUserIds)
    {
        AllowedUserIds = [..allowedUserIds];
        return this;
    }

    /// <summary>
    ///     添加拥有操作此按钮的权限的用户的 ID。
    /// </summary>
    /// <param name="userIds"> 用户的 ID。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder AddAllowedUserId(params string[] userIds)
    {
        AllowedUserIds ??= [];
        AllowedUserIds.AddRange(userIds);
        return this;
    }

    /// <summary>
    ///     设置拥有操作此按钮的权限的所有角色的 ID。
    /// </summary>
    /// <param name="allowedRoleIds"> 拥有操作此按钮的权限的所有角色的 ID。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithAllowedRoleIds(params uint[] allowedRoleIds)
    {
        AllowedRoleIds = [..allowedRoleIds];
        return this;
    }

    /// <summary>
    ///     添加拥有操作此按钮的权限的角色的 ID。
    /// </summary>
    /// <param name="roleIds"> 角色的 ID。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder AddAllowedRoleId(params uint[] roleIds)
    {
        AllowedRoleIds ??= [];
        AllowedRoleIds.AddRange(roleIds);
        return this;
    }

    /// <summary>
    ///     设置此按钮的数据。
    /// </summary>
    /// <param name="data"> 此按钮的数据。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithData(string data)
    {
        Data = data;
        return this;
    }

    /// <summary>
    ///     设置此指令钮是否带引用回复。
    /// </summary>
    /// <param name="isCommandReply"> 此指令钮是否带引用回复。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithIsCommandReply(bool isCommandReply)
    {
        IsCommandReply = isCommandReply;
        return this;
    }

    /// <summary>
    ///     设置此指令钮是否自动发送。
    /// </summary>
    /// <param name="isCommandAutoSend"> 此指令钮是否自动发送。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithIsCommandAutoSend(bool isCommandAutoSend)
    {
        IsCommandAutoSend = isCommandAutoSend;
        return this;
    }

    /// <summary>
    ///     设置此指定按钮的特殊操作。
    /// </summary>
    /// <param name="actionAnchor"> 此指定按钮的特殊操作。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithActionAnchor(ButtonActionAnchor actionAnchor)
    {
        ActionAnchor = actionAnchor;
        return this;
    }

    /// <summary>
    ///     设置客户端不支持此按钮时弹出的提示信息。
    /// </summary>
    /// <param name="unsupportedVersionTip"> 客户端不支持此按钮时弹出的提示信息。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonBuilder WithUnsupportedVersionTip(string unsupportedVersionTip)
    {
        UnsupportedVersionTip = unsupportedVersionTip;
        return this;
    }

    /// <summary>
    ///     将当前构建器构建为一个 <see cref="KeyboardButton"/> 实例。
    /// </summary>
    /// <returns> 构建的 <see cref="KeyboardButton"/> 实例。 </returns>
    /// <exception cref="ArgumentNullException"> <see cref="Label"/> 或 <see cref="Id"/> 必须被设置。 </exception>
    public KeyboardButton Build()
    {
        string? label = Label ?? Id;
        if (label is null)
            throw new ArgumentNullException(nameof(Label), "Label or ID must be set.");

        return new KeyboardButton(
            Id, label, LabelVisited ?? label, Style ?? ButtonStyle.Blue, Action ?? InferButtonAction(),
            Permission ?? ButtonPermission.Everyone, AllowedUserIds, AllowedRoleIds,
            Data ?? Id ?? label, IsCommandReply, IsCommandAutoSend, ActionAnchor, UnsupportedVersionTip ?? label
        );
    }

    private ButtonAction InferButtonAction()
    {
        if (IsCommandReply.HasValue || IsCommandAutoSend.HasValue || ActionAnchor.HasValue)
            return ButtonAction.Command;
        if (Data is not null && Uri.TryCreate(Data, UriKind.Absolute, out _))
            return ButtonAction.Jump;
        return ButtonAction.Callback;
    }

    private string DebuggerDisplay => $"{Label} ({Action}{(Id is null ? "" : $", {Id}")})";
}
