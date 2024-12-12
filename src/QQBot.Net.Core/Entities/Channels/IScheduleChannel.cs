namespace QQBot;

/// <summary>
///     表示一个日程子频道。
/// </summary>
public interface IScheduleChannel : INestedChannel
{
    /// <summary>
    ///     修改此日程子频道。
    /// </summary>
    /// <param name="func"> 一个包含修改日程子频道属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示日程子频道属性修改操作的异步任务。 </returns>
    Task ModifyAsync(Action<ModifyScheduleChannelProperties> func, RequestOptions? options = null);

    /// <summary>
    ///     获取此日程子频道的所有当日日程。
    /// </summary>
    /// <param name="since"> 当不为 <c>null</c> 时，将获取结束时间在此参数表示的时间之后的日程列表；当为 <c>null</c> 时，将获取当天的所有列表。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务，其结果为此日程子频道的所有当日日程的只读集合。 </returns>
    Task<IReadOnlyCollection<IGuildSchedule>> GetSchedulesAsync(DateTimeOffset? since = null, RequestOptions? options = null);

    /// <summary>
    ///     获取此日程子频道的指定日程。
    /// </summary>
    /// <param name="id"> 要获取的日程的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务，其结果为此日程子频道的指定日程。 </returns>
    Task<IGuildSchedule> GetScheduleAsync(ulong id, RequestOptions? options = null);

    /// <summary>
    ///     创建一个新的日程
    /// </summary>
    /// <param name="name"> 日程名称。 </param>
    /// <param name="startTime"> 日程开始时间。 </param>
    /// <param name="endTime"> 日程结束时间。 </param>
    /// <param name="description"> 日程描述。 </param>
    /// <param name="jumpChannel"> 日程开始时要跳转到的子频道。 </param>
    /// <param name="remindType"> 日程提醒类型。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务，其结果为新创建的日程。 </returns>
    Task<IGuildSchedule> CreateScheduleAsync(string name, DateTimeOffset startTime, DateTimeOffset endTime,
        string? description = null, IGuildChannel? jumpChannel = null, RemindType remindType = RemindType.None,
        RequestOptions? options = null);
}


