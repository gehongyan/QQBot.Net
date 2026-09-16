using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一条群全员周期禁言规则。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class GroupMuteRecurringRule
{
    /// <summary>
    ///     获取此周期禁言任务的标识符。
    /// </summary>
    public string TaskId { get; }

    /// <summary>
    ///     获取此规则生效的星期。
    /// </summary>
    public IReadOnlyCollection<DayOfWeek> Weekdays { get; }

    /// <summary>
    ///     获取禁言时段的开始时间（北京时间）。
    /// </summary>
    public TimeOnly StartTime { get; }

    /// <summary>
    ///     获取禁言时段的结束时间（北京时间）；若早于 <see cref="StartTime"/> 表示跨天至次日。
    /// </summary>
    public TimeOnly EndTime { get; }

    /// <summary>
    ///     获取此规则是否启用。
    /// </summary>
    public bool IsEnabled { get; }

    internal GroupMuteRecurringRule(string taskId, IReadOnlyCollection<DayOfWeek> weekdays,
        TimeOnly startTime, TimeOnly endTime, bool isEnabled)
    {
        TaskId = taskId;
        Weekdays = weekdays;
        StartTime = startTime;
        EndTime = endTime;
        IsEnabled = isEnabled;
    }

    private string DebuggerDisplay =>
        $"{TaskId} ({StartTime}–{EndTime} ×{Weekdays.Count} days, {(IsEnabled ? "Enabled" : "Disabled")})";
}
