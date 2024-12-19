namespace QQBot;

/// <summary>
///     表示一个图片元素的构建器。
/// </summary>
public class ImageElementBuilder : IElementBuilder
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Image;

    /// <summary>
    ///     获取或设置图片的 URL。
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    ///     获取或设置图片的长宽比例。
    /// </summary>
    public double? Ratio { get; set; }

    /// <summary>
    ///     初始化 <see cref="ImageElementBuilder"/> 类的新实例。
    /// </summary>
    public ImageElementBuilder()
    {
    }

    /// <summary>
    ///     初始化 <see cref="ImageElementBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <param name="ratio"> 图片的长宽比例。 </param>
    public ImageElementBuilder(string url, double ratio)
    {
        Url = url;
        Ratio = ratio;
    }

    /// <summary>
    ///     设置图片的 URL。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <returns> 返回当前 <see cref="ImageElementBuilder"/> 实例。 </returns>
    public ImageElementBuilder WithUrl(string url)
    {
        Url = url;
        return this;
    }

    /// <summary>
    ///     设置图片的长宽比例。
    /// </summary>
    /// <param name="ratio"> 图片的长宽比例。 </param>
    /// <returns> 返回当前 <see cref="ImageElementBuilder"/> 实例。 </returns>
    public ImageElementBuilder WithRatio(double ratio)
    {
        Ratio = ratio;
        return this;
    }
}
