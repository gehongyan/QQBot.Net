namespace QQBot;

/// <summary>
///     表示一个自定义键盘内的按钮行。
/// </summary>
public class KeyboardButtonRow
{
    /// <summary>
    ///     获取此按钮行内包含的按钮。
    /// </summary>
    public IReadOnlyCollection<KeyboardButton> Buttons { get; }

    internal KeyboardButtonRow()
    {
        Buttons = [];
    }

    internal KeyboardButtonRow(IEnumerable<KeyboardButton> buttons)
    {
        Buttons = [..buttons];
    }
}
