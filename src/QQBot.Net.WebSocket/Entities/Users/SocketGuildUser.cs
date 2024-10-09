using System.Diagnostics;
using QQBot.API;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道用户。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class SocketGuildUser : SocketUser, IGuildUser
{
    /// <inheritdoc cref="QQBot.IGuildUser.Id" />
    public new ulong Id { get; }

    /// <inheritdoc />
    public abstract string Username { get; internal set; }

    /// <inheritdoc />
    public abstract bool? IsBot { get; internal set; }

    /// <inheritdoc />
    public abstract string? UnionOpenId { get; internal set; }

    /// <inheritdoc />
    public abstract string? UnionUserAccount { get; internal set; }

    /// <inheritdoc />
    internal SocketGuildUser(QQBotSocketClient client, ulong id)
        : base(client, id.ToString())
    {
        Id = id;
    }

    internal override void Update(ClientState state, User model)
    {
        Username = model.Username;
        Avatar = model.Avatar;
        IsBot = model.IsBot;
        UnionOpenId = model.UnionOpenId;
        UnionUserAccount = model.UnionUserAccount;
    }

    private string DebuggerDisplay =>
        $"{Username} ({Id}{(IsBot ?? false ? ", Bot" : "")}, Guild)";
}
