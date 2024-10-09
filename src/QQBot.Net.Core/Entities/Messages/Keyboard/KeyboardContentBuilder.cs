namespace QQBot;

/// <summary>
///     表示一个自定义键盘按钮的构建器。
/// </summary>
public class KeyboardContentBuilder : IKeyboardBuilder
{
    /// <summary>
    ///     The max amount of rows a message can have.
    /// </summary>
    public const int MaxActionRowCount = 5;

    /// <summary>
    ///     获取或设置此键盘的按钮行。
    /// </summary>
    public List<KeyboardButtonRowBuilder> Rows { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardButtonRowBuilder"/> 类的新实例。
    /// </summary>
    public KeyboardContentBuilder()
    {
        Rows = [];
    }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardButtonRowBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="rows"> 此键盘的按钮行。 </param>
    public KeyboardContentBuilder(IEnumerable<KeyboardButtonRowBuilder> rows)
    {
        Rows = [..rows];
    }

    /// <summary>
    ///     添加此键盘的按钮行。
    /// </summary>
    /// <param name="row"> 要添加的按钮行。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardContentBuilder AddRow(KeyboardButtonRowBuilder row)
    {
        if (Rows.Count >= MaxActionRowCount)
            throw new InvalidOperationException($"Rows count reached {MaxActionRowCount}");
        Rows.Add(row);
        return this;
    }

    /// <summary>
    ///     添加此键盘的按钮行。
    /// </summary>
    /// <param name="action"> 要添加的按钮行。 </param>
    /// <returns> 当前构建器。 </returns>
    public KeyboardContentBuilder AddRow(Action<KeyboardButtonRowBuilder> action)
    {
        KeyboardButtonRowBuilder row = new();
        action(row);
        return AddRow(row);
    }

    /// <summary>
    ///     在指定行添加一个按钮。
    /// </summary>
    /// <param name="button"> 要添加的按钮。 </param>
    /// <param name="row"> 要添加到的行的索引。 </param>
    /// <returns> 当前构建器。 </returns>
    /// <remarks>
    ///     当指定的行内的按钮数量达到 <see cref="KeyboardButtonRowBuilder.MaxChildCount"/> 时，将会尝试添加到下一行。
    /// </remarks>
    /// <exception cref="InvalidOperationException"> 找不到可添加按钮的行时引发。 </exception>
    public KeyboardContentBuilder AddButton(KeyboardButtonBuilder button, int row = 0)
    {
        if (Rows.Count == row)
            Rows.Add(new KeyboardButtonRowBuilder().AddButton(button));
        else
        {
            KeyboardButtonRowBuilder targetRow;
            if (row < Rows.Count)
                targetRow = Rows[row];
            else
            {
                targetRow = new KeyboardButtonRowBuilder();
                Rows.Add(targetRow);
            }

            if (targetRow.CanTakeComponent())
                targetRow.AddButton(button);
            else if (row < MaxActionRowCount)
                AddButton(button, row + 1);
            else
                throw new InvalidOperationException($"There is no more row to add a {nameof(button)}");
        }

        return this;
    }

    /// <summary>
    ///     添加一个按钮到指定行。
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
    /// <param name="row"> 要添加到的行的索引。 </param>
    /// <returns> 当前构建器。 </returns>
    /// <remarks>
    ///     当指定的行内的按钮数量达到 <see cref="KeyboardButtonRowBuilder.MaxChildCount"/> 时，将会尝试添加到下一行。
    /// </remarks>
    /// <exception cref="InvalidOperationException"> 找不到可添加按钮的行时引发。 </exception>
    public KeyboardContentBuilder AddButton(string? id = null, string? label = null, string? labelVisited = null,
        ButtonStyle? style = null, ButtonAction? action = null, ButtonPermission? permission = null,
        IEnumerable<string>? allowedUserIds = null, IEnumerable<uint>? allowedRoleIds = null, string? data = null,
        bool? isCommandReply = null, bool? isCommandAutoSend = null, ButtonActionAnchor? actionAnchor = null,
        string? unsupportedVersionTip = null, int row = 0)
    {
        KeyboardButtonBuilder buttonBuilder = new(
            id, label, labelVisited, style, action, permission, allowedUserIds, allowedRoleIds,
            data, isCommandReply, isCommandAutoSend, actionAnchor, unsupportedVersionTip);
        return AddButton(buttonBuilder, row);
    }

    /// <summary>
    ///     在指定行添加一个按钮。
    /// </summary>
    /// <param name="action"> 一个委托，用于配置按钮构建器。 </param>
    /// <param name="row"> 要添加到的行的索引。 </param>
    /// <returns> 当前构建器。 </returns>
    /// <remarks>
    ///     当指定的行内的按钮数量达到 <see cref="KeyboardButtonRowBuilder.MaxChildCount"/> 时，将会尝试添加到下一行。
    /// </remarks>
    /// <exception cref="InvalidOperationException"> 找不到可添加按钮的行时引发。 </exception>
    public KeyboardContentBuilder AddButton(Action<KeyboardButtonBuilder> action, int row = 0)
    {
        KeyboardButtonBuilder button = new();
        action(button);
        return AddButton(button, row);
    }

    /// <summary>
    ///     将当前构建器构建为一个 <see cref="KeyboardContent"/> 实例。
    /// </summary>
    /// <returns> 构建的 <see cref="KeyboardContent"/> 实例。 </returns>
    public KeyboardContent Build()
    {
        if (Rows.Count > MaxActionRowCount)
            throw new InvalidOperationException($"Rows count reached {MaxActionRowCount}");
        return new KeyboardContent(Rows.Select(x => x.Build()));
    }

    /// <inheritdoc />
    IKeyboard IKeyboardBuilder.Build() => Build();
}
