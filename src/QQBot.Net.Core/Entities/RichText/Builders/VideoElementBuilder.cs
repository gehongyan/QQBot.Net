namespace QQBot;

/// <summary>
///     表示一个视频元素的构建器。
/// </summary>
public class VideoElementBuilder : IElementBuilder
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Video;

    /// <summary>
    ///     获取或设置视频的 URL。
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    ///     初始化 <see cref="VideoElementBuilder"/> 类的新实例。
    /// </summary>
    public VideoElementBuilder()
    {
    }

    /// <summary>
    ///     初始化 <see cref="VideoElementBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="url"> 视频的 URL。 </param>
    public VideoElementBuilder(string url)
    {
        Url = url;
    }

    /// <summary>
    ///     设置视频的 URL。
    /// </summary>
    /// <param name="url"> 视频的 URL。 </param>
    /// <returns> 返回当前 <see cref="VideoElementBuilder"/> 实例。 </returns>
    public VideoElementBuilder WithUrl(string url)
    {
        Url = url;
        return this;
    }
}
