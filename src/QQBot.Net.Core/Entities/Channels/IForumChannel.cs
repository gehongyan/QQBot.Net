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
    Task<IReadOnlyCollection<IThread>> GetThreadsAsync(RequestOptions? options = null);

    /// <summary>
    ///     获取此论坛子频道的主题。
    /// </summary>
    /// <param name="id"> 要获取的主题的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此论坛子频道的所有主题。 </returns>
    Task<IThread> GetThreadAsync(string id, RequestOptions? options = null);

    /// <summary>
    ///     创建一个新的主题。
    /// </summary>
    /// <param name="title"> 主题的标题。 </param>
    /// <param name="textType"> 主题的内容类型。 </param>
    /// <param name="content"> 主题的内容。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的主题。 </returns>
    Task CreateThreadAsync(string title, ThreadTextType textType, string content, RequestOptions? options = null);

    /// <summary>
    ///     创建一个新的主题。
    /// </summary>
    /// <param name="title"> 主题的标题。 </param>
    /// <param name="content"> 主题的内容。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的主题。 </returns>
    Task CreateThreadAsync(string title, RichTextBuilder content, RequestOptions? options = null);
}
