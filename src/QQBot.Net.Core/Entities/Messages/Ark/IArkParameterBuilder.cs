namespace QQBot;

/// <summary>
///     表示一个通用的用于构建 <see cref="IArkParameter"/> 实例的构建器。
/// </summary>
public interface IArkParameterBuilder
{
    /// <summary>
    ///     构建 <see cref="IArkParameter"/> 实例。
    /// </summary>
    /// <returns> 构建的 <see cref="IArkParameter"/> 实例。 </returns>
    IArkParameter Build();
}