using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个富文本段落。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Paragraph
{
    /// <summary>
    ///     获取此段落的元素。
    /// </summary>
    public IReadOnlyCollection<IElement> Elements { get; }

    /// <summary>
    ///     获取此段落的对齐方式。
    /// </summary>
    public TextAlignment? Alignment { get; }

    internal Paragraph(IReadOnlyCollection<IElement> elements, TextAlignment? alignment)
    {
        Elements = elements;
        Alignment = alignment;
    }

    private string DebuggerDisplay =>
        $"{Elements.Count} element{(Elements.Count == 1 ? string.Empty : "s")}{(Alignment.HasValue ? $" ({Alignment.Value} alignment)" : string.Empty)}";
}
