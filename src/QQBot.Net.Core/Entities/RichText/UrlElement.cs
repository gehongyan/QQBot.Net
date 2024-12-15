using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个 URL 元素。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class UrlElement : IElement
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Url;

    /// <summary>
    ///     获取此 URL 元素的 URL。
    /// </summary>
    public string Url { get; }

    /// <summary>
    ///     获取此 URL 元素的描述。
    /// </summary>
    public string Description { get; }

    internal UrlElement(string url, string description)
    {
        Url = url;
        Description = description;
    }

    /// <inheritdoc cref="QQBot.UrlElement.Description" />
    public override string ToString() => Description;

    private string DebuggerDisplay => $"{Url} ({Description})";
}