namespace QQBot;

/// <summary>
///     表示一个通用的日程。
/// </summary>
public interface IGuildSchedule : IEntity<ulong>, IUpdateable, IDeletable
{
    /// <summary>
    ///     获取此日程所属的频道。
    /// </summary>
    IScheduleChannel Channel { get; }

    /// <summary>
    ///     获取此日程的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此日程的描述。
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     获取此日程的开始时间。
    /// </summary>
    DateTimeOffset StartTime { get; }

    /// <summary>
    ///     获取此日程的结束时间。
    /// </summary>
    DateTimeOffset EndTime { get; }

    /// <summary>
    ///     获取此日程的创建者。
    /// </summary>
    IGuildMember? Creator { get; }

    /// <summary>
    ///     获取此日程开始时跳转到的子频道的 ID。
    /// </summary>
    ulong? JumpChannelId { get; }

    /// <summary>
    ///     获取此日程的提醒类型。
    /// </summary>
    RemindType RemindType { get; }

    /// <summary>
    ///     修改此日程。
    /// </summary>
    /// <param name="func"> 一个包含修改日程属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示直日程性修改操作的异步任务。 </returns>
    Task ModifyAsync(Action<ModifyGuildScheduleProperties> func, RequestOptions? options = null);
}
