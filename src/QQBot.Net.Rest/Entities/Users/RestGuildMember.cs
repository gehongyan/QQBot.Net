using QQBot.API;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的子频道内用户。
/// </summary>
public class RestGuildMember : RestGuildUser, IGuildMember
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
    internal RestGuildMember(BaseQQBotClient client, IGuild guild, ulong id)
        : base(client, id)
    {
        Guild = guild;
        GuildId = guild.Id;
        Nickname = string.Empty;
    }

    internal static RestGuildMember Create(BaseQQBotClient client, IGuild guild, User userModel, Member? memberModel)
    {
        RestGuildMember entity = new(client, guild, userModel.Id);
        entity.Update(userModel, memberModel);
        return entity;
    }

    internal void Update(User userModel, Member? memberModel)
    {
        base.Update(userModel);
        Nickname = memberModel?.Nickname;
        RoleIds = memberModel?.Roles;
        JoinedAt = memberModel?.JoinedAt;
    }

    /// <inheritdoc />
    public Task KickAsync(bool addBlacklist = false, int pruneDays = 0, RequestOptions? options = null) =>
        UserHelper.KickAsync(this, Client, addBlacklist, pruneDays, options);
}
