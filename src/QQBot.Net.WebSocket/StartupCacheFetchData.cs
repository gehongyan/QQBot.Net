namespace QQBot.WebSocket;

/// <summary>
///     表示基于网关的客户端在启动时所加载的除频道列表外的其它数据。
/// </summary>
[Flags]
public enum StartupCacheFetchData
{
    /// <summary>
    ///     不在启动时加载除频道列表外的其它数据。
    /// </summary>
    None = 0,

    /// <summary>
    ///     加载频道角色列表。
    /// </summary>
    /// <remarks>
    ///     角色列表会在 Bot 启动时优先下载，若 <see cref="QQBot.WebSocket.QQBotSocketConfig.StartupCacheFetchMode"/>
    ///     设置为 <see cref="QQBot.WebSocket.StartupCacheFetchMode.Synchronous"/>，
    ///     <see cref="QQBot.WebSocket.QQBotSocketClient.Ready"/> 事件引发时角色列表应已经下载完成。
    ///     调用 <see cref="QQBot.WebSocket.SocketGuild.UpdateAsync(QQBot.RequestOptions)"/> 时也会更新角色列表。
    /// </remarks>
    Roles = 1 << 0,

    /// <summary>
    ///     加载子频道列表。
    /// </summary>
    /// <remarks>
    ///     子频道列表会在 Bot 启动时优先下载，若 <see cref="QQBot.WebSocket.QQBotSocketConfig.StartupCacheFetchMode"/>
    ///     设置为 <see cref="QQBot.WebSocket.StartupCacheFetchMode.Synchronous"/>，
    ///     <see cref="QQBot.WebSocket.QQBotSocketClient.Ready"/> 事件引发时子频道列表应已经下载完成。
    ///     调用 <see cref="QQBot.WebSocket.SocketGuild.UpdateAsync(QQBot.RequestOptions)"/> 时也会更新角色列表。
    /// </remarks>
    Channels = 1 << 1,

    /// <summary>
    ///     加载频道用户列表。
    /// </summary>
    /// <remarks>
    ///     子频道列表会在 Bot 启动时推迟下载，即便 <see cref="QQBot.WebSocket.QQBotSocketConfig.StartupCacheFetchMode"/>
    ///     设置为 <see cref="QQBot.WebSocket.StartupCacheFetchMode.Synchronous"/>，
    ///     <see cref="QQBot.WebSocket.QQBotSocketClient.Ready"/> 事件引发时也无法保证频道用户列表下载完成。
    ///     调用 <see cref="QQBot.WebSocket.SocketGuild.UpdateAsync(QQBot.RequestOptions)"/> 时不会更新频道用户列表。
    ///     要获取频道用户列表，需要调用 <see cref="QQBot.WebSocket.SocketGuild.DownloadUsersAsync(QQBot.RequestOptions)"/>。
    /// </remarks>
    GuildUsers = 1 << 2,

    /// <summary>
    ///     加载所有公域机器人可以加载的数据。
    /// </summary>
    AllPublicDomain = Roles | Channels,

    /// <summary>
    ///     加载所有基础数据。
    /// </summary>
    All = AllPublicDomain | GuildUsers
}
