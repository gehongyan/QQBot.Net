using System.Diagnostics;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class SocketChannel : SocketEntity<ulong>, IChannel, IUpdateable
{
    internal SocketChannel(QQBotSocketClient kook, ulong id)
        : base(kook, id)
    {
    }

    internal abstract void Update(ClientState state, Model model);

    /// <inheritdoc />
    public abstract Task UpdateAsync(RequestOptions? options = null);

    private string DebuggerDisplay => $"Unknown ({Id}, Channel)";

    #region IChannel

    /// <inheritdoc />
    string IChannel.Name => string.Empty;

    #endregion
}
