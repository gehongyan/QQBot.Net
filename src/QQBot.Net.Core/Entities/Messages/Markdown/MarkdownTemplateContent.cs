using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个 Markdown 模板。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MarkdownTemplateContent : IMarkdownContent, IEquatable<MarkdownTemplateContent>
{
    /// <summary>
    ///     获取模板的 ID。
    /// </summary>
    public string TemplateId { get; }

    /// <summary>
    ///     获取 Markdown 模板内容的参数。
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Parameters { get; }

    /// <summary>
    ///     初始化一个 <see cref="MarkdownTemplateContent"/> 类的新实例。
    /// </summary>
    /// <param name="templateId"> Markdown 模板 ID。 </param>
    /// <param name="parameters"> Markdown 模板内容参数。 </param>
    internal MarkdownTemplateContent(string templateId,
        IReadOnlyDictionary<string, IReadOnlyCollection<string>> parameters)
    {
        TemplateId = templateId;
        Parameters = parameters;
    }

    private string DebuggerDisplay => $"TemplateId: {TemplateId}, Parameters: {Parameters.Count}";

    /// <inheritdoc />
    public bool Equals(MarkdownTemplateContent? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (TemplateId != other.TemplateId) return false;
        if (Parameters.Count != other.Parameters.Count) return false;
        foreach ((string key, IReadOnlyCollection<string> value) in Parameters)
        {
            if (!other.Parameters.TryGetValue(key, out IReadOnlyCollection<string>? otherValue)) return false;
            if (value.Count != otherValue.Count) return false;
            if (!value.SequenceEqual(otherValue)) return false;
        }
        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MarkdownTemplateContent content && Equals(content);

    /// <summary>
    ///     确定两个 <see cref="MarkdownTemplateContent"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(MarkdownTemplateContent? left, MarkdownTemplateContent? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="MarkdownTemplateContent"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(MarkdownTemplateContent? left, MarkdownTemplateContent? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}
