using System.Diagnostics;
using System.Drawing;

namespace QQBot;

/// <summary>
///     表示一个视频元素。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class VideoElement : IElement
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Video;

    /// <summary>
    ///     获取此视频元素的 ID。
    /// </summary>
    public string Id { get; }

    /// <summary>
    ///     获取此视频元素的 URL。
    /// </summary>
    public string Url { get; }

    /// <summary>
    ///     获取此视频元素的尺寸。
    /// </summary>
    public Size Size { get; }

    /// <summary>
    ///     获取此视频元素的时长。
    /// </summary>
    public TimeSpan? Duration { get; }

    /// <summary>
    ///     获取此视频元素的封面的 URL。
    /// </summary>
    public string CoverUrl { get; }

    /// <summary>
    ///     获取此视频元素的封面的尺寸。
    /// </summary>
    public Size CoverSize { get; }

    internal VideoElement(string id, string url, Size size, TimeSpan? duration, string coverUrl, Size coverSize)
    {
        Id = id;
        Url = url;
        Size = size;
        Duration = duration;
        CoverUrl = coverUrl;
        CoverSize = coverSize;
    }

    /// <inheritdoc cref="QQBot.ImageElement.Url" />
    public override string ToString() => Url;

    private string DebuggerDisplay => $"{Url} ({Id}, {Duration}, {Size.Width}×{Size.Height})";
}
