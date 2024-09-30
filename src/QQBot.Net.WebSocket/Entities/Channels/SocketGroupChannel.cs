using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的群组频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGroupChannel : SocketChannel, IGroupChannel, ISocketMessageChannel
{
    /// <inheritdoc cref="QQBot.IGroupChannel.Id" />
    public new Guid Id { get; }

    /// <inheritdoc />
    internal SocketGroupChannel(QQBotSocketClient client, Guid id)
        : base(client, id.ToString("N").ToUpperInvariant())
    {
        Id = id;
    }

    internal static SocketGroupChannel Create(QQBotSocketClient client, ClientState state, Guid id)
    {
        SocketGroupChannel channel = new(client, id);
        return channel;
    }

    private string DebuggerDisplay => $"Unknown ({Id}, Group)";
}
