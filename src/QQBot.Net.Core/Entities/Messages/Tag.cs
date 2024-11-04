using System.Diagnostics;

namespace QQBot;

/// <inheritdoc />
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Tag<TKey, TValue> : ITag
    where TKey : IEquatable<TKey>
    where TValue : IEntity<TKey>
{
    /// <inheritdoc />
    public TagType Type { get; }

    /// <inheritdoc />
    public int Index { get; }

    /// <inheritdoc />
    public int Length { get; }

    /// <inheritdoc cref="QQBot.ITag.Key" />
    public TKey Key { get; }

    /// <inheritdoc cref="QQBot.ITag.Key" />
    public TValue? Value { get; }

    internal Tag(TagType type, int index, int length, TKey key, TValue? value)
    {
        Type = type;
        Index = index;
        Length = length;
        Key = key;
        Value = value;
    }

    private string DebuggerDisplay => ToString();

    /// <inheritdoc />
    public override string ToString() => $"{Value?.ToString() ?? "null"} ({Type})";

    /// <inheritdoc />
    object ITag.Key => Key;

    /// <inheritdoc />
    object? ITag.Value => Value;
}
