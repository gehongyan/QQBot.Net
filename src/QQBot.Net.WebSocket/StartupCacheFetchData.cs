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
    Roles = 1 << 0,

    /// <summary>
    ///     加载子频道列表。
    /// </summary>
    Channels = 1 << 1,

    /// <summary>
    ///     加载频道用户列表。
    /// </summary>
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
