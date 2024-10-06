using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个模板。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Ark : IArk, IEquatable<Ark>
{
    /// <inheritdoc />
    public int TemplateId { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, IArkParameter> Parameters { get; }

    internal Ark(int templateId, IReadOnlyDictionary<string, IArkParameter> parameters)
    {
        TemplateId = templateId;
        Parameters = parameters;
    }

    private string DebuggerDisplay => $"TemplateId: {TemplateId}, Parameters Count: {Parameters.Count}";

    /// <inheritdoc />
    public bool Equals(Ark? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return TemplateId == other.TemplateId
            && Parameters.Equals(other.Parameters);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Ark ark && Equals(ark);

    /// <summary>
    ///     确定两个 <see cref="Ark"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(Ark? left, Ark? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="Ark"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(Ark? left, Ark? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(TemplateId);
        foreach ((string key, IArkParameter value) in Parameters)
        {
            hash.Add(key);
            hash.Add(value);
        }
        return hash.ToHashCode();
    }
}
