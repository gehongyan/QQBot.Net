using System.Collections.Concurrent;
using QQBot.Rest;
using Model = QQBot.API.Guild;
using ChannelModel = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的子频道。
/// </summary>
public class SocketGuild : SocketEntity<ulong>, IGuild, IUpdateable
{
    private readonly ConcurrentDictionary<ulong, SocketGuildChannel> _channels;
    private readonly ConcurrentDictionary<ulong, SocketGuildMember> _members;
    private readonly ConcurrentDictionary<uint, SocketRole> _roles;

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public ulong OwnerId { get; private set; }

    /// <inheritdoc />
    public bool IsOwner { get; private set; }

    /// <inheritdoc />
    public int MemberCount { get; private set; }

    /// <inheritdoc />
    public int MaxMembers { get; private set; }

    /// <inheritdoc />
    public string Description { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset JoinedAt { get; private set; }

    /// <inheritdoc />
    public int MaxRoles { get; private set; }

    /// <inheritdoc cref="QQBot.IGuild.Roles" />
    public IReadOnlyCollection<SocketRole> Roles => _roles.ToReadOnlyCollection();

    /// <summary>
    ///     获取此频道内已缓存的成员数量。
    /// </summary>
    public int DownloadedMemberCount => _members.Count;

    /// <inheritdoc />
    public bool IsAvailable { get; private set; }

    /// <summary>
    ///     获取此频道是否已连接至网关。
    /// </summary>
    public bool IsConnected { get; internal set; }

    /// <summary>
    ///     获取是否已下载此频道的所有成员至缓存。
    /// </summary>
    /// <remarks>
    ///     当调用 <see cref="QQBot.WebSocket.SocketGuild.DownloadUsersAsync(QQBot.RequestOptions)"/>
    ///     方法并成功下载所有成员时，此属性将会被设置为 <see langword="true"/>。
    /// </remarks>
    public bool HasAllMembers { get; internal set; }

    /// <summary>
    ///     获取此频道中所有具有文字聊天能力的子频道。
    /// </summary>
    /// <remarks>
    ///     语音子频道也是一种文字子频道，此属性本意用于获取所有具有文字聊天能力的子频道，通过此方法获取到的文字子频道列表中也包含了语音子频道。
    ///     如需获取子频道的实际类型，请参考 <see cref="QQBot.ChannelExtensions.GetChannelType(QQBot.IChannel)"/>。
    /// </remarks>
    public IReadOnlyCollection<SocketTextChannel> TextChannels => [..Channels.OfType<SocketTextChannel>()];

    /// <summary>
    ///     获取此频道中所有具有语音聊天能力的子频道。
    /// </summary>
    public IReadOnlyCollection<SocketVoiceChannel> VoiceChannels => [..Channels.OfType<SocketVoiceChannel>()];

    /// <summary>
    ///     获取此频道中的所有分组子频道。
    /// </summary>
    public IReadOnlyCollection<SocketCategoryChannel> CategoryChannels => [..Channels.OfType<SocketCategoryChannel>()];

    /// <summary>
    ///     获取此频道的所有子频道。
    /// </summary>
    public IReadOnlyCollection<SocketGuildChannel> Channels => [.._channels.Values];

    /// <summary>
    ///     获取当前登录的用户。
    /// </summary>
    public SocketGuildMember? CurrentUser =>
        Client.CurrentUser is { Id: var id } && _members.TryGetValue(id, out SocketGuildMember? member) ? member : null;

    /// <inheritdoc />
    internal SocketGuild(QQBotSocketClient client, ulong id)
        : base(client, id)
    {
        Name = string.Empty;
        Description = string.Empty;
        _channels = [];
        _members = [];
        _roles = [];
    }

    internal static SocketGuild Create(QQBotSocketClient client, ClientState state, Model model)
    {
        SocketGuild entity = new(client, model.Id);
        entity.Update(state, model);
        return entity;
    }

    internal void Update(ClientState state, Model model)
    {
        Name = model.Name;
        OwnerId = model.OwnerId;
        IsOwner = model.OwnerId == Client.CurrentUser?.Id;
        MemberCount = model.MemberCount;
        MaxMembers = model.MaxMembers;
        Description = model.Description;
        JoinedAt = model.JoinedAt;

        IsAvailable = true;
    }

    internal void Update(ClientState state, IReadOnlyCollection<ChannelModel> models)
    {
        foreach (ChannelModel model in models)
        {
            if (_channels.TryGetValue(model.Id, out SocketGuildChannel? existing))
            {
                existing.Update(state, model);
                continue;
            }
            SocketGuildChannel channel = SocketGuildChannel.Create(this, state, model);
            state.AddGuildChannel(channel);
            _channels.TryAdd(channel.Id, channel);
        }
    }

    internal void Update(ClientState state, API.Rest.GetGuildRolesResponse model)
    {
        MaxRoles = model.RoleNumLimit;
        foreach (API.Role roleModel in model.Roles)
        {
            if (_roles.TryGetValue(roleModel.Id, out SocketRole? existing))
            {
                existing.Update(state, roleModel);
                continue;
            }
            SocketRole role = SocketRole.Create(this, state, roleModel);
            _roles.TryAdd(role.Id, role);
        }
    }

    /// <inheritdoc />
    public Task UpdateAsync(RequestOptions? options = null) => SocketGuildHelper.UpdateAsync(this, Client, options);

    #region Roles

    /// <inheritdoc cref="QQBot.IGuild.GetRole(System.UInt32)" />
    public SocketRole? GetRole(uint id) => _roles.GetValueOrDefault(id);

    #endregion

    #region Users

    /// <summary>
    ///     获取此频道内的用户。
    /// </summary>
    /// <remarks>
    ///     此方法可能返回 <c>null</c>，因为在大型子频道中，用户列表的缓存可能不完整。
    /// </remarks>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的用户；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketGuildMember? GetUser(ulong id) => _members.GetValueOrDefault(id);

    internal SocketGuildMember AddOrUpdateUser(API.User userModel, API.Member? memberModel)
    {
        if (_members.TryGetValue(userModel.Id, out SocketGuildMember? cachedMember))
        {
            cachedMember.Update(Client.State, userModel, memberModel);
            return cachedMember;
        }

        SocketGuildMember member = SocketGuildMember.Create(this, Client.State, userModel, memberModel);
        member.GlobalUser.AddRef();
        _members[member.Id] = member;
        return member;
    }

    /// <inheritdoc />
    public async Task DownloadUsersAsync(RequestOptions? options = null) =>
        await Client.DownloadUsersAsync([this], options).ConfigureAwait(false);

    #endregion

    #region Channels

    /// <summary>
    ///     获取此频道内的子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketGuildChannel? GetChannel(ulong id) => Client.State.GetGuildChannel(id);

    /// <summary>
    ///     获取此子频道中所有具有文字聊天能力的子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketTextChannel? GetTextChannel(ulong id) => GetChannel(id) as SocketTextChannel;

    #endregion

    #region IGuild

    /// <inheritdoc />
    IReadOnlyCollection<IRole> IGuild.Roles => Roles;

    /// <inheritdoc />
    IRole? IGuild.GetRole(uint id) => GetRole(id);

    /// <inheritdoc />
    async Task<IGuildMember?> IGuild.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options)
    {
        IGuildMember? user = GetUser(id);
        if (user is not null || mode == CacheMode.CacheOnly)
            return user;
        return await GuildHelper.GetUserAsync(this, Client, id, options).ConfigureAwait(false);
    }

    #endregion
}
