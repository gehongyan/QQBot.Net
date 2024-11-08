namespace QQBot;

/// <summary>
///     表示一个分组子频道。
/// </summary>
public interface ICategoryChannel : IGuildChannel
{
    /// <summary>
    ///     修改此分组子频道。
    /// </summary>
    /// <param name="func"> 一个包含修改分组子频道属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示分组子频道属性修改操作的异步任务。 </returns>
    Task ModifyAsync(Action<ModifyCategoryChannelProperties> func, RequestOptions? options = null);
}
