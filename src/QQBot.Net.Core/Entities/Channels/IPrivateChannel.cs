namespace QQBot;

/// <summary>
///     表示一个私有子频道。
/// </summary>
public interface IPrivateChannel : IChannel
{
    /// <summary>
    ///     获取可以访问此子频道的所有用户。
    /// </summary>
    IReadOnlyCollection<IUser> Recipients { get; }
}
