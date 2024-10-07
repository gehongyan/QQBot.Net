namespace QQBot;

/// <summary>
///     表示网关意图
/// </summary>
[Flags]
public enum GatewayIntents
{
    /// <summary>
    ///     此网关意图不包括任何事件
    /// </summary>
    None = 0,

    /// <summary>
    ///     此网关意图包括 <c>GUILD_CREATE</c>, <c>GUILD_UPDATE</c>, <c>GUILD_DELETE</c>, <c>CHANNEL_CREATE</c>,
    ///     <c>CHANNEL_UPDATE</c>, <c>CHANNEL_DELETE</c>
    /// </summary>
    Guilds = 1 << 0,

    /// <summary>
    ///     此网关意图包括 <c>GUILD_MEMBER_ADD</c>, <c>GUILD_MEMBER_UPDATE</c>, <c>GUILD_MEMBER_REMOVE</c>
    /// </summary>
    GuildMembers = 1 << 1,

    /// <summary>
    ///     此网关意图包括 <c>MESSAGE_CREATE</c>, <c>MESSAGE_DELETE</c>
    /// </summary>
    GuildMessages = 1 << 9,

    /// <summary>
    ///     此网关意图包括 <c>MESSAGE_REACTION_ADD</c>, <c>MESSAGE_REACTION_REMOVE</c>
    /// </summary>
    GuildMessageReactions = 1 << 10,

    /// <summary>
    ///     此网关意图包括 <c>DIRECT_MESSAGE_CREATE</c>, <c>DIRECT_MESSAGE_DELETE</c>
    /// </summary>
    DirectMessages = 1 << 12,

    /// <summary>
    ///     此网关意图包括 <c>OPEN_FORUM_THREAD_CREATE</c>, <c>OPEN_FORUM_THREAD_UPDATE</c>,
    ///     <c>OPEN_FORUM_THREAD_DELETE</c>, <c>OPEN_FORUM_POST_CREATE</c>, <c>OPEN_FORUM_POST_DELETE</c>,
    ///     <c>OPEN_FORUM_REPLY_CREATE</c>, <c>OPEN_FORUM_REPLY_DELETE</c>
    /// </summary>
    OpenForumsEvent = 1 << 18,

    /// <summary>
    ///     此网关意图包括 <c>AUDIO_OR_LIVE_CHANNEL_MEMBER_ENTER</c>, <c>AUDIO_OR_LIVE_CHANNEL_MEMBER_EXIT</c>
    /// </summary>
    AudioOrLiveChannelMember = 1 << 19,

    /// <summary>
    ///     此网关意图包括 <c>C2C_MESSAGE_CREATE</c>, <c>FRIEND_ADD</c>, <c>FRIEND_DEL</c>, <c>C2C_MSG_REJECT</c>,
    ///     <c>C2C_MSG_RECEIVE</c>, <c>GROUP_AT_MESSAGE_CREATE</c>, <c>GROUP_ADD_ROBOT</c>, <c>GROUP_DEL_ROBOT</c>,
    ///     <c>GROUP_MSG_REJECT</c>, <c>GROUP_MSG_RECEIVE</c>
    /// </summary>
    GroupAndC2CEvent = 1 << 25,

    /// <summary>
    ///     此网关意图包括 <c>INTERACTION_CREATE</c>
    /// </summary>
    Interaction = 1 << 26,

    /// <summary>
    ///     此网关意图包括 <c>MESSAGE_AUDIT_PASS</c>, <c>MESSAGE_AUDIT_REJECT</c>
    /// </summary>
    MessageAudit = 1 << 27,

    /// <summary>
    ///     此网关意图包括 <c>FORUM_THREAD_CREATE</c>, <c>FORUM_THREAD_UPDATE</c>, <c>FORUM_THREAD_DELETE</c>,
    ///     <c>FORUM_POST_CREATE</c>, <c>FORUM_POST_DELETE</c>, <c>FORUM_REPLY_CREATE</c>, <c>FORUM_REPLY_DELETE</c>,
    ///     <c>FORUM_PUBLISH_AUDIT_RESULT</c>
    /// </summary>
    ForumsEvent = 1 << 28,

    /// <summary>
    ///     此网关意图包括 <c>AUDIO_START</c>, <c>AUDIO_FINISH</c>, <c>AUDIO_ON_MIC</c>, <c>AUDIO_OFF_MIC</c>
    /// </summary>
    AudioAction = 1 << 29,

    /// <summary>
    ///     此网关意图包括 <c>AT_MESSAGE_CREATE</c>, <c>PUBLIC_MESSAGE_DELETE</c>
    /// </summary>
    PublicGuildMessages = 1 << 30,

    /// <summary>
    ///     此网关意图包括所有公域机器人可以订阅的事件
    /// </summary>
    AllPublicDomain = Guilds
        | GuildMembers
        | GuildMessageReactions
        | DirectMessages
        | OpenForumsEvent
        | AudioOrLiveChannelMember
        | GroupAndC2CEvent
        | Interaction
        | MessageAudit
        | AudioAction
        | PublicGuildMessages,

    /// <summary>
    ///     此网关意图包括所有事件
    /// </summary>
    All = AllPublicDomain | GuildMessages | ForumsEvent
}
