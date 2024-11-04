using System.Diagnostics;

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

    /// <summary>
    ///     获取此频道中的一个用户。
    /// </summary>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <returns> 如果找到了具有指定 ID 的用户，则返回该用户；否则返回 <c>null</c>。 </returns>
    public SocketUser? GetUser(string id) => GetUserInternal(id);

    /// <inheritdoc cref="QQBot.WebSocket.SocketChannel.GetUser(System.String)" />
    protected virtual SocketUser? GetUserInternal(string id) => null;

    private string DebuggerDisplay => $"Unknown ({Id}, Channel)";

    #region IChannel

    /// <inheritdoc />
    Task<IUser?> IChannel.GetUserAsync(string id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IUser?>(ulong.TryParse(id, out ulong userId) ? GetUserInternal(userId.ToIdString()) : null);

    #endregion
}
