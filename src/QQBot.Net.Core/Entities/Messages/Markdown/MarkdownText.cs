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
    ///     获取是否在发送消息前校验 Markdown 图片资源的转存结果。
    /// </summary>
    /// <remarks>
    ///     此选项仅适用于 QQ 单聊和群聊消息。文字子频道和频道私信消息不支持该字段；当其值不为
    ///     <see langword="null"/> 时，QQBot.Net 会忽略该设置并写入一条警告日志。
    ///     <see langword="true"/> 表示图片转存失败时中断消息发送；<see langword="false"/> 表示显式关闭校验；
    ///     <see langword="null"/> 表示不发送该字段并使用 QQ 平台默认行为。
    /// </remarks>
    public bool? ForceVerifyImageResource { get; }

    /// <summary>
    ///     初始化一个 <see cref="MarkdownText"/> 类的新实例。
    /// </summary>
    /// <param name="text"> Markdown 文本内容。 </param>
    /// <param name="forceVerifyImageResource"> 是否校验 Markdown 图片资源的转存结果。 </param>
    internal MarkdownText(string text, bool? forceVerifyImageResource = null)
    {
        Text = text;
        ForceVerifyImageResource = forceVerifyImageResource;
    }

    private string DebuggerDisplay => $"Text: {Text}, ForceVerifyImageResource: {ForceVerifyImageResource}";

    /// <inheritdoc />
    public override string ToString() => Text;

    /// <inheritdoc />
    public bool Equals(MarkdownText? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Text == other.Text && ForceVerifyImageResource == other.ForceVerifyImageResource;
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
    public override int GetHashCode() => HashCode.Combine(Text, ForceVerifyImageResource);
}
