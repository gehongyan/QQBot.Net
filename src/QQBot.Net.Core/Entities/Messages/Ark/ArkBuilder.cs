namespace QQBot;

/// <summary>
///     表示一个模板构建器。
/// </summary>
public class ArkBuilder : IEquatable<ArkBuilder>
{
    /// <summary>
    ///     获取模板的 ID。
    /// </summary>
    public int TemplateId { get; }

    /// <summary>
    ///     获取或设置参数。
    /// </summary>
    public Dictionary<string, IArkParameterBuilder> Parameters { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="ArkBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="templateId"> 模板的 ID。 </param>
    /// <param name="parameters"> 参数构建器。 </param>
    public ArkBuilder(int templateId, Dictionary<string, IArkParameterBuilder>? parameters = null)
    {
        TemplateId = templateId;
        Parameters = parameters ?? [];
    }

    /// <summary>
    ///     设置一个参数。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="value"> 单值参数的值。 </param>
    public void AddParameter(string key, string value)
    {
        AddParameter(key, new ArkSingleParameterBuilder().WithValue(value));
    }

    /// <summary>
    ///     设置一个参数。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="values"> 多字典参数的值。 </param>
    public void AddParameter(string key, params IReadOnlyDictionary<string, string>[] values)
    {
        AddParameter(key, new ArkMultiDictionaryParameterBuilder([..values]));
    }

    /// <summary>
    ///     设置一个参数。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="values"> 多字典参数的值。 </param>
    public void AddParameter(string key, IEnumerable<IReadOnlyDictionary<string, string>> values)
    {
        AddParameter(key, new ArkMultiDictionaryParameterBuilder([..values]));
    }

    /// <summary>
    ///     设置一个参数。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="value"> 参数值构建器。 </param>
    public void AddParameter(string key, IArkParameterBuilder value)
    {
        Parameters[key] = value;
    }

    /// <summary>
    ///     设置一个参数。
    /// </summary>
    /// <param name="key"> 参数的键。 </param>
    /// <param name="action"> 一个包含对要添加的新创建的参数进行配置的操作的委托。 </param>
    /// <typeparam name="T"> 参数值构建器的类型。 </typeparam>
    public void AddParameter<T>(string key, Action<T> action) where T : IArkParameterBuilder, new()
    {
        T builder = new();
        action(builder);
        AddParameter(key, builder);
    }

    /// <summary>
    ///     将此构建器构建为 <see cref="QQBot.Ark"/> 实例。
    /// </summary>
    /// <returns> 构建的模板实例。 </returns>
    public Ark Build()
    {
        return new Ark(TemplateId, Parameters.ToDictionary(x => x.Key, x => x.Value.Build()));
    }

    /// <inheritdoc />
    public bool Equals(ArkBuilder? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return TemplateId == other.TemplateId
            && Parameters.Equals(other.Parameters);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ArkBuilder builder && Equals(builder);

    /// <summary>
    ///     确定两个 <see cref="ArkBuilder"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(ArkBuilder? left, ArkBuilder? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="ArkBuilder"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(ArkBuilder? left, ArkBuilder? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}
