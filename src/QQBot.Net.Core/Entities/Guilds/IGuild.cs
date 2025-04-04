﻿namespace QQBot;

/// <summary>
///     表示一个子频道.
/// </summary>
public interface IGuild : IEntity<ulong>
{
    /// <summary>
    ///     获取此子频道的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此子频道创建者用户的 ID。
    /// </summary>
    ulong OwnerId { get; }

    /// <summary>
    ///     获取当前用户是否是此子频道的创建者。
    /// </summary>
    bool IsOwner { get; }

    /// <summary>
    ///     获取此子频道的成员数量。
    /// </summary>
    int MemberCount { get; }

    /// <summary>
    ///     获取可以加入到此频道的最大成员数量。
    /// </summary>
    int MaxMembers { get; }

    /// <summary>
    ///     获取此子频道的描述。
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     获取当前用户加入此子频道的时间。
    /// </summary>
    DateTimeOffset JoinedAt { get; }

    /// <summary>
    ///     获取此频道内可以同时拥有的角色的最大数量。
    /// </summary>
    int MaxRoles { get; }

    /// <summary>
    ///     确定此频道实体是否已准备就绪以供用户代码访问。
    /// </summary>
    /// <remarks>
    ///     <note>
    ///         此属性仅对基于网关连接的客户端有意义。
    ///     </note>
    ///     此属性为 <c>true</c> 表示，此频道实体已完整缓存基础数据，并与网关同步。 <br />
    ///     缓存基础数据包括频道基本信息、子频道、角色、子频道权限重写、当前用户在频道内的昵称。
    /// </remarks>
    bool IsAvailable { get; }

    // /// <summary>
    // ///     获取此频道的所有角色。
    // /// </summary>
    // IReadOnlyCollection<IRole> Roles { get; }

    #region Roles

    // /// <summary>
    // ///     获取此频道中的 <c>@全体成员</c> 全体成员角色。
    // /// </summary>
    // IRole EveryoneRole { get; }

    // /// <summary>
    // ///     获取此频道内的角色。
    // /// </summary>
    // /// <param name="id"> 要获取的角色的 ID。 </param>
    // /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的角色；如果未找到，则返回 <c>null</c>。 </returns>
    // IRole? GetRole(uint id);

