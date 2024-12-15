namespace QQBot;

/// <summary>
///     表示一个文本元素 <see cref="QQBot.TextElement"/> 的样式。
/// </summary>
[Flags]
public enum TextStyle
{
    /// <summary>
    ///     无样式。
    /// </summary>
    None = 0,

    /// <summary>
    ///     粗体。
    /// </summary>
    Bold = 1 << 0,

    /// <summary>
    ///     斜体。
    /// </summary>
    Italic = 1 << 1,

    /// <summary>
    ///     下划线。
    /// </summary>
    Underline = 1 << 2,
}