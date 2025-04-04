﻿using System.Diagnostics;
using QQBot.API;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道上下文中的用户。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGuildUser : SocketUser, IGuildUser
{
    internal SocketGlobalUser _globalUser;

    /// <inheritdoc />
    internal override SocketGlobalUser GlobalUser => _globalUser;

    /// <inheritdoc />
    public override string? Avatar
    {
        get => GlobalUser.Avatar;
        internal set => GlobalUser.Avatar = value;
    }

    /// <inheritdoc cref="QQBot.IGuildUser.Id" />
    public new ulong Id => ulong.Parse(GlobalUser.Id);

    /// <inheritdoc />
    public string Username { get; private set; }

    /// <inheritdoc />
    public bool? IsBot { get; private set; }

    /// <inheritdoc />
    public string? UnionOpenId { get; private set; }

    /// <inheritdoc />
    public string? UnionUserAccount { get; private set; }

    /// <inheritdoc />
    internal SocketGuildUser(QQBotSocketClient client, SocketGlobalUser globalUser)
        : base(client, globalUser.Id)
    {
        _globalUser = globalUser;
        Username = string.Empty;
    }

    internal static SocketGuildUser Create(QQBotSocketClient client, ClientState state, User model)
    {
        SocketGlobalUser globalUser = state.GetOrAddGlobalUser(model.Id, _ =>
        {
            SocketGlobalUser user = SocketGlobalUser.Create(client, state, model);
            user.GlobalUser.AddRef();
            return user;
        });
        SocketGuildUser entity = new(client, globalUser);
        entity.Update(state, model);
        return entity;
    }

    internal override void Update(ClientState state, User model)
    {
        Username = model.Username;
        Avatar = model.Avatar;
        IsBot = model.IsBot;
        UnionOpenId = model.UnionOpenId;
        UnionUserAccount = model.UnionUserAccount;
    }

    /// <inheritdoc cref="QQBot.WebSocket.SocketGuildUser.Username" />
    public override string ToString() => Username;

    private string DebuggerDisplay => $"{Username} ({Id}{(IsBot ?? false ? ", Bot" : "")}, Guild)";
}
