namespace QQBot;

/// <summary>
///     表示一个论坛子频道。
/// </summary>
public interface IForumChannel : INestedChannel
{
    /// <summary>
    ///     修改此论坛子频道。
    /// </summary>
    /// <param name="func"> 一个包含修改论坛子频道属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示论坛子频道属性修改操作的异步任务。 </returns>
    Task ModifyAsync(Action<ModifyForumChannelProperties> func, RequestOptions? options = null);

    /// <summary>
    ///     获取此论坛子频道的所有主题。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此论坛子频道的所有主题。 </returns>
    Task<IReadOnlyCollection<IForumThread>> GetThreadsAsync(RequestOptions? options = null);

    /// <summary>
    ///     获取此论坛子频道的主题。
    /// </summary>
    /// <param name="id"> 要获取的主题的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此论坛子频道的所有主题。 </returns>
    Task<IForumThread> GetThreadAsync(string id, RequestOptions? options = null);
}
