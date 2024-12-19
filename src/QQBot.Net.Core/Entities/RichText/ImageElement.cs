using System.Diagnostics;
using System.Drawing;

namespace QQBot;

/// <summary>
///     表示一个图片元素。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ImageElement : IElement
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Image;

    /// <summary>
    ///     获取此图片元素的 ID。
    /// </summary>
    public string Id { get; }

    /// <summary>
    ///     获取此图片元素的 URL。
    /// </summary>
    public string Url { get; }

    /// <summary>
    ///     获取此图片元素的尺寸。
    /// </summary>
    public Size Size { get; }

    /// <summary>
    ///     获取此图片元素的长宽比例。
    /// </summary>
    public double Ratio => (double)Size.Width / Size.Height;

    internal ImageElement(string id, string url, Size size)
    {
        Id = id;
        Url = url;
        Size = size;
    }

    /// <inheritdoc cref="QQBot.ImageElement.Url" />
    public override string ToString() => Url;

    private string DebuggerDisplay => $"{Url} ({Id}, {Size.Width}×{Size.Height})";
}
