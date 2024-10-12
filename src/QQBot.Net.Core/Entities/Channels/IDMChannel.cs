namespace QQBot;

/// <summary>
///     表示一个子频道内用户的私聊子频道。
/// </summary>
public interface IDMChannel : IMessageChannel, IPrivateChannel, IEntity<ulong>
{
    /// <summary>
    ///     获取此子频道内用户私聊子频道的唯一标识符。
    /// </summary>
    new ulong Id { get; }

    /// <summary>
    ///     获取参与到此私聊子频道的另外一位用户。
    /// </summary>
    IUser Recipient { get; }
}
