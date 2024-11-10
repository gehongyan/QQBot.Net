using System.Collections.Concurrent;
using System.Collections.Immutable;
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

    /// <summary>
    ///     获取此频道的所有成员。
    /// </summary>
    public IReadOnlyCollection<SocketGuildMember> Users => _members.ToReadOnlyCollection();

    /// <summary>
    ///     获取此频道的所有角色。
    /// </summary>
    public IReadOnlyCollection<SocketRole> Roles => _roles.ToReadOnlyCollection();

    /// <summary>
    ///     获取此频道内已缓存的成员数量。
    /// </summary>
    public int DownloadedMemberCount => _members.Count;

    /// <inheritdoc />
    public bool IsAvailable { get; internal set; }

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
    ///     获取此频道中所有直播子频道。
    /// </summary>
    public IReadOnlyCollection<SocketLiveStreamChannel> LiveStreamChannels => [..Channels.OfType<SocketLiveStreamChannel>()];

    /// <summary>
    ///     获取此频道中所有应用程序子频道。
    /// </summary>
    public IReadOnlyCollection<SocketApplicationChannel> ApplicationChannels => [..Channels.OfType<SocketApplicationChannel>()];

    /// <summary>
    ///     获取此频道中所有论坛子频道。
    /// </summary>
    public IReadOnlyCollection<SocketForumChannel> ForumChannels => [..Channels.OfType<SocketForumChannel>()];

    /// <summary>
    ///     获取此频道中所有日程子频道。
    /// </summary>
    public IReadOnlyCollection<SocketScheduleChannel> ScheduleChannels => [..Channels.OfType<SocketScheduleChannel>()];

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

        IsAvailable = false;
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

    /// <summary>
    ///     获取此频道中的 <c>@全体成员</c> 全体成员角色。
    /// </summary>
    public SocketRole EveryoneRole => GetRole(0) ?? new SocketRole(this, 0);

    /// <summary>
    ///     获取此频道内的角色。
    /// </summary>
    /// <param name="id"> 要获取的角色的 ID。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的角色；如果未找到，则返回 <c>null</c>。 </returns>
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

    /// <summary>
    ///     获取此频道内的用户。
    /// </summary>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的用户；如果未找到，则返回 <c>null</c>。 </returns>
    public async Task<RestGuildMember> GetUserAsync(ulong id, RequestOptions? options = null) =>
        await GuildHelper.GetUserAsync(this, Client, id, options).ConfigureAwait(false);

    /// <summary>
    ///     获取此频道内的所有用户。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道内的所有用户。 </returns>
    public IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> GetUsersAsync(RequestOptions? options = null)
    {
        if (HasAllMembers)
            return ImmutableArray.Create(Users).ToAsyncEnumerable<IReadOnlyCollection<IGuildMember>>();
        return GuildHelper.GetUsersAsync(this, Client, null, options);
    }

    /// <inheritdoc />
    public async Task DownloadUsersAsync(RequestOptions? options = null)
    {
        await Client.DownloadUsersAsync([this], options).ConfigureAwait(false);
        HasAllMembers = true;
    }

    #endregion

    #region Channels

    /// <summary>
    ///     获取此频道内的子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketGuildChannel? GetChannel(ulong id) => Client.State.GetGuildChannel(id);

    /// <summary>
    ///     获取此子频道中具有文字聊天能力的子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的具有文字聊天能力的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketTextChannel? GetTextChannel(ulong id) => GetChannel(id) as SocketTextChannel;

    /// <summary>
    ///     获取此子频道中具有语音聊天能力的子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的具有语音聊天能力的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketVoiceChannel? GetVoiceChannel(ulong id) => GetChannel(id) as SocketVoiceChannel;

    /// <summary>
    ///     获取此子频道中的直播子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的直播子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketLiveStreamChannel? GetLiveStreamChannel(ulong id) => GetChannel(id) as SocketLiveStreamChannel;

    /// <summary>
    ///     获取此子频道中的应用程序子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的应用程序子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketApplicationChannel? GetApplicationChannel(ulong id) => GetChannel(id) as SocketApplicationChannel;

    /// <summary>
    ///     获取此子频道中的论坛子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的论坛子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketForumChannel? GetForumChannel(ulong id) => GetChannel(id) as SocketForumChannel;

    /// <summary>
    ///     获取此子频道中的日程子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的日程子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketScheduleChannel? GetScheduleChannel(ulong id) => GetChannel(id) as SocketScheduleChannel;

    /// <summary>
    ///     获取此子频道中的分组子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的分组子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketCategoryChannel? GetCategoryChannel(ulong id) => GetChannel(id) as SocketCategoryChannel;

    /// <inheritdoc cref="QQBot.IGuild.CreateTextChannelAsync(System.String,System.Action{QQBot.CreateTextChannelProperties},QQBot.RequestOptions)" />
    public Task<RestTextChannel> CreateTextChannelAsync(string name,
        Action<CreateTextChannelProperties>? func = null, RequestOptions? options = null) =>
        GuildHelper.CreateTextChannelAsync(this, Client, name, func, options);

    /// <inheritdoc cref="QQBot.IGuild.CreateVoiceChannelAsync(System.String,System.Action{QQBot.CreateVoiceChannelProperties},QQBot.RequestOptions)" />
    public Task<RestVoiceChannel> CreateVoiceChannelAsync(string name,
        Action<CreateVoiceChannelProperties>? func = null, RequestOptions? options = null) =>
        GuildHelper.CreateVoiceChannelAsync(this, Client, name, func, options);

    /// <inheritdoc cref="QQBot.IGuild.CreateLiveStreamChannelAsync(System.String,System.Action{QQBot.CreateLiveStreamChannelProperties},QQBot.RequestOptions)" />
    public Task<RestLiveStreamChannel> CreateLiveStreamChannelAsync(string name,
        Action<CreateLiveStreamChannelProperties>? func = null, RequestOptions? options = null) =>
        GuildHelper.CreateLiveStreamChannelAsync(this, Client, name, func, options);

    /// <inheritdoc cref="QQBot.IGuild.CreateApplicationChannelAsync(System.String,System.Action{QQBot.CreateApplicationChannelProperties},QQBot.RequestOptions)" />
    public Task<RestApplicationChannel> CreateApplicationChannelAsync(string name,
        Action<CreateApplicationChannelProperties>? func = null, RequestOptions? options = null) =>
        GuildHelper.CreateApplicationChannelAsync(this, Client, name, func, options);

    /// <inheritdoc cref="QQBot.IGuild.CreateForumChannelAsync(System.String,System.Action{QQBot.CreateForumChannelProperties},QQBot.RequestOptions)" />
    public Task<RestForumChannel> CreateForumChannelAsync(string name,
        Action<CreateForumChannelProperties>? func = null, RequestOptions? options = null) =>
        GuildHelper.CreateForumChannelAsync(this, Client, name, func, options);

    /// <inheritdoc cref="QQBot.IGuild.CreateScheduleChannelAsync(System.String,System.Action{QQBot.CreateScheduleChannelProperties},QQBot.RequestOptions)" />
    public Task<RestScheduleChannel> CreateScheduleChannelAsync(string name,
        Action<CreateScheduleChannelProperties>? func = null, RequestOptions? options = null) =>
        GuildHelper.CreateScheduleChannelAsync(this, Client, name, func, options);

    /// <inheritdoc cref="QQBot.IGuild.CreateCategoryChannelAsync(System.String,System.Action{QQBot.CreateCategoryChannelProperties},QQBot.RequestOptions)" />
    public Task<RestCategoryChannel> CreateCategoryChannelAsync(string name,
        Action<CreateCategoryChannelProperties>? func = null, RequestOptions? options = null) =>
        GuildHelper.CreateCategoryChannelAsync(this, Client, name, func, options);

    #endregion

    #region IGuild

    /// <inheritdoc />
    Task<IReadOnlyCollection<IGuildChannel>> IGuild.GetChannelsAsync(CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IReadOnlyCollection<IGuildChannel>>(Channels);

    /// <inheritdoc />
    Task<IGuildChannel?> IGuild.GetChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IGuildChannel?>(GetChannel(id));

    /// <inheritdoc />
    Task<IReadOnlyCollection<ITextChannel>> IGuild.GetTextChannelsAsync(CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IReadOnlyCollection<ITextChannel>>(TextChannels);

    /// <inheritdoc />
    Task<ITextChannel?> IGuild.GetTextChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<ITextChannel?>(GetTextChannel(id));

    /// <inheritdoc />
    Task<IReadOnlyCollection<IVoiceChannel>> IGuild.GetVoiceChannelsAsync(CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IReadOnlyCollection<IVoiceChannel>>(VoiceChannels);

    /// <inheritdoc />
    Task<IVoiceChannel?> IGuild.GetVoiceChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IVoiceChannel?>(GetVoiceChannel(id));

    /// <inheritdoc />
    Task<IReadOnlyCollection<ILiveStreamChannel>> IGuild.GetLiveStreamChannelsAsync(CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IReadOnlyCollection<ILiveStreamChannel>>(LiveStreamChannels);

    /// <inheritdoc />
    Task<ILiveStreamChannel?> IGuild.GetLiveStreamChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<ILiveStreamChannel?>(GetLiveStreamChannel(id));

    /// <inheritdoc />
    Task<IReadOnlyCollection<IApplicationChannel>> IGuild.GetApplicationChannelsAsync(CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IReadOnlyCollection<IApplicationChannel>>(ApplicationChannels);

    /// <inheritdoc />
    Task<IApplicationChannel?> IGuild.GetApplicationChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IApplicationChannel?>(GetApplicationChannel(id));

    /// <inheritdoc />
    Task<IReadOnlyCollection<IForumChannel>> IGuild.GetForumChannelsAsync(CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IReadOnlyCollection<IForumChannel>>(ForumChannels);

    /// <inheritdoc />
    Task<IForumChannel?> IGuild.GetForumChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IForumChannel?>(GetForumChannel(id));

    /// <inheritdoc />
    Task<IReadOnlyCollection<IScheduleChannel>> IGuild.GetScheduleChannelsAsync(CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IReadOnlyCollection<IScheduleChannel>>(ScheduleChannels);

    /// <inheritdoc />
    Task<IScheduleChannel?> IGuild.GetScheduleChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IScheduleChannel?>(GetScheduleChannel(id));

    /// <inheritdoc />
    Task<IReadOnlyCollection<ICategoryChannel>> IGuild.GetCategoryChannelsAsync(CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IReadOnlyCollection<ICategoryChannel>>(CategoryChannels);

    /// <inheritdoc />
    Task<ICategoryChannel?> IGuild.GetCategoryChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<ICategoryChannel?>(GetCategoryChannel(id));

    async Task<ITextChannel> IGuild.CreateTextChannelAsync(string name, Action<CreateTextChannelProperties>? action, RequestOptions? options) =>
        await CreateTextChannelAsync(name, action, options).ConfigureAwait(false);

    async Task<IVoiceChannel> IGuild.CreateVoiceChannelAsync(string name, Action<CreateVoiceChannelProperties>? action, RequestOptions? options) =>
        await CreateVoiceChannelAsync(name, action, options).ConfigureAwait(false);

    async Task<ILiveStreamChannel> IGuild.CreateLiveStreamChannelAsync(string name, Action<CreateLiveStreamChannelProperties>? action, RequestOptions? options) =>
        await CreateLiveStreamChannelAsync(name, action, options).ConfigureAwait(false);

    async Task<IApplicationChannel> IGuild.CreateApplicationChannelAsync(string name, Action<CreateApplicationChannelProperties>? action, RequestOptions? options) =>
        await CreateApplicationChannelAsync(name, action, options).ConfigureAwait(false);

    async Task<IForumChannel> IGuild.CreateForumChannelAsync(string name, Action<CreateForumChannelProperties>? action, RequestOptions? options) =>
        await CreateForumChannelAsync(name, action, options).ConfigureAwait(false);

    async Task<IScheduleChannel> IGuild.CreateScheduleChannelAsync(string name, Action<CreateScheduleChannelProperties>? action, RequestOptions? options) =>
        await CreateScheduleChannelAsync(name, action, options).ConfigureAwait(false);

    async Task<ICategoryChannel> IGuild.CreateCategoryChannelAsync(string name, Action<CreateCategoryChannelProperties>? action, RequestOptions? options) =>
        await CreateCategoryChannelAsync(name, action, options).ConfigureAwait(false);

    IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> IGuild.GetUsersAsync(CacheMode mode, RequestOptions? options) =>
        mode is CacheMode.AllowDownload && !HasAllMembers
            ? GetUsersAsync(options)
            : ImmutableArray.Create(Users).ToAsyncEnumerable<IReadOnlyCollection<IGuildMember>>();

    /// <inheritdoc />
    async Task<IGuildMember?> IGuild.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options)
    {
        IGuildMember? user = GetUser(id);
        if (user is not null || mode == CacheMode.CacheOnly)
            return user;
        return await GetUserAsync(id, options).ConfigureAwait(false);
    }

    #endregion
}
