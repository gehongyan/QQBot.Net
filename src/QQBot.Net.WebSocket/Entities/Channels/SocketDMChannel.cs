namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的子频道用户私聊频道。
/// </summary>
public class SocketDMChannel : SocketChannel, IDMChannel, ISocketMessageChannel
{
    /// <inheritdoc cref="QQBot.IDMChannel.Id" />
    public new ulong Id { get; }

    /// <inheritdoc />
    internal SocketDMChannel(QQBotSocketClient client, ulong id)
        : base(client, id.ToString())
    {
        Id = id;
    }

    internal static SocketDMChannel Create(QQBotSocketClient client, ClientState state, ulong id)
    {
        SocketDMChannel channel = new(client, id);
        return channel;
    }
}
