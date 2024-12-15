using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个文本元素。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class TextElement : IElement
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Text;

    /// <summary>
    ///     获取此文本元素的文本内容。
    /// </summary>
    public string Text { get; }

    /// <summary>
    ///     获取此文本元素的样式。
    /// </summary>
    public TextStyle Style { get; }

    /// <summary>
    ///     获取此文本元素是否为粗体样式。
    /// </summary>
    public bool Bold => Style.HasFlag(TextStyle.Bold);

    /// <summary>
    ///     获取此文本元素是否为斜体样式。
    /// </summary>
    public bool Italic => Style.HasFlag(TextStyle.Italic);

    /// <summary>
    ///     获取此文本元素是否为下划线样式。
    /// </summary>
    public bool Underline => Style.HasFlag(TextStyle.Underline);

    internal TextElement(string text, TextStyle style)
    {
        Text = text;
        Style = style;
    }

    /// <inheritdoc cref="QQBot.TextElement.Text" />
    public override string ToString() => Text;

    private string DebuggerDisplay => $"{Text} ({(Style is TextStyle.None ? "No Style" : Style.ToString())})";
}
