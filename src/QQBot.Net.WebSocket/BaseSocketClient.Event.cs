namespace QQBot.WebSocket;

public abstract partial class BaseSocketClient
{
    #region Guilds

    /// <summary>
    ///     当当前用户新加入频道时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuild"/> 参数是当前用户新加入的频道。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是将当前 Bot 添加到频道的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuild, Cacheable<SocketGuildMember, ulong>, Task> JoinedGuild
    {
        add => _joinedGuildEvent.Add(value);
        remove => _joinedGuildEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuild, Cacheable<SocketGuildMember, ulong>, Task>> _joinedGuildEvent = new();

    /// <summary>
    ///     当当前用户离开频道或频道被解散时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuild"/> 参数是当前用户离开或被解散的频道。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是将当前 Bot 从频道中移除、或解散该频道的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuild, Cacheable<SocketGuildMember, ulong>, Task> LeftGuild
    {
        add => _leftGuildEvent.Add(value);
        remove => _leftGuildEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuild, Cacheable<SocketGuildMember, ulong>, Task>> _leftGuildEvent = new();

    /// <summary>
    ///     当频道信息被更新时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGuild"/> 参数是频道信息被更新前的状态。 </item>
    ///     <item> <see cref="QQBot.WebSocket.SocketGuild"/> 参数是频道信息被更新后的状态。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是更新该频道信息的用户。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketGuildMember"/> 频道用户；否则，包含 <see cref="System.UInt64"/> 用户 ID，以供按需下载实体。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGuild, SocketGuild, Cacheable<SocketGuildMember, ulong>, Task> GuildUpdated
    {
        add => _guildUpdatedEvent.Add(value);
        remove => _guildUpdatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGuild, SocketGuild, Cacheable<SocketGuildMember, ulong>, Task>> _guildUpdatedEvent = new();

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

    /// <summary>
    ///     当消息被撤回时引发。
    /// </summary>
    /// <remarks>
    ///     此事件涵盖文字子频道消息（私域需 <see cref="QQBot.GatewayIntents.GuildMessages"/>、公域需
    ///     <see cref="QQBot.GatewayIntents.PublicGuildMessages"/>）与频道私信（<see cref="QQBot.GatewayIntents.DirectMessages"/>）的撤回。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是被撤回的消息。如果缓存中存在此消息实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketMessage"/> 消息；否则，包含 <see cref="System.String"/> 消息 ID。
    ///         <note type="important">
    ///             被撤回的消息无法通过 <see cref="QQBot.Cacheable{TEntity,TId}.DownloadAsync"/> 方法下载。
    ///         </note>
    ///     </item>
    ///     <item> <see cref="QQBot.WebSocket.ISocketMessageChannel"/> 参数是消息被撤回的子频道或私信频道。 </item>
    ///     <item> <see cref="System.UInt64"/> 参数是执行撤回操作的用户的 ID。 </item>
    ///     </list>
    /// </remarks>
    public event Func<Cacheable<IMessage, string>, ISocketMessageChannel, ulong, Task> MessageDeleted
    {
        add => _messageDeletedEvent.Add(value);
        remove => _messageDeletedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Cacheable<IMessage, string>, ISocketMessageChannel, ulong, Task>> _messageDeletedEvent = new();

    /// <summary>
    ///     当消息审核完成时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.MessageAudit"/> 网关意图。 <br />
    ///     机器人主动发送的频道消息经过审核后，无论通过或不通过均会引发此事件；可通过
    ///     <see cref="QQBot.WebSocket.SocketMessageAudit.Result"/> 区分。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketMessageAudit"/> 参数是本次消息审核的结果。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketMessageAudit, Task> MessageAudited
    {
        add => _messageAuditedEvent.Add(value);
        remove => _messageAuditedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketMessageAudit, Task>> _messageAuditedEvent = new();

    #endregion

    #region Interactions

    /// <summary>
    ///     当用户触发互动时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.Interaction"/> 网关意图。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketInteraction"/> 参数是网关接收到的互动事件。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketInteraction, Task> InteractionCreated
    {
        add => _interactionCreatedEvent.Add(value);
        remove => _interactionCreatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketInteraction, Task>> _interactionCreatedEvent = new();

    /// <summary>
    ///     当用户点击消息按钮时引发。
    /// </summary>
    /// <remarks>
    ///     此事件在 <see cref="QQBot.WebSocket.BaseSocketClient.InteractionCreated"/> 之后引发。
    /// </remarks>
    public event Func<SocketInteraction, Task> ButtonExecuted
    {
        add => _buttonExecutedEvent.Add(value);
        remove => _buttonExecutedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketInteraction, Task>> _buttonExecutedEvent = new();

    #endregion

    #region Reactions

    /// <summary>
    ///     当用户对目标对象添加表情表态时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.GuildMessageReactions"/> 网关意图。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketReaction"/> 参数是本次添加的表情表态。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketReaction, Task> ReactionAdded
    {
        add => _reactionAddedEvent.Add(value);
        remove => _reactionAddedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketReaction, Task>> _reactionAddedEvent = new();

    /// <summary>
    ///     当用户对目标对象移除表情表态时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.GuildMessageReactions"/> 网关意图。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketReaction"/> 参数是本次移除的表情表态。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketReaction, Task> ReactionRemoved
    {
        add => _reactionRemovedEvent.Add(value);
        remove => _reactionRemovedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketReaction, Task>> _reactionRemovedEvent = new();

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

    /// <summary>
    ///     当论坛发表内容的审核结果送达时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.ForumsEvent"/> 网关意图，且仅私域机器人可订阅。 <br />
    ///     用户在论坛发表主题、评论或回复后，内容经过审核，审核完成时引发此事件。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketForumAuditResult"/> 参数是本次发表内容的审核结果。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketForumAuditResult, Task> ForumPublishAudited
    {
        add => _forumPublishAuditedEvent.Add(value);
        remove => _forumPublishAuditedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketForumAuditResult, Task>> _forumPublishAuditedEvent = new();

    #endregion

    #region Open Forums

    /// <summary>
    ///     当公域论坛主题被创建时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.OpenForumsEvent"/> 网关意图。 <br />
    ///     开放论坛事件仅携带频道与操作者信息，不包含主题内容详情。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketOpenForumEvent"/> 参数是本次开放论坛主题创建事件。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketOpenForumEvent, Task> OpenForumThreadCreated
    {
        add => _openForumThreadCreatedEvent.Add(value);
        remove => _openForumThreadCreatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketOpenForumEvent, Task>> _openForumThreadCreatedEvent = new();

    /// <summary>
    ///     当公域论坛主题被更新时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.OpenForumsEvent"/> 网关意图。 <br />
    ///     开放论坛事件仅携带频道与操作者信息，不包含主题内容详情。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketOpenForumEvent"/> 参数是本次开放论坛主题更新事件。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketOpenForumEvent, Task> OpenForumThreadUpdated
    {
        add => _openForumThreadUpdatedEvent.Add(value);
        remove => _openForumThreadUpdatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketOpenForumEvent, Task>> _openForumThreadUpdatedEvent = new();

    /// <summary>
    ///     当公域论坛主题被删除时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.OpenForumsEvent"/> 网关意图。 <br />
    ///     开放论坛事件仅携带频道与操作者信息，不包含主题内容详情。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketOpenForumEvent"/> 参数是本次开放论坛主题删除事件。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketOpenForumEvent, Task> OpenForumThreadDeleted
    {
        add => _openForumThreadDeletedEvent.Add(value);
        remove => _openForumThreadDeletedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketOpenForumEvent, Task>> _openForumThreadDeletedEvent = new();

    /// <summary>
    ///     当公域论坛主题评论被创建时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.OpenForumsEvent"/> 网关意图。 <br />
    ///     开放论坛事件仅携带频道与操作者信息，不包含评论内容详情。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketOpenForumEvent"/> 参数是本次开放论坛评论创建事件。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketOpenForumEvent, Task> OpenForumPostCreated
    {
        add => _openForumPostCreatedEvent.Add(value);
        remove => _openForumPostCreatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketOpenForumEvent, Task>> _openForumPostCreatedEvent = new();

    /// <summary>
    ///     当公域论坛主题评论被删除时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.OpenForumsEvent"/> 网关意图。 <br />
    ///     开放论坛事件仅携带频道与操作者信息，不包含评论内容详情。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketOpenForumEvent"/> 参数是本次开放论坛评论删除事件。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketOpenForumEvent, Task> OpenForumPostDeleted
    {
        add => _openForumPostDeletedEvent.Add(value);
        remove => _openForumPostDeletedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketOpenForumEvent, Task>> _openForumPostDeletedEvent = new();

    /// <summary>
    ///     当公域论坛主题评论回复被创建时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.OpenForumsEvent"/> 网关意图。 <br />
    ///     开放论坛事件仅携带频道与操作者信息，不包含回复内容详情。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketOpenForumEvent"/> 参数是本次开放论坛回复创建事件。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketOpenForumEvent, Task> OpenForumReplyCreated
    {
        add => _openForumReplyCreatedEvent.Add(value);
        remove => _openForumReplyCreatedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketOpenForumEvent, Task>> _openForumReplyCreatedEvent = new();

    /// <summary>
    ///     当公域论坛主题评论回复被删除时引发。
    /// </summary>
    /// <remarks>
    ///     此事件需要订阅 <see cref="QQBot.GatewayIntents.OpenForumsEvent"/> 网关意图。 <br />
    ///     开放论坛事件仅携带频道与操作者信息，不包含回复内容详情。 <br />
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketOpenForumEvent"/> 参数是本次开放论坛回复删除事件。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketOpenForumEvent, Task> OpenForumReplyDeleted
    {
        add => _openForumReplyDeletedEvent.Add(value);
        remove => _openForumReplyDeletedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketOpenForumEvent, Task>> _openForumReplyDeletedEvent = new();

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
    ///     当群成员加入群组时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGroupChannel"/> 参数是成员加入的群组。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是加入群组的成员。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 群组用户；否则，包含 <see cref="System.String"/> 用户 ID。
    ///         如果网关没有提供实体的详细信息，由于目前无法通过 API 获取此用户实体，因此
    ///         <see cref="QQBot.Cacheable{TEntity,TId}.GetOrDownloadAsync"/> 总会返回 <c>null</c>。
    ///     </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是执行该操作的群成员。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 群组用户；否则，包含 <see cref="System.String"/> 用户 ID。
    ///         如果网关没有提供执行该操作的群成员，此参数为 <c>null</c>。
    ///         如果网关没有提供实体的详细信息，由于目前无法通过 API 获取此用户实体，因此
    ///         <see cref="QQBot.Cacheable{TEntity,TId}.GetOrDownloadAsync"/> 总会返回 <c>null</c>。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGroupChannel, Cacheable<SocketUser, string>, Cacheable<SocketUser, string>?, Task> GroupMemberJoined
    {
        add => _groupMemberJoinedEvent.Add(value);
        remove => _groupMemberJoinedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGroupChannel, Cacheable<SocketUser, string>, Cacheable<SocketUser, string>?, Task>> _groupMemberJoinedEvent = new();

    /// <summary>
    ///     当群成员离开群组时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketGroupChannel"/> 参数是成员离开的群组。 </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是离开群组的成员。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 群组用户；否则，包含 <see cref="System.String"/> 用户 ID。
    ///         如果网关没有提供实体的详细信息，由于目前无法通过 API 获取此用户实体，因此
    ///         <see cref="QQBot.Cacheable{TEntity,TId}.GetOrDownloadAsync"/> 总会返回 <c>null</c>。
    ///     </item>
    ///     <item>
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是执行该操作的群成员。如果缓存中存在此用户实体，那么该结构内包含该
    ///         <see cref="QQBot.WebSocket.SocketUser"/> 群组用户；否则，包含 <see cref="System.String"/> 用户 ID。
    ///         如果网关没有提供执行该操作的群成员，此参数为 <c>null</c>。
    ///         如果网关没有提供实体的详细信息，由于目前无法通过 API 获取此用户实体，因此
    ///         <see cref="QQBot.Cacheable{TEntity,TId}.GetOrDownloadAsync"/> 总会返回 <c>null</c>。
    ///     </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketGroupChannel, Cacheable<SocketUser, string>, Cacheable<SocketUser, string>?, Task> GroupMemberLeft
    {
        add => _groupMemberLeftEvent.Add(value);
        remove => _groupMemberLeftEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketGroupChannel, Cacheable<SocketUser, string>, Cacheable<SocketUser, string>?, Task>> _groupMemberLeftEvent = new();

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
    ///     <item> <see cref="string"/> 参数是开发者自定义的回调数据，参见 <see cref="QQBot.IQQBotClient.GenerateProfileUrlAsync(System.String,QQBot.RequestOptions)"/>。 </item>
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
