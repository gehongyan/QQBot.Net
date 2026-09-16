namespace QQBot;

/// <summary>
///     表示群的全员禁言规则。
/// </summary>
public class GroupGlobalMuteRule
{
    /// <summary>
    ///     获取全员禁言模式。
    /// </summary>
    public GroupGlobalMuteMode Mode { get; }

    /// <summary>
    ///     获取定时禁言规则列表。
    /// </summary>
    public IReadOnlyCollection<GroupMuteScheduleRule> ScheduleRules { get; }

    /// <summary>
    ///     获取周期禁言规则列表。
    /// </summary>
    public IReadOnlyCollection<GroupMuteRecurringRule> RecurringRules { get; }

    internal GroupGlobalMuteRule(GroupGlobalMuteMode mode,
        IReadOnlyCollection<GroupMuteScheduleRule> scheduleRules,
        IReadOnlyCollection<GroupMuteRecurringRule> recurringRules)
    {
        Mode = mode;
        ScheduleRules = scheduleRules;
        RecurringRules = recurringRules;
    }
}
