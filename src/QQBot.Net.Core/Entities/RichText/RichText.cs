using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个富文本。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RichText
{
    internal static RichText Empty { get; } = new([]);

    /// <summary>
    ///     获取此富文本的段落。
    /// </summary>
    public IReadOnlyCollection<Paragraph> Paragraphs { get; }

    internal RichText(IReadOnlyCollection<Paragraph> paragraphs)
    {
        Paragraphs = paragraphs;
    }

    internal string DebuggerDisplay => $"{Paragraphs.Count} Paragraph{(Paragraphs.Count == 1 ? string.Empty : "s")}";
}
