namespace QQBot.WebSocket;

public abstract partial class BaseSocketClient
{
    #region Guilds

    /// <summary>
    ///     当频道状态变更为可用时引发。
    /// </summary>
    /// <remarks>
    ///     频道状态变更为可用，表示此频道实体已完整缓存基础数据，并与网关同步。 <br />
    ///     缓存基础数据包括频道基本信息、子频道、角色、子频道权限重写、当前用户在频道内的昵称。
    ///     <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuild"/> 参数是频道状态变更为可用的频道。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuild, Task> GuildAvailable
    {
        add => _guildAvailableEvent.Add(value);
        remove => _guildAvailableEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuild, Task>> _guildAvailableEvent = new();

    /// <summary>
    ///     当频道状态变更为不可用时引发。
    /// </summary>
    /// <remarks>
    ///     频道状态变更为不可用，表示此频道实体丢失与网关的同步，所缓存的数据不可靠，这通常发生在频道被删除、当前用户离开频道、网关连接断开等情况。
    ///     <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuild"/> 参数是频道状态变更为不可用的频道。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuild, Task> GuildUnavailable
    {
        add => _guildUnavailableEvent.Add(value);
        remove => _guildUnavailableEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuild, Task>> _guildUnavailableEvent = new();

    #endregion

    #region Channels

    /// <summary>
    ///     当子频道被创建时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildChannel"/> 参数是新创建的子频道。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuildChannel, Task> ChannelCreated
    {
        add => _channelCreatedEvent.Add(value);
        remove => _channelCreatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuildChannel, Task>> _channelCreatedEvent = new();

    /// <summary>
    ///     当子频道被删除时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildChannel"/> 参数是被删除的子频道。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是删除此子频道的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuildChannel, Cacheable<SocketGuildMember, ulong>, Task> ChannelDestroyed
    {
        add => _channelDestroyedEvent.Add(value);
        remove => _channelDestroyedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuildChannel, Cacheable<SocketGuildMember, ulong>, Task>> _channelDestroyedEvent = new();

    /// <summary>
    ///     当子频道信息被更新时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildChannel"/> 参数是更新前的子频道。 </item>
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildChannel"/> 参数是更新后的子频道。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是更新此子频道的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuildChannel, SocketGuildChannel, Cacheable<SocketGuildMember, ulong>, Task> ChannelUpdated
    {
        add => _channelUpdatedEvent.Add(value);
        remove => _channelUpdatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuildChannel, SocketGuildChannel, Cacheable<SocketGuildMember, ulong>, Task>> _channelUpdatedEvent = new();

    #endregion

    #region Guild Members

    /// <summary>
    ///     当用户加入频道时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildMember"/> 参数是加入频道的频道用户。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是进行此操作的子频道的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuildMember, Cacheable<SocketGuildMember, ulong>, Task> UserJoined
    {
        add => _userJoinedEvent.Add(value);
        remove => _userJoinedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuildMember, Cacheable<SocketGuildMember, ulong>, Task>> _userJoinedEvent = new();

    /// <summary>
    ///     当用户离开频道时引发。
    /// </summary>
    /// <remarks>
    ///     <note type="warning">
    ///         有消息称，那么此事件不会在其成员数量超过 2000 人的频道内被触发。
    ///     </note>
    ///     <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuild"/> 参数是用户离开的频道。 </item>
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildUser"/> 参数是离开频道的频道用户。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是进行此操作的子频道的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuild, SocketGuildUser, Cacheable<SocketGuildMember, ulong>, Task> UserLeft
    {
        add => _userLeftEvent.Add(value);
        remove => _userLeftEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuild, SocketGuildUser, Cacheable<SocketGuildMember, ulong>, Task>> _userLeftEvent = new();

    /// <summary>
    ///     当频道用户信息被更新时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是可缓存用户被更新前的状态。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 用户被更新前的状态；否则，包含 <see cref="System.UInt64"/> 用户 ID。
    ///         <br />
    ///         <note type="important">
    ///             用户被更新前的状态无法通过 <see cref="QQBot.Cacheable{TEntity,TId}.DownloadAsync"/> 方法下载。
    ///         </note>
    ///     </item>
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildUser"/> 参数是更新后的频道用户。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是进行此操作的子频道的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<Cacheable<SocketGuildMember, ulong>, SocketGuildMember, Cacheable<SocketGuildMember, ulong>, Task> GuildMemberUpdated
    {
        add => _guildMemberUpdatedEvent.Add(value);
        remove => _guildMemberUpdatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Cacheable<SocketGuildMember, ulong>, SocketGuildMember, Cacheable<SocketGuildMember, ulong>, Task>> _guildMemberUpdatedEvent = new();

    #endregion

    #region Messages

    /// <summary>
    ///     当接收到新消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketMessage"/> 参数是新接收到的消息。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketUserMessage, Task> MessageReceived
    {
        add => _messageReceivedEvent.Add(value);
        remove => _messageReceivedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketUserMessage, Task>> _messageReceivedEvent = new();

    #endregion

    #region Voices

    /// <summary>
    ///     当频道用户连接到语音子频道或直播子频道时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是连接到语音子频道或直播子频道的可缓存频道用户。如果缓存中存在此频道用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildChannel"/> 参数是用户连接到的语音子频道或直播子频道。 </item>
    ///     </list>
    /// </remarks>
    public event Func<Cacheable<SocketGuildMember, ulong>, SocketGuildChannel, Task> UserConnected
    {
        add => _userConnectedEvent.Add(value);
        remove => _userConnectedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Cacheable<SocketGuildMember, ulong>, SocketGuildChannel, Task>> _userConnectedEvent = new();

    /// <summary>
    ///     当频道用户从语音子频道或直播子频道断开连接时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是从语音子频道或直播子频道断开连接的可缓存频道用户。如果缓存中存在此频道用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     <item> <see cref="QQBot.WebSocket.SocketGuildChannel"/> 参数是用户断开连接的语音子频道或直播子频道。 </item>
    ///     </list>
    /// </remarks>
    public event Func<Cacheable<SocketGuildMember, ulong>, SocketGuildChannel, Task> UserDisconnected
    {
        add => _userDisconnectedEvent.Add(value);
        remove => _userDisconnectedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Cacheable<SocketGuildMember, ulong>, SocketGuildChannel, Task>> _userDisconnectedEvent = new();

    #endregion

    #region Forums

    /// <summary>
    ///     当论坛主题被创建时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketThread"/> 参数是新创建的论坛主题。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketThread, Task> ForumThreadCreated
    {
        add => _forumThreadCreatedEvent.Add(value);
        remove => _forumThreadCreatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketThread, Task>> _forumThreadCreatedEvent = new();

    /// <summary>
    ///     当论坛主题被修改时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketThread"/> 参数是修改后的论坛主题。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketThread, Task> ForumThreadUpdated
    {
        add => _forumThreadUpdatedEvent.Add(value);
        remove => _forumThreadUpdatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketThread, Task>> _forumThreadUpdatedEvent = new();

    /// <summary>
    ///     当论坛主题被删除时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketThread"/> 参数是被删除的论坛主题。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketThread, Task> ForumThreadDeleted
    {
        add => _forumThreadDeletedEvent.Add(value);
        remove => _forumThreadDeletedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketThread, Task>> _forumThreadDeletedEvent = new();

    /// <summary>
    ///     当论坛主题评论被创建时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketPost"/> 参数是新创建的论坛主题评论。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketPost, Task> ForumPostCreated
    {
        add => _forumPostCreatedEvent.Add(value);
        remove => _forumPostCreatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketPost, Task>> _forumPostCreatedEvent = new();

    /// <summary>
    ///     当论坛主题评论被删除时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketPost"/> 参数是被删除的论坛主题评论。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketPost, Task> ForumPostDeleted
    {
        add => _forumPostDeletedEvent.Add(value);
        remove => _forumPostDeletedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketPost, Task>> _forumPostDeletedEvent = new();

    /// <summary>
    ///     当论坛主题评论回复被创建时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketReply"/> 参数是新创建的论坛主题评论回复。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketReply, Task> ForumReplyCreated
    {
        add => _forumReplyCreatedEvent.Add(value);
        remove => _forumReplyCreatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketReply, Task>> _forumReplyCreatedEvent = new();

    /// <summary>
    ///     当论坛主题评论回复被删除时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketReply"/> 参数是被删除的论坛主题评论回复。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketReply, Task> ForumReplyDeleted
    {
        add => _forumReplyDeletedEvent.Add(value);
        remove => _forumReplyDeletedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketReply, Task>> _forumReplyDeletedEvent = new();

    #endregion

    #region Groups

    /// <summary>
    ///     当群组添加当前用户时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGroupChannel"/> 参数是添加当前用户的群组。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是添加当前用户的群组的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 群组用户；否则，包含 <see cref="System.String"/> 用户 ID。
    ///         如果网关没有提供实体的详细信息，由于目前无法通过 API 获取此用户实体，因此
    ///         <see cref="QQBot.Cacheable{TEntity,TId}.GetOrDownloadAsync"/> 总会返回 <c>null</c>。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGroupChannel, Cacheable<SocketUser, string>, Task> JoinedGroup
    {
        add => _joinedGroupEvent.Add(value);
        remove => _joinedGroupEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGroupChannel, Cacheable<SocketUser, string>, Task>> _joinedGroupEvent = new();

    /// <summary>
    ///     当群组移除当前用户时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGroupChannel"/> 参数是移除当前用户的群组。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是添加当前用户的群组的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 群组用户；否则，包含 <see cref="System.String"/> 用户 ID。
    ///         如果网关没有提供实体的详细信息，由于目前无法通过 API 获取此用户实体，因此
    ///         <see cref="QQBot.Cacheable{TEntity,TId}.GetOrDownloadAsync"/> 总会返回 <c>null</c>。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGroupChannel, Cacheable<SocketUser, string>, Task> LeftGroup
    {
        add => _leftGroupEvent.Add(value);
        remove => _leftGroupEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGroupChannel, Cacheable<SocketUser, string>, Task>> _leftGroupEvent = new();

    /// <summary>
    ///     当群组接受当前用户的主动消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGroupChannel"/> 参数是接受当前用户主动消息的群组。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是添加当前用户的群组的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 群组用户；否则，包含 <see cref="System.String"/> 用户 ID。
    ///         如果网关没有提供实体的详细信息，由于目前无法通过 API 获取此用户实体，因此
    ///         <see cref="QQBot.Cacheable{TEntity,TId}.GetOrDownloadAsync"/> 总会返回 <c>null</c>。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGroupChannel, Cacheable<SocketUser, string>, Task> GroupActiveMessageAllowed
    {
        add => _groupActiveMessageAllowedEvent.Add(value);
        remove => _groupActiveMessageAllowedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGroupChannel, Cacheable<SocketUser, string>, Task>> _groupActiveMessageAllowedEvent = new();

    /// <summary>
    ///     当群组接受当前用户的主动消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGroupChannel"/> 参数是接受当前用户主动消息的群组。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是添加当前用户的群组的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 群组用户；否则，包含 <see cref="System.String"/> 用户 ID。
    ///         如果网关没有提供实体的详细信息，由于目前无法通过 API 获取此用户实体，因此
    ///         <see cref="QQBot.Cacheable{TEntity,TId}.GetOrDownloadAsync"/> 总会返回 <c>null</c>。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGroupChannel, Cacheable<SocketUser, string>, Task> GroupActiveMessageRejected
    {
        add => _groupActiveMessageRejectedEvent.Add(value);
        remove => _groupActiveMessageRejectedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGroupChannel, Cacheable<SocketUser, string>, Task>> _groupActiveMessageRejectedEvent = new();

    #endregion

    #region Users

    /// <summary>
    ///     当用户添加当前用户时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketUserChannel"/> 参数是添加当前用户的用户频道。 </item>
    ///     <item> <see cref="QQBot.UserChannelSource"/> 参数是添加当前用户的来源场景。 </item>
    ///     <item> <see cref="string"/> 参数是开发者自定义的回调数据，参见 <see cref="QQBot.IQQBotClient.GenerateProfileUrl(System.String,QQBot.RequestOptions)"/>。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketUserChannel, UserChannelSource, string?, Task> UserAdded
    {
        add => _userAddedEvent.Add(value);
        remove => _userAddedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketUserChannel, UserChannelSource, string?, Task>> _userAddedEvent = new();

    /// <summary>
    ///     当用户移除当前用户时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketUserChannel"/> 参数是移除当前用户的用户频道。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketUserChannel, Task> UserRemoved
    {
        add => _userRemovedEvent.Add(value);
        remove => _userRemovedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketUserChannel, Task>> _userRemovedEvent = new();

    /// <summary>
    ///     当用户接受当前用户的主动消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketUserChannel"/> 参数是接受当前用户主动消息的用户。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketUserChannel, Task> UserActiveMessageAllowed
    {
        add => _userActiveMessageAllowedEvent.Add(value);
        remove => _userActiveMessageAllowedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketUserChannel, Task>> _userActiveMessageAllowedEvent = new();

    /// <summary>
    ///     当用户接受当前用户的主动消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketUserChannel"/> 参数是接受当前用户主动消息的用户。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketUserChannel, Task> UserActiveMessageRejected
    {
        add => _userActiveMessageRejectedEvent.Add(value);
        remove => _userActiveMessageRejectedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketUserChannel, Task>> _userActiveMessageRejectedEvent = new();

    #endregion
}
