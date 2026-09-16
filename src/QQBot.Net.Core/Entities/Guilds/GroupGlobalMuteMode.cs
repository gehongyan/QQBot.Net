namespace QQBot;

/// <summary>
///     表示群的全员禁言模式。
/// </summary>
public enum GroupGlobalMuteMode
{
    /// <summary>
    ///     未开启全员禁言。
    /// </summary>
    None,

    /// <summary>
    ///     始终全员禁言。
    /// </summary>
    Always,

    /// <summary>
    ///     定时或周期性全员禁言。
    /// </summary>
    Schedule
}
