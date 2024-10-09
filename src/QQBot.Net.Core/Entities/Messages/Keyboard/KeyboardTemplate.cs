using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个按钮键盘模板。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class KeyboardTemplate : IKeyboard, IEquatable<KeyboardTemplate>
{
    /// <summary>
    ///     获取模板的 ID。
    /// </summary>
    public string TemplateId { get; }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardTemplate"/> 类的新实例。
    /// </summary>
    /// <param name="templateId"> Keyboard 模板 ID。 </param>
    internal KeyboardTemplate(string templateId)
    {
        TemplateId = templateId;
    }

    private string DebuggerDisplay => $"TemplateId: {TemplateId}";

    /// <inheritdoc />
    public bool Equals(KeyboardTemplate? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return TemplateId == other.TemplateId;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is KeyboardTemplate content && Equals(content);

    /// <summary>
    ///     确定两个 <see cref="KeyboardTemplate"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(KeyboardTemplate? left, KeyboardTemplate? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="KeyboardTemplate"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(KeyboardTemplate? left, KeyboardTemplate? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}
