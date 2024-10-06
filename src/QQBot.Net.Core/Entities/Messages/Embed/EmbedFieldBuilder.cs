namespace QQBot;

/// <summary>
///     表示一个用于构建 <see cref="QQBot.EmbedField"/> 的构建器类。
/// </summary>
public class EmbedFieldBuilder : IEquatable<EmbedFieldBuilder>
{
    /// <summary>
    ///     获取或设置字段名称。
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     设置字段的名称。
    /// </summary>
    /// <param name="name"> 要设置的名称。 </param>
    /// <returns> 当前构建器。 </returns>
    public EmbedFieldBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    /// <summary>
    ///     构建此字段构建器为 <see cref="QQBot.EmbedField"/> 类。
    /// </summary>
    /// <returns> 构建的字段。 </returns>
    public EmbedField Build() => new(Name);

    /// <summary>
    ///     比较两个 <see cref="EmbedFieldBuilder"/> 是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个 <see cref="EmbedFieldBuilder"/>。 </param>
    /// <param name="right"> 要比较的第二个 <see cref="EmbedFieldBuilder"/>。 </param>
    /// <returns> 如果两个 <see cref="EmbedFieldBuilder"/> 相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public static bool operator ==(EmbedFieldBuilder? left, EmbedFieldBuilder? right)
        => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     比较两个 <see cref="EmbedFieldBuilder"/> 是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个 <see cref="EmbedFieldBuilder"/>。 </param>
    /// <param name="right"> 要比较的第二个 <see cref="EmbedFieldBuilder"/>。 </param>
    /// <returns> 如果两个 <see cref="EmbedFieldBuilder"/> 不相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public static bool operator !=(EmbedFieldBuilder? left, EmbedFieldBuilder? right) => !(left == right);

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is EmbedFieldBuilder embedFieldBuilder && Equals(embedFieldBuilder);

    /// <inheritdoc />
    public bool Equals(EmbedFieldBuilder? embedFieldBuilder) =>
        embedFieldBuilder is not null && Name == embedFieldBuilder.Name;

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}