using System.Collections.ObjectModel;

namespace QQBot;

/// <summary>
///     表示一个用于创建 <see cref="QQBot.MarkdownTemplateContent"/> 实例的构建器。
/// </summary>
public class MarkdownTemplateContentBuilder : IMarkdownContentBuilder, IEquatable<MarkdownTextContentBuilder>
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
    ///     初始化一个 <see cref="MarkdownTemplateContentBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="templateId"> 模板的 ID。 </param>
    public MarkdownTemplateContentBuilder(string templateId)
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

    /// <inheritdoc cref="QQBot.IMarkdownContentBuilder.Build" />
    public MarkdownTemplateContent Build() => new(TemplateId, new ReadOnlyDictionary<string, IReadOnlyCollection<string>>(Parameters));

    /// <inheritdoc />
    IMarkdownContent IMarkdownContentBuilder.Build() => Build();

    /// <inheritdoc />
    public bool Equals(MarkdownTextContentBuilder? other) => throw new NotImplementedException();
}