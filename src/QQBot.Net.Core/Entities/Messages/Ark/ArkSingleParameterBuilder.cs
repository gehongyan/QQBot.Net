namespace QQBot;

/// <summary>
///     用于构建 <see cref="ArkSingleParameter"/> 的构建器。
/// </summary>
public class ArkSingleParameterBuilder : IArkParameterBuilder, IEquatable<ArkSingleParameterBuilder>
{
    /// <summary>
    ///     获取或设置参数值。
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    ///     设置参数值。
    /// </summary>
    /// <param name="value"> 要设置的参数值。 </param>
    /// <returns> 当前构建器。 </returns>
    public ArkSingleParameterBuilder WithValue(string value)
    {
        Value = value;
        return this;
    }

    /// <summary>
    ///     构建 <see cref="ArkSingleParameter"/> 实例。
    /// </summary>
    /// <returns> 构建的 <see cref="ArkSingleParameter"/> 实例。 </returns>
    public ArkSingleParameter Build()
    {
        if (Value is null)
            throw new InvalidOperationException("Value must be set.");
        return new ArkSingleParameter(Value);
    }

    /// <inheritdoc />
    IArkParameter IArkParameterBuilder.Build() => Build();

    /// <inheritdoc />
    public bool Equals(ArkSingleParameterBuilder? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ArkSingleParameterBuilder other && Equals(other);

    /// <summary>
    ///     比较两个 <see cref="ArkSingleParameterBuilder"/> 是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个 <see cref="ArkSingleParameterBuilder"/>。 </param>
    /// <param name="right"> 要比较的第二个 <see cref="ArkSingleParameterBuilder"/>。 </param>
    /// <returns> 如果两个 <see cref="ArkSingleParameterBuilder"/> 相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public static bool operator ==(ArkSingleParameterBuilder? left, ArkSingleParameterBuilder? right)
        => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     比较两个 <see cref="ArkSingleParameterBuilder"/> 是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个 <see cref="ArkSingleParameterBuilder"/>。 </param>
    /// <param name="right"> 要比较的第二个 <see cref="ArkSingleParameterBuilder"/>。 </param>
    /// <returns> 如果两个 <see cref="ArkSingleParameterBuilder"/> 不相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public static bool operator !=(ArkSingleParameterBuilder? left, ArkSingleParameterBuilder? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}