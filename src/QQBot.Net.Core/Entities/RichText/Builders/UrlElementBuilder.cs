namespace QQBot;

/// <summary>
///     表示一个 URL 元素的构建器。
/// </summary>
public class UrlElementBuilder : IElementBuilder
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Url;

    /// <summary>
    ///     获取或设置 URL 元素的 URL。
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    ///     获取或设置 URL 元素的描述。
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     初始化 <see cref="UrlElementBuilder"/> 类的新实例。
    /// </summary>
    public UrlElementBuilder()
    {
    }

    /// <summary>
    ///     初始化 <see cref="UrlElementBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="url">  URL 的 URL。 </param>
    /// <param name="description">  URL 的长宽比例。 </param>
    public UrlElementBuilder(string url, string description)
    {
        Url = url;
        Description = description;
    }

    /// <summary>
    ///     设置 URL 的 URL。
    /// </summary>
    /// <param name="url">  URL 的 URL。 </param>
    /// <returns> 返回当前 <see cref="UrlElementBuilder"/> 实例。 </returns>
    public UrlElementBuilder WithUrl(string url)
    {
        Url = url;
        return this;
    }

    /// <summary>
    ///     设置 URL 的描述。
    /// </summary>
    /// <param name="description">  URL 的描述。 </param>
    /// <returns> 返回当前 <see cref="UrlElementBuilder"/> 实例。 </returns>
    public UrlElementBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }
}
