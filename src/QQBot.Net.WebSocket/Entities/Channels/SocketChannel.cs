using System.Diagnostics;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class SocketChannel : SocketEntity<string>, IChannel
{
    internal SocketChannel(QQBotSocketClient client, string id)
        : base(client, id)
    {
    }

    private string DebuggerDisplay => $"Unknown ({Id}, Channel)";
}
