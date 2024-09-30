namespace QQBot;

/// <summary>
///     表示一个群组频道，即 QQ 群。
/// </summary>
public interface IGroupChannel : IMessageChannel, IEntity<Guid>
{
    /// <summary>
    ///     获取此群组频道的唯一标识符。
    /// </summary>
    new Guid Id { get; }
}
