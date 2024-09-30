namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道用户。
/// </summary>
public class SocketGuildMember : SocketGuildUser, IGuildMember
{
    internal override SocketGlobalUser GlobalUser { get; }

    /// <inheritdoc />
    public override string Username { get; internal set; } = string.Empty;

    /// <inheritdoc />
    public override string? Avatar { get; internal set; }

    /// <inheritdoc />
    public override bool? IsBot { get; internal set; }

    /// <inheritdoc />
    public override string? UnionOpenId { get; internal set; }

    /// <inheritdoc />
    public override string? UnionUserAccount { get; internal set; }

    /// <inheritdoc />
    public string Nickname { get; private set; }

    /// <inheritdoc />
    public IReadOnlyCollection<uint> RoleIds { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset JoinedAt { get; private set; }

    /// <inheritdoc />
    internal SocketGuildMember(SocketGuild guild, SocketGlobalUser globalUser)
        : base(guild.Client, ulong.Parse(globalUser.Id))
    {
        GlobalUser = globalUser;
        Nickname = string.Empty;
        RoleIds = [];
        JoinedAt = default;
    }

    internal static SocketGuildMember Create(SocketGuild guild, ClientState state, API.User userModel, API.Member memberModel)
    {
        SocketGuildMember entity = new(guild, guild.Client.GetOrCreateUser(state, userModel));
        entity.Update(state, userModel, memberModel);
        return entity;
    }

    internal void Update(ClientState state, API.User userModel, API.Member model)
    {
        base.Update(state, userModel);
        Nickname = model.Nickname;
        RoleIds = model.Roles;
        JoinedAt = model.JoinedAt;
    }
}
