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
}


