using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道用户。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGuildMember : SocketGuildUser, IGuildMember
{
    /// <inheritdoc />
    public IGuild Guild { get; }

    /// <inheritdoc />
    public ulong GuildId { get; }

    /// <inheritdoc />
    public string? Nickname { get; private set; }

    /// <inheritdoc />
    public IReadOnlyCollection<uint>? RoleIds { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? JoinedAt { get; private set; }

    /// <inheritdoc />
    internal SocketGuildMember(SocketGuild guild, SocketGlobalUser globalUser)
        : base(guild.Client, globalUser)
    {
        Guild = guild;
        GuildId = guild.Id;
    }

    internal static SocketGuildMember Create(SocketGuild guild, ClientState state, API.User userModel, API.Member? memberModel)
    {
        SocketGuildMember entity = new(guild, guild.Client.GetOrCreateUser(state, userModel));
        entity.Update(state, userModel, memberModel);
        return entity;
    }

    internal void Update(ClientState state, API.User userModel, API.Member? model)
    {
        base.Update(state, userModel);
        Nickname = model?.Nickname;
        RoleIds = model?.Roles;
        JoinedAt = model?.JoinedAt;
    }

    private string DebuggerDisplay =>
        $"{Nickname ?? Username} ({Id}{(IsBot ?? false ? ", Bot" : "")}, Guild Member)";
}
