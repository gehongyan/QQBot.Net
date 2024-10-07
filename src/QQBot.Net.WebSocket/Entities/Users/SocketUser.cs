using System.Diagnostics;
using Model = QQBot.API.User;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的用户
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class SocketUser : SocketEntity<string>, IUser
{
    internal abstract SocketGlobalUser GlobalUser { get; }

    /// <inheritdoc />
    public string Mention => MentionUtils.MentionUser(this);

    /// <inheritdoc />
    protected SocketUser(QQBotSocketClient client, string id)
        : base(client, id)
    {
    }

    internal virtual void Update(ClientState state, Model model) { }
    internal virtual void Update(ClientState state, API.Gateway.Author model) { }

    private string DebuggerDisplay => $"Unknown ({Id})";
}
