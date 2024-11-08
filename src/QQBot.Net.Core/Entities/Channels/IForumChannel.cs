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
}
