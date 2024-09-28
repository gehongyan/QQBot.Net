namespace QQBot;

/// <summary>
///     表示一个子频道。
/// </summary>
public interface IChannel : IEntity<ulong>
{
    /// <summary>
    ///     获取此子频道的名称。
    /// </summary>
    string Name { get; }
}
