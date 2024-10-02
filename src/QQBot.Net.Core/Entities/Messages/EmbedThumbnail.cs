using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个 <see cref="QQBot.IEmbed"/> 的缩略图。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly record struct EmbedThumbnail
{
    /// <summary>
    ///     获取缩略图的 URL。
    /// </summary>
    public string? Url { get; }

    internal EmbedThumbnail(string? url)
    {
        Url = url;
    }

    private string DebuggerDisplay => Url ?? "No URL";
}
