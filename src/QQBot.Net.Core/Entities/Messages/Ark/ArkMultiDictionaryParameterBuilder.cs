namespace QQBot;

/// <summary>
///     用于构建 <see cref="QQBot.ArkMultiDictionaryParameter"/> 的构建器。
/// </summary>
public class ArkMultiDictionaryParameterBuilder : IArkParameterBuilder, IEquatable<ArkMultiDictionaryParameterBuilder>
{
    /// <summary>
    ///     获取或设置参数值。
    /// </summary>
    public List<IReadOnlyDictionary<string, string>> Values { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="ArkMultiDictionaryParameterBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="values"></param>
    public ArkMultiDictionaryParameterBuilder(IEnumerable<IReadOnlyDictionary<string, string>>? values = null)
    {
        Values = values?.ToList() ?? [];
    }

    /// <summary>
    ///     设置参数值。
    /// </summary>
    /// <param name="value"> 要设置的参数值。 </param>
    /// <returns> 当前构建器。 </returns>
    public ArkMultiDictionaryParameterBuilder AddValue(IReadOnlyDictionary<string, string> value)
    {
        Values.Add(value);
        return this;
    }

    /// <summary>
    ///     构建 <see cref="ArkSingleParameter"/> 实例。
    /// </summary>
    /// <returns> 构建的 <see cref="ArkSingleParameter"/> 实例。 </returns>
    public ArkMultiDictionaryParameter Build()
    {
        if (Values.Count > 0)
        {
            HashSet<string> referenceKeys = [..Values[0].Keys];
            if (referenceKeys.Any(string.IsNullOrEmpty))
                throw new InvalidOperationException("Key cannot be empty.");
            if (Values.Skip(1).Any(dict => !referenceKeys.SetEquals(dict.Keys)))
                throw new InvalidOperationException("Keys must be the same.");
        }
        return new ArkMultiDictionaryParameter([..Values]);
    }

    /// <inheritdoc />
    IArkParameter IArkParameterBuilder.Build() => Build();

    /// <inheritdoc />
    public bool Equals(ArkMultiDictionaryParameterBuilder? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Values.Equals(other.Values);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ArkMultiDictionaryParameterBuilder other && Equals(other);

    /// <summary>
    ///     比较两个 <see cref="ArkMultiDictionaryParameterBuilder"/> 是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个 <see cref="ArkMultiDictionaryParameterBuilder"/>。 </param>
    /// <param name="right"> 要比较的第二个 <see cref="ArkMultiDictionaryParameterBuilder"/>。 </param>
    /// <returns> 如果两个 <see cref="ArkMultiDictionaryParameterBuilder"/> 相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public static bool operator ==(ArkMultiDictionaryParameterBuilder? left, ArkMultiDictionaryParameterBuilder? right)
        => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     比较两个 <see cref="ArkMultiDictionaryParameterBuilder"/> 是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个 <see cref="ArkMultiDictionaryParameterBuilder"/>。 </param>
    /// <param name="right"> 要比较的第二个 <see cref="ArkMultiDictionaryParameterBuilder"/>。 </param>
    /// <returns> 如果两个 <see cref="ArkMultiDictionaryParameterBuilder"/> 不相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public static bool operator !=(ArkMultiDictionaryParameterBuilder? left, ArkMultiDictionaryParameterBuilder? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}
