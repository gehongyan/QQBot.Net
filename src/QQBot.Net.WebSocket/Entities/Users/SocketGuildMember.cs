using System.Collections.Immutable;
using System.Diagnostics;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的子频道用户。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGuildMember : SocketGuildUser, IGuildMember, IUpdateable
{
    /// <inheritdoc cref="QQBot.IGuildMember.Guild" />
    public SocketGuild Guild { get; }

    /// <inheritdoc />
    public ulong GuildId { get; }

    /// <inheritdoc />
    public string? Nickname { get; private set; }

    /// <inheritdoc />
    /// <remarks>
    ///     如果此用户在该服务器内所拥有的角色未知，则此属性值为 <see langword="null"/>。
    ///     <note type="warning">
    ///         由于网关不会发布有关服务器用户角色变更的事件，此属性值可能并不准确。要获取准确的角色信息，请在使用此属性前调用
    ///         <see cref="QQBot.WebSocket.SocketGuildMember.UpdateAsync(QQBot.RequestOptions)"/>。
    ///     </note>
    /// </remarks>
    public IReadOnlyCollection<uint>? RoleIds { get; private set; }

    /// <summary>
    ///     获取此用户在该服务器内拥有的所有角色。
    /// </summary>
    /// <remarks>
    ///     如果此用户在该服务器内所拥有的角色未知，则此属性值为 <see langword="null"/>。
    ///     <note type="warning">
    ///         由于网关不会发布有关服务器用户角色变更的事件，此属性值可能并不准确。要获取准确的角色信息，请在使用此属性前调用
    ///         <see cref="QQBot.WebSocket.SocketGuildMember.UpdateAsync(QQBot.RequestOptions)"/>。
    ///     </note>
    /// </remarks>
    public IReadOnlyCollection<SocketRole>? Roles => RoleIds?
        .Select(x => Guild.GetRole(x) ?? new SocketRole(Guild, x))
        .Where(x => x != null)
        .ToImmutableArray();

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

    /// <inheritdoc />
    public Task UpdateAsync(RequestOptions? options = null) =>
        SocketUserHelper.UpdateAsync(this, Client, options);

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
        $"{Nickname ?? Username}{(Nickname is not null ? Username : string.Empty)} ({Id}{(IsBot ?? false ? ", Bot" : "")}, Guild)";

    #region IGuild

    /// <inheritdoc />
    IGuild IGuildMember.Guild => Guild;

    #endregion
}
