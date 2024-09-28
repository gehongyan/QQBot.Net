namespace QQBot;

/// <summary>
///     指定客户端的连接状态。
/// </summary>
public enum ConnectionState : byte
{
    /// <summary>
    ///     客户端已断开与 QQ 的连接。
    /// </summary>
    Disconnected,

    /// <summary>
    ///     客户端正在连接到 QQ。
    /// </summary>
    Connecting,

    /// <summary>
    ///     客户端已连接到 QQ。
    /// </summary>
    Connected,

    /// <summary>
    ///     客户端正在断开与 QQ 的连接。
    /// </summary>
    Disconnecting
}