    /// <summary>
    ///     获取此频道的所有角色。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有角色。 </returns>
    Task<IReadOnlyCollection<IRole>> GetRolesAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道的角色。
    /// </summary>
    /// <param name="id"> 要获取的角色的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有角色。 </returns>
    Task<IRole?> GetRoleAsync(uint id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     在此服务器内创建一个新的角色。
    /// </summary>
    /// <param name="func"> 一个包含要应用到新创建角色的配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的角色。 </returns>
    Task<IRole> CreateRoleAsync(Action<RoleProperties> func, RequestOptions? options = null);

    #endregion

    #region Users

    /// <summary>
    ///     获取此频道内的所有用户。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的可枚举集合，包含此频道内的所有用户。 </returns>
    IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> GetUsersAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的用户。
    /// </summary>
    /// <remarks>
    ///     此方法获取加入到此频道内的用户。
    ///     <note>
    ///         此方法在网关的实现中可能返回 <c>null</c>，因为在大型频道中，用户列表的缓存可能不完整。
    ///     </note>
    /// </remarks>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的用户；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IGuildMember?> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     下载此频道内的所有用户。
    /// </summary>
    /// <remarks>
    ///     此方法会下载所有加入到此频道内的用户，并缓存它们。
    /// </remarks>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步下载操作的任务。 </returns>
    Task DownloadUsersAsync(RequestOptions? options = null);

    #endregion

    #region Channels

    /// <summary>
    ///     获取此频道的所有子频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有子频道。 </returns>
    Task<IReadOnlyCollection<IGuildChannel>> GetChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IGuildChannel?> GetChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道的所有具有文字聊天能力的子频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有具有文字聊天能力的子频道。 </returns>
    Task<IReadOnlyCollection<ITextChannel>> GetTextChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的具有文字聊天能力的子频道。
    /// </summary>
    /// <param name="id"> 要获取的具有文字聊天能力的子频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的具有文字聊天能力的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<ITextChannel?> GetTextChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道的所有具有语音聊天能力的子频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有具有语音聊天能力的子频道。 </returns>
    Task<IReadOnlyCollection<IVoiceChannel>> GetVoiceChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的具有语音聊天能力的子频道。
    /// </summary>
    /// <param name="id"> 要获取的具有语音聊天能力的子频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的具有语音聊天能力的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IVoiceChannel?> GetVoiceChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道的所有直播子频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有直播子频道。 </returns>
    Task<IReadOnlyCollection<ILiveStreamChannel>> GetLiveStreamChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的直播子频道。
    /// </summary>
    /// <param name="id"> 要获取的直播子频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的直播子频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<ILiveStreamChannel?> GetLiveStreamChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道的所有应用子频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有应用子频道。 </returns>
    Task<IReadOnlyCollection<IApplicationChannel>> GetApplicationChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的应用子频道。
    /// </summary>
    /// <param name="id"> 要获取的应用子频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的应用子频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IApplicationChannel?> GetApplicationChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道的所有论坛子频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有论坛子频道。 </returns>
    Task<IReadOnlyCollection<IForumChannel>> GetForumChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的论坛子频道。
    /// </summary>
    /// <param name="id"> 要获取的论坛子频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的论坛子频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IForumChannel?> GetForumChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道的所有日程子频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有日程子频道。 </returns>
    Task<IReadOnlyCollection<IScheduleChannel>> GetScheduleChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的日程子频道。
    /// </summary>
    /// <param name="id"> 要获取的日程子频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的日程子频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IScheduleChannel?> GetScheduleChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道的所有分组子频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的所有分组子频道。 </returns>
    Task<IReadOnlyCollection<ICategoryChannel>> GetCategoryChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内的分组子频道。
    /// </summary>
    /// <param name="id"> 要获取的分组子频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的分组子频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<ICategoryChannel?> GetCategoryChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     在此服务器内创建一个新的文字子频道。
    /// </summary>
    /// <param name="name"> 频道的名称。 </param>
    /// <param name="func"> 一个包含要应用到新创建频道的配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的文字频道。 </returns>
    Task<ITextChannel> CreateTextChannelAsync(string name, Action<CreateTextChannelProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     在此服务器内创建一个新的语音子频道。
    /// </summary>
    /// <param name="name"> 频道的名称。 </param>
    /// <param name="func"> 一个包含要应用到新创建频道的配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的语音频道。 </returns>
    Task<IVoiceChannel> CreateVoiceChannelAsync(string name, Action<CreateVoiceChannelProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     在此服务器内创建一个新的直播子频道。
    /// </summary>
    /// <param name="name"> 频道的名称。 </param>
    /// <param name="func"> 一个包含要应用到新创建频道的配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的直播频道。 </returns>
    Task<ILiveStreamChannel> CreateLiveStreamChannelAsync(string name, Action<CreateLiveStreamChannelProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     在此服务器内创建一个新的应用子频道。
    /// </summary>
    /// <param name="name"> 频道的名称。 </param>
    /// <param name="func"> 一个包含要应用到新创建频道的配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的应用频道。 </returns>
    Task<IApplicationChannel> CreateApplicationChannelAsync(string name, Action<CreateApplicationChannelProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     在此服务器内创建一个新的论坛子频道。
    /// </summary>
    /// <param name="name"> 频道的名称。 </param>
    /// <param name="func"> 一个包含要应用到新创建频道的配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的论坛频道。 </returns>
    Task<IForumChannel> CreateForumChannelAsync(string name, Action<CreateForumChannelProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     在此服务器内创建一个新的日程子频道。
    /// </summary>
    /// <param name="name"> 频道的名称。 </param>
    /// <param name="func"> 一个包含要应用到新创建频道的配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的日程频道。 </returns>
    Task<IScheduleChannel> CreateScheduleChannelAsync(string name, Action<CreateScheduleChannelProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     在此服务器内创建一个新的分组子频道。
    /// </summary>
    /// <param name="name"> 频道的名称。 </param>
    /// <param name="func"> 一个包含要应用到新创建频道的配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的分组频道。 </returns>
    Task<ICategoryChannel> CreateCategoryChannelAsync(string name, Action<CreateCategoryChannelProperties>? func = null, RequestOptions? options = null);

    #endregion

    #region API Permissions

    /// <summary>
    ///     获取当前用户在此子频道内的可用权限。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 当前用户在此子频道内的可用权限。 </returns>
    Task<IReadOnlyCollection<ApplicationPermission>> GetApplicationPermissionsAsync(RequestOptions? options = null);

    /// <summary>
    ///     请求应用程序权限。
    /// </summary>
    /// <param name="channelId"> 要发送请求的频道的 ID。 </param>
    /// <param name="title"> 接口权限链接中的接口权限描述信息。 </param>
    /// <param name="permission"> 要请求的权限。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RequestApplicationPermissionAsync(ulong channelId,
        string title, ApplicationPermission permission, RequestOptions? options = null);

    /// <summary>
    ///     请求应用程序权限。
    /// </summary>
    /// <param name="channel"> 要发送请求的频道。 </param>
    /// <param name="title"> 接口权限链接中的接口权限描述信息。 </param>
    /// <param name="permission"> 要请求的权限。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RequestApplicationPermissionAsync(ITextChannel channel,
        string title, ApplicationPermission permission, RequestOptions? options = null);

    /// <summary>
    ///     请求应用程序权限。
    /// </summary>
    /// <param name="channelId"> 要发送请求的频道的 ID。 </param>
    /// <param name="title"> 接口权限链接中的接口权限描述信息。 </param>
    /// <param name="description"> 接口权限链接中的机器人可使用功能的描述信息。 </param>
    /// <param name="method"> 要请求的权限的请求方法。 </param>
    /// <param name="path"> 要请求的权限的路径。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RequestApplicationPermissionAsync(ulong channelId,
        string title, string description, HttpMethod method, string path, RequestOptions? options = null);

    /// <summary>
    ///     请求应用程序权限。
    /// </summary>
    /// <param name="channel"> 要发送请求的频道。 </param>
    /// <param name="title"> 接口权限链接中的接口权限描述信息。 </param>
    /// <param name="description"> 接口权限链接中的机器人可使用功能的描述信息。 </param>
    /// <param name="method"> 要请求的权限的请求方法。 </param>
    /// <param name="path"> 要请求的权限的路径。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RequestApplicationPermissionAsync(ITextChannel channel,
        string title, string description, HttpMethod method, string path, RequestOptions? options = null);

    #endregion

    #region Message Settings

    /// <summary>
    ///     获取此频道的消息设置。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道的消息设置。 </returns>
    Task<MessageSetting> GetMessageSettingAsync(RequestOptions? options = null);

    /// <summary>
    ///     禁言全体成员。
    /// </summary>
    /// <param name="duration"> 禁言的时长。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteEveryoneAsync(TimeSpan duration, RequestOptions? options = null);

    /// <summary>
    ///     禁言全体成员。
    /// </summary>
    /// <param name="until"> 禁言的结束时间。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteEveryoneAsync(DateTimeOffset until, RequestOptions? options = null);

    /// <summary>
    ///     禁言全体成员。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task UnmuteEveryoneAsync(RequestOptions? options = null);

    /// <summary>
    ///     禁言指定用户。
    /// </summary>
    /// <param name="user"> 要禁言的用户。 </param>
    /// <param name="duration"> 禁言的时长。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteMemberAsync(IGuildMember user, TimeSpan duration, RequestOptions? options = null);

    /// <summary>
    ///     禁言指定用户。
    /// </summary>
    /// <param name="userId"> 要禁言的用户的 ID。 </param>
    /// <param name="duration"> 禁言的时长。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteMemberAsync(ulong userId, TimeSpan duration, RequestOptions? options = null);

    /// <summary>
    ///     禁言指定用户。
    /// </summary>
    /// <param name="user"> 要禁言的用户。 </param>
    /// <param name="until"> 禁言的结束时间。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteMemberAsync(IGuildMember user, DateTimeOffset until, RequestOptions? options = null);

    /// <summary>
    ///     禁言指定用户。
    /// </summary>
    /// <param name="userId"> 要禁言的用户的 ID。 </param>
    /// <param name="until"> 禁言的结束时间。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteMemberAsync(ulong userId, DateTimeOffset until, RequestOptions? options = null);

    /// <summary>
    ///     取消禁言用户。
    /// </summary>
    /// <param name="user"> 要取消禁言的用户。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task UnmuteMemberAsync(IGuildMember user, RequestOptions? options = null);

    /// <summary>
    ///     取消禁言用户。
    /// </summary>
    /// <param name="userId"> 要取消禁言的用户的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task UnmuteMemberAsync(ulong userId, RequestOptions? options = null);

    /// <summary>
    ///     批量禁言用户。
    /// </summary>
    /// <param name="users"> 所有要禁言的用户。 </param>
    /// <param name="duration"> 禁言的时长。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteMembersAsync(IEnumerable<IGuildMember> users, TimeSpan duration, RequestOptions? options = null);

    /// <summary>
    ///     批量禁言用户。
    /// </summary>
    /// <param name="userIds"> 所有要禁言的用户的 ID。 </param>
    /// <param name="duration"> 禁言的时长。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteMembersAsync(IEnumerable<ulong> userIds, TimeSpan duration, RequestOptions? options = null);

    /// <summary>
    ///     批量禁言用户。
    /// </summary>
    /// <param name="users"> 所有要禁言的用户。 </param>
    /// <param name="until"> 禁言的结束时间。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteMembersAsync(IEnumerable<IGuildMember> users, DateTimeOffset until, RequestOptions? options = null);

    /// <summary>
    ///     批量禁言用户。
    /// </summary>
    /// <param name="userIds"> 所有要禁言的用户的 ID。 </param>
    /// <param name="until"> 禁言的结束时间。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task MuteMembersAsync(IEnumerable<ulong> userIds, DateTimeOffset until, RequestOptions? options = null);

    /// <summary>
    ///     批量取消禁言用户。
    /// </summary>
    /// <param name="users"> 所有要取消禁言的用户。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task UnmuteMembersAsync(IEnumerable<IGuildMember> users, RequestOptions? options = null);

    /// <summary>
    ///     批量取消禁言用户。
    /// </summary>
    /// <param name="userIds"> 所有要取消禁言的用户的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task UnmuteMembersAsync(IEnumerable<ulong> userIds, RequestOptions? options = null);

    #endregion

    #region Announcements

    /// <summary>
    ///     发布消息类型的频道公告。
    /// </summary>
    /// <param name="channelId"> 要发布为公告的消息所在的频道的 ID。 </param>
    /// <param name="messageId"> 要发布为公告的消息的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task PublishAnnouncementAsync(ulong channelId, string messageId, RequestOptions? options = null);

    /// <summary>
    ///     发布消息类型的频道公告。
    /// </summary>
    /// <param name="message"> 要发布为公告的消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task PublishAnnouncementAsync(IUserMessage message, RequestOptions? options = null);

    /// <summary>
    ///     撤销频道公告。
    /// </summary>
    /// <param name="messageId"> 要撤销的公告的消息的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RevokeAnnouncementAsync(string messageId, RequestOptions? options = null);

    /// <summary>
    ///     撤销频道公告。
    /// </summary>
    /// <param name="message"> 要撤销的公告的消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RevokeAnnouncementAsync(IUserMessage message, RequestOptions? options = null);

    /// <summary>
    ///     发布子频道推荐类型的公告。
    /// </summary>
    /// <param name="recommendations"> 所有要推荐的子频道及推荐于。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    /// <remarks>
    ///     同频道内最多同时推荐 3 个子频道。
    /// </remarks>
    Task RecommendChannelsAsync(IEnumerable<ChannelRecommendation> recommendations, RequestOptions? options = null);

    /// <summary>
    ///     移除所有子频道推荐。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RemoveAllChannelRecommendationsAsync(RequestOptions? options = null);

    #endregion
}
