namespace QQBot;

/// <summary>
///     表示一条群全员定时禁言规则。
/// </summary>
public class GroupMuteScheduleRule
{
    /// <summary>
    ///     获取此定时禁言任务的标识符。
    /// </summary>
    public string TaskId { get; }

    /// <summary>
    ///     获取禁言开始时间。
    /// </summary>
    public DateTimeOffset StartAt { get; }

    /// <summary>
    ///     获取禁言结束时间。
    /// </summary>
    public DateTimeOffset EndAt { get; }

    /// <summary>
    ///     获取此规则是否启用。
    /// </summary>
    public bool IsEnabled { get; }

    internal GroupMuteScheduleRule(string taskId, DateTimeOffset startAt, DateTimeOffset endAt, bool isEnabled)
    {
        TaskId = taskId;
        StartAt = startAt;
        EndAt = endAt;
        IsEnabled = isEnabled;
    }
}
