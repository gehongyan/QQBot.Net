using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个嵌入式消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Embed : IEmbed, IEquatable<Embed>
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

    /// <inheritdoc />
    public bool Equals(Embed? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Title == other.Title
            && Prompt == other.Prompt
            && Nullable.Equals(Thumbnail, other.Thumbnail)
            && Fields.Equals(other.Fields);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Embed other && Equals(other);

    /// <summary>
    ///     确定两个 <see cref="Embed"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(Embed? left, Embed? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="Embed"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(Embed? left, Embed? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(Title);
        hash.Add(Prompt);
        hash.Add(Thumbnail);
        foreach (EmbedField field in Fields)
            hash.Add(field);
        return hash.ToHashCode();
    }
}
