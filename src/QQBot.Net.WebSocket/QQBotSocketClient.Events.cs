namespace QQBot.WebSocket;

public partial class QQBotSocketClient
{
    #region General

    /// <summary>
    ///     当连接到 QQ Bot 网关时引发。
    /// </summary>
    public event Func<Task> Connected
    {
        add => _connectedEvent.Add(value);
        remove => _connectedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Task>> _connectedEvent = new();

    /// <summary>
    ///     当与 QQ Bot 网关断开连接时引发。
    /// </summary>
    public event Func<Exception, Task> Disconnected
    {
        add => _disconnectedEvent.Add(value);
        remove => _disconnectedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Exception, Task>> _disconnectedEvent = new();

    /// <summary>
    ///     当此 Bot 准备就绪以供用户代码访问时引发。
    /// </summary>
    /// <remarks>
    ///     此事件引发的时机可由 <see cref="QQBot.WebSocket.QQBotSocketConfig.StartupCacheFetchMode"/> 配置指定。
    /// </remarks>
    public event Func<Task> Ready
    {
        add => _readyEvent.Add(value);
        remove => _readyEvent.Remove(value);
    }

    private readonly AsyncEvent<Func<Task>> _readyEvent = new();

    /// <summary>
    ///     当网关延迟已更新时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="System.Int32"/> 参数是更新前的延迟（毫秒）。 </item>
    ///     <item> <see cref="System.Int32"/> 参数是更新后的延迟（毫秒）。 </item>
    ///     </list>
    /// </remarks>
    public event Func<int, int, Task> LatencyUpdated
    {
        add => _latencyUpdatedEvent.Add(value);
        remove => _latencyUpdatedEvent.Remove(value);
    }

    private readonly AsyncEvent<Func<int, int, Task>> _latencyUpdatedEvent = new();

    #endregion

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
    ///         <see cref="QQBot.Cacheable{TEntity,TId}"/> 参数是删除此子频道的用户。如果缓存中存在此用户实体，那么该结构内包含该
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
}
