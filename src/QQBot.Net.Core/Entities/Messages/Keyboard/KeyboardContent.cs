namespace QQBot;

/// <summary>
///     表示一个自定义按钮键盘。
/// </summary>
public class KeyboardContent : IKeyboard
{
    /// <summary>
    ///     获取自定义键盘的按钮行。
    /// </summary>
    public IReadOnlyCollection<KeyboardButtonRow> Rows { get; }

    internal KeyboardContent(IEnumerable<KeyboardButtonRow> rows)
    {
        Rows = [..rows];
    }

    internal static KeyboardContent Empty => new([]);
}
