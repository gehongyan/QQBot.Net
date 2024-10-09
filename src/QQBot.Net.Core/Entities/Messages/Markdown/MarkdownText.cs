using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个 Markdown 文本内容。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MarkdownText : IMarkdown, IEquatable<MarkdownText>
{
    /// <summary>
    ///     获取 Markdown 文本内容。
    /// </summary>
    public string Text { get; }

    /// <summary>
    ///     初始化一个 <see cref="MarkdownText"/> 类的新实例。
    /// </summary>
    /// <param name="text"> Markdown 文本内容。 </param>
    internal MarkdownText(string text)
    {
        Text = text;
    }

    private string DebuggerDisplay => $"Text: {Text}";

    /// <inheritdoc />
    public override string ToString() => Text;

    /// <inheritdoc />
    public bool Equals(MarkdownText? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Text == other.Text;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MarkdownText content && Equals(content);

    /// <summary>
    ///     确定两个 <see cref="MarkdownText"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(MarkdownText? left, MarkdownText? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="MarkdownText"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(MarkdownText? left, MarkdownText? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}
