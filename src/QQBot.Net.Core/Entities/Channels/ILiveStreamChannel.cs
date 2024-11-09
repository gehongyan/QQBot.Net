namespace QQBot;

/// <summary>
///     表示一个直播子频道。
/// </summary>
public interface ILiveStreamChannel : INestedChannel
{
    /// <summary>
    ///     修改此直播子频道。
    /// </summary>
    /// <param name="func"> 一个包含修改直播子频道属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示直播子频道属性修改操作的异步任务。 </returns>
    Task ModifyAsync(Action<ModifyLiveStreamChannelProperties> func, RequestOptions? options = null);

    /// <summary>
    ///     获取此频道内在线用户的数量。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道内在线用户的数量；如果无法获取，则返回 <c>null</c>。 </returns>
    Task<int> CountOnlineUsersAsync(RequestOptions? options = null);
}

