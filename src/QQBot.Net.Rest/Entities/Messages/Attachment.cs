using System.Diagnostics;

namespace QQBot.Rest;

/// <inheritdoc cref="IAttachment"/>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Attachment : IAttachment
{
    /// <inheritdoc />
    public AttachmentType Type { get; }

    /// <inheritdoc />
    public string Url { get; }

    /// <inheritdoc />
    public string? Content { get; internal init; }

    /// <inheritdoc />
    public string? Filename { get; internal init; }

    /// <inheritdoc />
    public int? Size { get; internal init; }

    /// <inheritdoc />
    public string? ContentType { get; internal init; }

    /// <inheritdoc />
    public int? Width { get; internal init; }

    /// <inheritdoc />
    public int? Height { get; internal init; }

    internal Attachment(AttachmentType type, string url)
    {
        Type = type;
        Url = url;
    }

    private string DebuggerDisplay => $"{Filename}{(Size.HasValue ? $" ({Size} bytes)" : "")}";
}
