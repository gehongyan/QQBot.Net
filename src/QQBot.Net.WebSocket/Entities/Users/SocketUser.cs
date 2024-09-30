namespace QQBot.WebSocket;
using Model = QQBot.API.User;

/// <summary>
///     表示一个基于网关的用户
/// </summary>
public abstract class SocketUser : SocketEntity<string>, IUser
{
    internal abstract SocketGlobalUser GlobalUser { get; }

    /// <inheritdoc />
    protected SocketUser(QQBotSocketClient client, string id)
        : base(client, id)
    {
    }

    internal virtual void Update(ClientState state, Model model) { }
    internal virtual void Update(ClientState state, API.Gateway.Author model) { }
}
