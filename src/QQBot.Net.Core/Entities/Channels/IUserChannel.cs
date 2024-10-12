namespace QQBot;

/// <summary>
///     表示一个用户单聊子频道。
/// </summary>
/// <remarks>
///     这可以包括 QQ 好友发起的聊天，或 QQ 群内用户发起的私聊，不包括子频道内用户发起的私聊。
/// </remarks>
public interface IUserChannel : IMessageChannel, IPrivateChannel, IEntity<Guid>
{
    /// <summary>
    ///     获取此用户单聊子频道的唯一标识符。
    /// </summary>
    new Guid Id { get; }
}
