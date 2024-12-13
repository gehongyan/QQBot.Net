namespace QQBot;

/// <summary>
///     提供用于修改 <see cref="QQBot.IGuildSchedule" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuildSchedule.ModifyAsync(System.Action{QQBot.ModifyGuildScheduleProperties},QQBot.RequestOptions)"/>
public class ModifyGuildScheduleProperties
{
    /// <summary>
    ///     获取或设置要设置到此日程的名称。
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     获取或设置要设置到此日程的描述。
    /// </summary>
    public required string? Description { get; set; }

    /// <summary>
    ///     获取或设置要设置到此日程的开始时间。
    /// </summary>
    public required DateTimeOffset StartTime { get; set; }

    /// <summary>
    ///     获取或设置要设置到此日程的结束时间。
    /// </summary>
    public required DateTimeOffset EndTime { get; set; }

    /// <summary>
    ///     获取或设置要设置到此日程在开始时要跳转到的子频道的 ID。
    /// </summary>
    public required ulong? JumpChannelId { get; set; }

    /// <summary>
    ///     获取或设置要设置到此日程的提醒类型。
    /// </summary>
    public required RemindType RemindType { get; set; }
}
