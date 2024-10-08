using Model = QQBot.API.SelfUser;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的当前用户。
/// </summary>
public class SocketSelfUser : SocketUser, ISelfUser
{
    /// <inheritdoc cref="QQBot.ISelfUser.Id" />
    public new ulong Id { get; }

    internal override SocketGlobalUser GlobalUser { get; }

    /// <inheritdoc />
    public string Username { get; internal set; } = string.Empty;

    /// <inheritdoc />
    public override string? Avatar { get; internal set; }

    /// <inheritdoc />
    public bool? IsBot { get; internal set; }

    /// <inheritdoc />
    public string? UnionOpenId { get; internal set; }

    /// <inheritdoc />
    public string? UnionUserAccount { get; internal set; }

    /// <inheritdoc />
    internal SocketSelfUser(QQBotSocketClient client, SocketGlobalUser globalUser)
        : base(client, globalUser.Id)
    {
        Id = ulong.Parse(globalUser.Id);
        GlobalUser = globalUser;
    }

    internal static SocketSelfUser Create(QQBotSocketClient client, ClientState state, Model model)
    {
        SocketSelfUser entity = new(client, client.GetOrCreateSelfUser(state, model));
        entity.Update(state, model);
        return entity;
    }

    internal void Update(ClientState state, Model model)
    {
        base.Update(state, model);
    }
}
