using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的群组频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketUserChannel : SocketChannel, IUserChannel, ISocketMessageChannel
{
    /// <inheritdoc cref="QQBot.IUserChannel.Id" />
    public new Guid Id { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> CachedMessages => [];

    /// <inheritdoc />
    internal SocketUserChannel(QQBotSocketClient client, Guid id)
        : base(client, id.ToString("N").ToUpperInvariant())
    {
        Id = id;
    }

    internal static SocketUserChannel Create(QQBotSocketClient client, ClientState state, Guid id)
    {
        SocketUserChannel channel = new(client, id);
        return channel;
    }

    internal void AddMessage(SocketMessage message) { }

    private string DebuggerDisplay => $"Unknown ({Id}, User)";
}
