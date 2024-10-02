namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的私有频道。
/// </summary>
public interface ISocketPrivateChannel : IPrivateChannel
{
    /// <inheritdoc cref="QQBot.IPrivateChannel.Recipients" />
    new IReadOnlyCollection<SocketUser> Recipients { get; }
}
