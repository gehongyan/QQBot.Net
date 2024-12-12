using System.Diagnostics;
using QQBot.API;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的子频道内用户。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
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

    #region Roles

    /// <inheritdoc />
    public Task AddRoleAsync(uint roleId, RequestOptions? options = null) =>
        AddRolesAsync([roleId], options);

    /// <inheritdoc />
    public Task AddRoleAsync(IRole role, RequestOptions? options = null) =>
        AddRoleAsync(role.Id, options);

    /// <inheritdoc />
    public Task AddRolesAsync(IEnumerable<uint> roleIds, RequestOptions? options = null) =>
        UserHelper.AddRolesAsync(this, Client, roleIds, options);

    /// <inheritdoc />
    public Task AddRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null) =>
        AddRolesAsync(roles.Select(x => x.Id), options);

    /// <inheritdoc />
    public Task RemoveRoleAsync(uint roleId, RequestOptions? options = null) =>
        RemoveRolesAsync([roleId], options);

    /// <inheritdoc />
    public Task RemoveRoleAsync(IRole role, RequestOptions? options = null) =>
        RemoveRoleAsync(role.Id, options);

    /// <inheritdoc />
    public Task RemoveRolesAsync(IEnumerable<uint> roleIds, RequestOptions? options = null) =>
        UserHelper.RemoveRolesAsync(this, Client, roleIds, options);

    /// <inheritdoc />
    public Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null) =>
        RemoveRolesAsync(roles.Select(x => x.Id));

    #endregion

    /// <inheritdoc cref="QQBot.Rest.RestGuildUser.Username" />
    public override string ToString() => Username;

    private string DebuggerDisplay =>
        $"{Nickname ?? Username} ({(Nickname is not null ? $"{Username}, " : string.Empty)}{Id}{(IsBot ?? false ? ", Bot" : "")}, Guild)";
}
