using System.Collections.ObjectModel;

namespace QQBot;

/// <summary>
///     表示一个用于创建 <see cref="MarkdownTemplate"/> 实例的构建器。
/// </summary>
public class MarkdownTemplateBuilder : IMarkdownBuilder, IEquatable<MarkdownTemplateBuilder>
{
    /// <summary>
    ///     获取或设置模板的 ID。
    /// </summary>
    public string TemplateId { get; set; }

    /// <summary>
    ///     获取或设置 Markdown 模板内容的参数。
    /// </summary>
    public Dictionary<string, IReadOnlyCollection<string>> Parameters { get; set; } = [];

    /// <summary>
    ///     初始化一个 <see cref="MarkdownTemplateBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="templateId"> 模板的 ID。 </param>
    public MarkdownTemplateBuilder(string templateId)
    {
        TemplateId = templateId;
    }

    /// <summary>
    ///     添加一个参数到 Markdown 模板内容中。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="values"> 参数的值。 </param>
    public void AddParameter(string key, params string[] values)
    {
        Parameters[key] = values;
    }

    /// <summary>
    ///     添加一个参数到 Markdown 模板内容中。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="values"> 参数的值。 </param>
    public void AddParameter(string key, IEnumerable<string> values)
    {
        Parameters[key] = [..values];
    }

    /// <summary>
    ///     添加参数值到 Markdown 模板内容参数列表中具有指定键的参数值列表的末尾。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="values"> 参数的值。 </param>
    public void AppendParameter(string key, params string[] values)
    {
        Parameters[key] = Parameters.TryGetValue(key, out IReadOnlyCollection<string>? existing)
            ? [..existing, ..values]
            : values;
    }

    /// <summary>
    ///     添加参数值到 Markdown 模板内容参数列表中具有指定键的参数值列表的末尾。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="values"> 参数的值。 </param>
    public void AppendParameter(string key, IEnumerable<string> values)
    {
        Parameters[key] = Parameters.TryGetValue(key, out IReadOnlyCollection<string>? existing)
            ? [..existing, ..values]
            : [..values];
    }

    /// <inheritdoc cref="IMarkdownBuilder.Build" />
    public MarkdownTemplate Build() => new(TemplateId, new ReadOnlyDictionary<string, IReadOnlyCollection<string>>(Parameters));

    /// <inheritdoc />
    IMarkdown IMarkdownBuilder.Build() => Build();

    /// <inheritdoc />
    public bool Equals(MarkdownTemplateBuilder? other)
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
    public override bool Equals(object? obj) => obj is MarkdownTemplateBuilder content && Equals(content);

    /// <summary>
    ///     确定两个 <see cref="MarkdownTemplateBuilder"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(MarkdownTemplateBuilder? left, MarkdownTemplateBuilder? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="MarkdownTemplateBuilder"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(MarkdownTemplateBuilder? left, MarkdownTemplateBuilder? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}
