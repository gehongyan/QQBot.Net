namespace QQBot;

/// <summary>
///     表示一个用于创建 <see cref="KeyboardTemplate"/> 实例的构建器。
/// </summary>
public class KeyboardTemplateBuilder : IKeyboardBuilder, IEquatable<KeyboardTemplateBuilder>
{
    /// <summary>
    ///     获取或设置模板的 ID。
    /// </summary>
    public string TemplateId { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardTemplateBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="templateId"> 模板的 ID。 </param>
    public KeyboardTemplateBuilder(string templateId)
    {
        TemplateId = templateId;
    }

    /// <inheritdoc cref="IKeyboardBuilder.Build" />
    public KeyboardTemplate Build() => new(TemplateId);

    /// <inheritdoc />
    IKeyboard IKeyboardBuilder.Build() => Build();

    /// <inheritdoc />
    public bool Equals(KeyboardTemplateBuilder? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return TemplateId == other.TemplateId;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is KeyboardTemplateBuilder content && Equals(content);

    /// <summary>
    ///     确定两个 <see cref="KeyboardTemplateBuilder"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(KeyboardTemplateBuilder? left, KeyboardTemplateBuilder? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="KeyboardTemplateBuilder"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(KeyboardTemplateBuilder? left, KeyboardTemplateBuilder? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}