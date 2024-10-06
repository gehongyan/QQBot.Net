using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个嵌入式消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public record Embed : IEmbed
{
    /// <inheritdoc />
    public string? Title { get; }

    /// <inheritdoc />
    public string? Prompt { get; }

    /// <inheritdoc />
    public EmbedThumbnail? Thumbnail { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<EmbedField> Fields { get; }

    internal Embed(string? title, string? prompt,
        EmbedThumbnail? thumbnail, IReadOnlyCollection<EmbedField> fields)
    {
        Title = title;
        Prompt = prompt;
        Thumbnail = thumbnail;
        Fields = fields;
    }

    private string DebuggerDisplay => Title ?? Prompt ?? "No Content";
}
