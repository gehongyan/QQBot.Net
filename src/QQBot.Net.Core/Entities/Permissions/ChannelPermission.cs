namespace QQBot;

/// <summary>
///     表示可以为角色或用户设置的子频道级别的权限。
/// </summary>
[Flags]
public enum ChannelPermission : uint
{
    /// <summary>
    ///     可查看子频道。
    /// </summary>
    ViewChannel = 1 << 0,

    /// <summary>
    ///     可管理子频道。
    /// </summary>
    ManageChannels = 1 << 1,

    /// <summary>
    ///     可发言子频道。
    /// </summary>
    SendMessages = 1 << 2,

    /// <summary>
    ///     可直播子频道。
    /// </summary>
    Stream = 1 << 3,
}
