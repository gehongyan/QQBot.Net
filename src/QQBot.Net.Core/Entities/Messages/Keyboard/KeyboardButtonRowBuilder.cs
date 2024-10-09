using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个自定义键盘内的按钮行构建器。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class KeyboardButtonRowBuilder
{
    /// <summary>
    ///     获取此行内的按钮的最大数量。
    /// </summary>
    public const int MaxChildCount = 5;

    /// <summary>
    ///     获取或设置此行内的按钮。
    /// </summary>
    public List<KeyboardButtonBuilder> Buttons { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardButtonRowBuilder"/> 类的新实例。
    /// </summary>
    public KeyboardButtonRowBuilder()
    {
        Buttons = [];
    }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardButtonRowBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="buttons"> 此行内的按钮。 </param>
    public KeyboardButtonRowBuilder(IEnumerable<KeyboardButtonBuilder> buttons)
    {
        Buttons = [..buttons];
    }

    /// <summary>
    ///     设置此行内的按钮。
    /// </summary>
    /// <param name="buttons"> 要设置的按钮。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonRowBuilder WithButton(List<KeyboardButtonBuilder> buttons)
    {
        Buttons = buttons;
        return this;
    }

    /// <summary>
    ///     在当前行的末尾添加一个按钮。
    /// </summary>
    /// <param name="button"> 要添加的按钮。 </param>
    /// <returns> 当前构建器。 </returns>
    /// <exception cref="InvalidOperationException"> 按钮数量达到了 <see cref="MaxChildCount"/> 时引发。 </exception>
    public KeyboardButtonRowBuilder AddButton(KeyboardButtonBuilder button)
    {
        if (Buttons.Count >= MaxChildCount)
            throw new InvalidOperationException($"Buttons count reached {MaxChildCount}");
        Buttons.Add(button);
        return this;
    }

    /// <summary>
    ///     在当前行的末尾添加一个按钮。
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
    /// <returns> 当前构建器。 </returns>
    /// <exception cref="InvalidOperationException"> 按钮数量达到了 <see cref="MaxChildCount"/> 时引发。 </exception>
    public KeyboardButtonRowBuilder AddButton(string? id = null, string? label = null, string? labelVisited = null,
        ButtonStyle? style = null, ButtonAction? action = null, ButtonPermission? permission = null,
        IEnumerable<string>? allowedUserIds = null, IEnumerable<uint>? allowedRoleIds = null, string? data = null,
        bool? isCommandReply = null, bool? isCommandAutoSend = null, ButtonActionAnchor? actionAnchor = null,
        string? unsupportedVersionTip = null)
    {
        KeyboardButtonBuilder buttonBuilder = new(
            id, label, labelVisited, style, action, permission, allowedUserIds, allowedRoleIds,
            data, isCommandReply, isCommandAutoSend, actionAnchor, unsupportedVersionTip);
        return AddButton(buttonBuilder);
    }

    /// <summary>
    ///     在当前行的末尾添加一个按钮。
    /// </summary>
    /// <param name="action"> 一个委托，用于配置按钮构建器。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardButtonRowBuilder AddButton(Action<KeyboardButtonBuilder> action)
    {
        KeyboardButtonBuilder buttonBuilder = new();
        action(buttonBuilder);
        return AddButton(buttonBuilder);
    }

    /// <summary>
    ///     将此构建器构建为 <see cref="QQBot.KeyboardButtonRow"/> 实例。
    /// </summary>
    /// <returns> 构建的按钮行。 </returns>
    public KeyboardButtonRow Build()
    {
        if (Buttons.Count == 0)
            throw new InvalidOperationException("There must be at least 1 button in a row.");
        if (Buttons.Count > MaxChildCount)
            throw new InvalidOperationException($"Button row can only contain {MaxChildCount} child components at most.");
        return new KeyboardButtonRow(Buttons.Select(x => x.Build()));
    }

    internal bool CanTakeComponent() => Buttons.Count < MaxChildCount;

    private string DebuggerDisplay => Buttons.Count switch
    {
        0 => "No Buttons",
        1 => "1 Button",
        _ => $"{Buttons.Count} Buttons"
    };
}
