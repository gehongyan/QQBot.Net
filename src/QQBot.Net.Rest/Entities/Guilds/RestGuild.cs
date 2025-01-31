﻿using System.Diagnostics;
using Model = QQBot.API.Guild;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestGuild : RestEntity<ulong>, IGuild
{
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

    internal bool IsAvailable { get; set; }

    /// <inheritdoc />
    internal RestGuild(BaseQQBotClient client, ulong id)
        : base(client, id)
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    internal static RestGuild Create(BaseQQBotClient client, Model model)
    {
        RestGuild entity = new(client, model.Id);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
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

    /// <inheritdoc cref="QQBot.Rest.RestGuild.Name" />
    public override string ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id})";

    #region Channels

    /// <summary>
    ///     获取此频道的所有子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有子频道。 </returns>
    public Task<IReadOnlyCollection<RestGuildChannel>> GetChannelsAsync(RequestOptions? options = null) =>
        GuildHelper.GetChannelsAsync(this, Client, options);

    /// <summary>
    ///     获取此频道内的子频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public Task<RestGuildChannel> GetChannelAsync(ulong id, RequestOptions? options = null) =>
        GuildHelper.GetChannelAsync(this, Client, id, options);

    /// <summary>
    ///     获取此频道的所有具有文字聊天能力的子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有具有文字聊天能力的子频道。 </returns>
    public async Task<IReadOnlyCollection<ITextChannel>> GetTextChannelsAsync(RequestOptions? options = null)
    {
        IReadOnlyCollection<RestGuildChannel> channels = await GuildHelper.GetChannelsAsync(this, Client, options).ConfigureAwait(false);
        return channels.OfType<ITextChannel>().ToArray();
    }

    /// <summary>
    ///     获取此频道内的具有文字聊天能力的子频道。
    /// </summary>
    /// <param name="id"> 要获取的具有文字聊天能力的子频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的具有文字聊天能力的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public async Task<ITextChannel?> GetTextChannelAsync(ulong id, RequestOptions? options = null)
    {
        RestGuildChannel channel = await GuildHelper.GetChannelAsync(this, Client, id, options).ConfigureAwait(false);
        return channel as ITextChannel;
    }

    /// <summary>
    ///     获取此频道的所有具有语音聊天能力的子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有具有语音聊天能力的子频道。 </returns>
    public async Task<IReadOnlyCollection<IVoiceChannel>> GetVoiceChannelsAsync(RequestOptions? options = null)
    {
        IReadOnlyCollection<RestGuildChannel> channels = await GuildHelper.GetChannelsAsync(this, Client, options).ConfigureAwait(false);
        return channels.OfType<IVoiceChannel>().ToArray();
    }

    /// <summary>
    ///     获取此频道内的具有语音聊天能力的子频道。
    /// </summary>
    /// <param name="id"> 要获取的具有语音聊天能力的子频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的具有语音聊天能力的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public async Task<IVoiceChannel?> GetVoiceChannelAsync(ulong id, RequestOptions? options = null)
    {
        RestGuildChannel channel = await GuildHelper.GetChannelAsync(this, Client, id, options).ConfigureAwait(false);
        return channel as IVoiceChannel;
    }

    /// <summary>
    ///     获取此频道的所有直播子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有直播子频道。 </returns>
    public async Task<IReadOnlyCollection<ILiveStreamChannel>> GetLiveStreamChannelsAsync(RequestOptions? options = null)
    {
        IReadOnlyCollection<RestGuildChannel> channels = await GuildHelper.GetChannelsAsync(this, Client, options).ConfigureAwait(false);
        return channels.OfType<ILiveStreamChannel>().ToArray();
    }

    /// <summary>
    ///     获取此频道内的直播子频道。
    /// </summary>
    /// <param name="id"> 要获取的直播子频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的直播子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public async Task<ILiveStreamChannel?> GetLiveStreamChannelAsync(ulong id, RequestOptions? options = null)
    {
        RestGuildChannel channel = await GuildHelper.GetChannelAsync(this, Client, id, options).ConfigureAwait(false);
        return channel as ILiveStreamChannel;
    }

    /// <summary>
    ///     获取此频道的所有应用子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有应用子频道。 </returns>
    public async Task<IReadOnlyCollection<IApplicationChannel>> GetApplicationChannelsAsync(RequestOptions? options = null)
    {
        IReadOnlyCollection<RestGuildChannel> channels = await GuildHelper.GetChannelsAsync(this, Client, options).ConfigureAwait(false);
        return channels.OfType<IApplicationChannel>().ToArray();
    }

    /// <summary>
    ///     获取此频道内的应用子频道。
    /// </summary>
    /// <param name="id"> 要获取的应用子频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的应用子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public async Task<IApplicationChannel?> GetApplicationChannelAsync(ulong id, RequestOptions? options = null)
    {
        RestGuildChannel channel = await GuildHelper.GetChannelAsync(this, Client, id, options).ConfigureAwait(false);
        return channel as IApplicationChannel;
    }

    /// <summary>
    ///     获取此频道的所有论坛子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有论坛子频道。 </returns>
    public async Task<IReadOnlyCollection<IForumChannel>> GetForumChannelsAsync(RequestOptions? options = null)
    {
        IReadOnlyCollection<RestGuildChannel> channels = await GuildHelper.GetChannelsAsync(this, Client, options).ConfigureAwait(false);
        return channels.OfType<IForumChannel>().ToArray();
    }

    /// <summary>
    ///     获取此频道内的论坛子频道。
    /// </summary>
    /// <param name="id"> 要获取的论坛子频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的论坛子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public async Task<IForumChannel?> GetForumChannelAsync(ulong id, RequestOptions? options = null)
    {
        RestGuildChannel channel = await GuildHelper.GetChannelAsync(this, Client, id, options).ConfigureAwait(false);
        return channel as IForumChannel;
    }

    /// <summary>
    ///     获取此频道的所有日程子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有日程子频道。 </returns>
    public async Task<IReadOnlyCollection<IScheduleChannel>> GetScheduleChannelsAsync(RequestOptions? options = null)
    {
        IReadOnlyCollection<RestGuildChannel> channels = await GuildHelper.GetChannelsAsync(this, Client, options).ConfigureAwait(false);
        return channels.OfType<IScheduleChannel>().ToArray();
    }

    /// <summary>
    ///     获取此频道内的日程子频道。
    /// </summary>
    /// <param name="id"> 要获取的论坛子频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的日程子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public async Task<IScheduleChannel?> GetScheduleChannelAsync(ulong id, RequestOptions? options = null)
    {
        RestGuildChannel channel = await GuildHelper.GetChannelAsync(this, Client, id, options).ConfigureAwait(false);
        return channel as IScheduleChannel;
    }

    /// <summary>
    ///     获取此频道的所有分组子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有分组子频道。 </returns>
    public async Task<IReadOnlyCollection<ICategoryChannel>> GetCategoryChannelsAsync(RequestOptions? options = null)
    {
        IReadOnlyCollection<RestGuildChannel> channels = await GuildHelper.GetChannelsAsync(this, Client, options).ConfigureAwait(false);
        return channels.OfType<ICategoryChannel>().ToArray();
    }

    /// <summary>
    ///     获取此频道内的分组子频道。
    /// </summary>
    /// <param name="id"> 要获取的分组子频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的分组子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public async Task<ICategoryChannel?> GetCategoryChannelAsync(ulong id, RequestOptions? options = null)
    {
        RestGuildChannel channel = await GuildHelper.GetChannelAsync(this, Client, id, options).ConfigureAwait(false);
        return channel as ICategoryChannel;
    }

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

    #region Roles

    /// <summary>
    ///     获取此频道的所有角色。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有角色。 </returns>
    public Task<IReadOnlyCollection<RestRole>> GetRolesAsync(RequestOptions? options = null) =>
        GuildHelper.GetRolesAsync(this, Client, options);

    /// <summary>
    ///     获取此频道的角色。
    /// </summary>
    /// <param name="id"> 要获取的角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有角色。 </returns>
    public Task<RestRole?> GetRoleAsync(uint id, RequestOptions? options = null) =>
        GuildHelper.GetRoleAsync(this, Client, id, options);

    /// <inheritdoc cref="QQBot.IGuild.CreateRoleAsync(System.Action{QQBot.RoleProperties},QQBot.RequestOptions)" />
    public Task<RestRole> CreateRoleAsync(Action<RoleProperties>? func = null, RequestOptions? options = null) =>
        GuildHelper.CreateRoleAsync(this, Client, func, options);

    #endregion

    #region Users

    /// <summary>
    ///     获取此频道内的用户。
    /// </summary>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的用户；如果未找到，则返回 <c>null</c>。 </returns>
    public Task<RestGuildMember> GetUserAsync(ulong id, RequestOptions? options = null) =>
        GuildHelper.GetUserAsync(this, Client, id, options);

    /// <summary>
    ///     获取此频道内的所有用户。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道内的所有用户。 </returns>
    public IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> GetUsersAsync(RequestOptions? options = null) =>
        GuildHelper.GetUsersAsync(this, Client, null, options);

    #endregion

    #region IGuild

    /// <inheritdoc />
    bool IGuild.IsAvailable => IsAvailable;

    /// <inheritdoc />
    async Task<IGuildMember?> IGuild.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetUserAsync(id, options).ConfigureAwait(false) : null;

    IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> IGuild.GetUsersAsync(CacheMode mode, RequestOptions? options) =>
        mode is CacheMode.AllowDownload ? GetUsersAsync(options) : AsyncEnumerable.Empty<IReadOnlyCollection<IGuildMember>>();

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Downloading users is not supported for a REST-based guild.</exception>
    Task IGuild.DownloadUsersAsync(RequestOptions? options) => throw new NotSupportedException();

    /// <inheritdoc />
    async Task<IReadOnlyCollection<IGuildChannel>> IGuild.GetChannelsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetChannelsAsync(options).ConfigureAwait(false) : [];

    /// <inheritdoc />
    async Task<IGuildChannel?> IGuild.GetChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetChannelAsync(id, options).ConfigureAwait(false) : null;

    /// <inheritdoc />
    async Task<IReadOnlyCollection<ITextChannel>> IGuild.GetTextChannelsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetTextChannelsAsync(options).ConfigureAwait(false) : [];

    /// <inheritdoc />
    async Task<ITextChannel?> IGuild.GetTextChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetTextChannelAsync(id, options).ConfigureAwait(false) : null;

    /// <inheritdoc />
    async Task<IReadOnlyCollection<IVoiceChannel>> IGuild.GetVoiceChannelsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetVoiceChannelsAsync(options).ConfigureAwait(false) : [];

    /// <inheritdoc />
    async Task<IVoiceChannel?> IGuild.GetVoiceChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetVoiceChannelAsync(id, options).ConfigureAwait(false) : null;

    /// <inheritdoc />
    async Task<IReadOnlyCollection<ILiveStreamChannel>> IGuild.GetLiveStreamChannelsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetLiveStreamChannelsAsync(options).ConfigureAwait(false) : [];

    /// <inheritdoc />
    async Task<ILiveStreamChannel?> IGuild.GetLiveStreamChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetLiveStreamChannelAsync(id, options).ConfigureAwait(false) : null;

    /// <inheritdoc />
    async Task<IReadOnlyCollection<IApplicationChannel>> IGuild.GetApplicationChannelsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetApplicationChannelsAsync(options).ConfigureAwait(false) : [];

    /// <inheritdoc />
    async Task<IApplicationChannel?> IGuild.GetApplicationChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetApplicationChannelAsync(id, options).ConfigureAwait(false) : null;

    /// <inheritdoc />
    async Task<IReadOnlyCollection<IForumChannel>> IGuild.GetForumChannelsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetForumChannelsAsync(options).ConfigureAwait(false) : [];

    /// <inheritdoc />
    async Task<IForumChannel?> IGuild.GetForumChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetForumChannelAsync(id, options).ConfigureAwait(false) : null;

    /// <inheritdoc />
    async Task<IReadOnlyCollection<IScheduleChannel>> IGuild.GetScheduleChannelsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetScheduleChannelsAsync(options).ConfigureAwait(false) : [];

    /// <inheritdoc />
    async Task<IScheduleChannel?> IGuild.GetScheduleChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetScheduleChannelAsync(id, options).ConfigureAwait(false) : null;

    /// <inheritdoc />
    async Task<IReadOnlyCollection<ICategoryChannel>> IGuild.GetCategoryChannelsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetCategoryChannelsAsync(options).ConfigureAwait(false) : [];

    /// <inheritdoc />
    async Task<ICategoryChannel?> IGuild.GetCategoryChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetCategoryChannelAsync(id, options).ConfigureAwait(false) : null;

    async Task<ITextChannel> IGuild.CreateTextChannelAsync(string name, Action<CreateTextChannelProperties>? func, RequestOptions? options) =>
        await CreateTextChannelAsync(name, func, options).ConfigureAwait(false);

    async Task<IVoiceChannel> IGuild.CreateVoiceChannelAsync(string name, Action<CreateVoiceChannelProperties>? func, RequestOptions? options) =>
        await CreateVoiceChannelAsync(name, func, options).ConfigureAwait(false);

    async Task<ILiveStreamChannel> IGuild.CreateLiveStreamChannelAsync(string name, Action<CreateLiveStreamChannelProperties>? func, RequestOptions? options) =>
        await CreateLiveStreamChannelAsync(name, func, options).ConfigureAwait(false);

    async Task<IApplicationChannel> IGuild.CreateApplicationChannelAsync(string name, Action<CreateApplicationChannelProperties>? func, RequestOptions? options) =>
        await CreateApplicationChannelAsync(name, func, options).ConfigureAwait(false);

    async Task<IForumChannel> IGuild.CreateForumChannelAsync(string name, Action<CreateForumChannelProperties>? func, RequestOptions? options) =>
        await CreateForumChannelAsync(name, func, options).ConfigureAwait(false);

    async Task<IScheduleChannel> IGuild.CreateScheduleChannelAsync(string name, Action<CreateScheduleChannelProperties>? func, RequestOptions? options) =>
        await CreateScheduleChannelAsync(name, func, options).ConfigureAwait(false);

    async Task<ICategoryChannel> IGuild.CreateCategoryChannelAsync(string name, Action<CreateCategoryChannelProperties>? func, RequestOptions? options) =>
        await CreateCategoryChannelAsync(name, func, options).ConfigureAwait(false);

    async Task<IReadOnlyCollection<IRole>> IGuild.GetRolesAsync(CacheMode mode, RequestOptions? options) =>
        mode is CacheMode.CacheOnly ? ([]) : await GetRolesAsync(options).ConfigureAwait(false);

    async Task<IRole?> IGuild.GetRoleAsync(uint id, CacheMode mode, RequestOptions? options) =>
        mode is CacheMode.CacheOnly ? null : await GetRoleAsync(id, options).ConfigureAwait(false);

    /// <inheritdoc />
    async Task<IRole> IGuild.CreateRoleAsync(Action<RoleProperties> func, RequestOptions? options) =>
        await CreateRoleAsync(func, options);

    #endregion

    #region API Permissions

    /// <inheritdoc />
    public Task<IReadOnlyCollection<ApplicationPermission>> GetApplicationPermissionsAsync(RequestOptions? options = null) =>
        GuildHelper.GetApplicationPermissionsAsync(this, Client, options);

    /// <inheritdoc />
    public Task RequestApplicationPermissionAsync(ulong channelId,
        string title, ApplicationPermission permission, RequestOptions? options = null) =>
        GuildHelper.RequestApplicationPermissionAsync(this, Client, channelId, title, permission, options);

    /// <inheritdoc />
    public Task RequestApplicationPermissionAsync(ITextChannel channel,
        string title, ApplicationPermission permission, RequestOptions? options = null) =>
        GuildHelper.RequestApplicationPermissionAsync(this, Client, channel, title, permission, options);

    /// <inheritdoc />
    public Task RequestApplicationPermissionAsync(ulong channelId,
        string title, string description, HttpMethod method, string path, RequestOptions? options = null) =>
        GuildHelper.RequestApplicationPermissionAsync(this, Client, channelId, title, description, method, path, options);

    /// <inheritdoc />
    public Task RequestApplicationPermissionAsync(ITextChannel channel,
        string title, string description, HttpMethod method, string path, RequestOptions? options = null) =>
        GuildHelper.RequestApplicationPermissionAsync(this, Client, channel, title, description, method, path, options);

    #endregion

    #region Message Settings

    /// <inheritdoc />
    public Task<MessageSetting> GetMessageSettingAsync(RequestOptions? options = null) =>
        GuildHelper.GetMessageSettingAsync(this, Client, options);

    /// <inheritdoc />
    public Task MuteEveryoneAsync(TimeSpan duration, RequestOptions? options = null) =>
        GuildHelper.MuteEveryoneAsync(this, Client, duration, options);

    /// <inheritdoc />
    public Task MuteEveryoneAsync(DateTimeOffset until, RequestOptions? options = null) =>
        GuildHelper.MuteEveryoneAsync(this, Client, until, options);

    /// <inheritdoc />
    public Task UnmuteEveryoneAsync(RequestOptions? options = null) =>
        GuildHelper.UnmuteEveryoneAsync(this, Client, options);

    /// <inheritdoc />
    public Task MuteMemberAsync(IGuildMember user, TimeSpan duration, RequestOptions? options = null) =>
        GuildHelper.MuteMemberAsync(this, Client, user.Id, duration, options);

    /// <inheritdoc />
    public Task MuteMemberAsync(ulong userId, TimeSpan duration, RequestOptions? options = null) =>
        GuildHelper.MuteMemberAsync(this, Client, userId, duration, options);

    /// <inheritdoc />
    public Task MuteMemberAsync(IGuildMember user, DateTimeOffset until, RequestOptions? options = null) =>
        GuildHelper.MuteMemberAsync(this, Client, user.Id, until, options);

    /// <inheritdoc />
    public Task MuteMemberAsync(ulong userId, DateTimeOffset until, RequestOptions? options = null) =>
        GuildHelper.MuteMemberAsync(this, Client, userId, until, options);

    /// <inheritdoc />
    public Task UnmuteMemberAsync(IGuildMember user, RequestOptions? options = null) =>
        GuildHelper.UnmuteMemberAsync(this, Client, user.Id, options);

    /// <inheritdoc />
    public Task UnmuteMemberAsync(ulong userId, RequestOptions? options = null) =>
        GuildHelper.UnmuteMemberAsync(this, Client, userId, options);

    /// <inheritdoc />
    public Task MuteMembersAsync(IEnumerable<IGuildMember> users, TimeSpan duration, RequestOptions? options = null) =>
        GuildHelper.MuteMembersAsync(this, Client, users.Select(x => x.Id), duration, options);

    /// <inheritdoc />
    public Task MuteMembersAsync(IEnumerable<ulong> userIds, TimeSpan duration, RequestOptions? options = null) =>
        GuildHelper.MuteMembersAsync(this, Client, userIds, duration, options);

    /// <inheritdoc />
    public Task MuteMembersAsync(IEnumerable<IGuildMember> users, DateTimeOffset until, RequestOptions? options = null) =>
        GuildHelper.MuteMembersAsync(this, Client, users.Select(x => x.Id), until, options);

    /// <inheritdoc />
    public Task MuteMembersAsync(IEnumerable<ulong> userIds, DateTimeOffset until, RequestOptions? options = null) =>
        GuildHelper.MuteMembersAsync(this, Client, userIds, until, options);

    /// <inheritdoc />
    public Task UnmuteMembersAsync(IEnumerable<IGuildMember> users, RequestOptions? options = null) =>
        GuildHelper.UnmuteMembersAsync(this, Client, users.Select(x => x.Id), options);

    /// <inheritdoc />
    public Task UnmuteMembersAsync(IEnumerable<ulong> userIds, RequestOptions? options = null) =>
        GuildHelper.UnmuteMembersAsync(this, Client, userIds, options);

    #endregion

    #region Announcements

    /// <inheritdoc />
    public Task PublishAnnouncementAsync(ulong channelId, string messageId, RequestOptions? options = null) =>
        GuildHelper.PublishAnnouncementAsync(this, Client, channelId, messageId, options);

    /// <inheritdoc />
    public Task PublishAnnouncementAsync(IUserMessage message, RequestOptions? options = null)
    {
        if (message.Channel is not IGuildChannel channel)
            throw new ArgumentException("The message must be from a guild channel.", nameof(message));
        return GuildHelper.PublishAnnouncementAsync(this, Client, channel.Id, message.Id, options);
    }

    /// <inheritdoc />
    public Task RevokeAnnouncementAsync(string messageId, RequestOptions? options = null) =>
        GuildHelper.RevokeAnnouncementAsync(this, Client, messageId, options);

    /// <inheritdoc />
    public Task RevokeAnnouncementAsync(IUserMessage message, RequestOptions? options = null) =>
        GuildHelper.RevokeAnnouncementAsync(this, Client, message.Id, options);

    /// <inheritdoc />
    public Task RecommendChannelsAsync(IEnumerable<ChannelRecommendation> recommendations, RequestOptions? options = null) =>
        GuildHelper.RecommendChannelsAsync(this, Client, recommendations, options);

    /// <inheritdoc />
    public Task RemoveAllChannelRecommendationsAsync(RequestOptions? options = null) =>
        GuildHelper.RemoveAllChannelRecommendationsAsync(this, Client, options);

    #endregion
}
