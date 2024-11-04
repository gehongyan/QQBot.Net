namespace QQBot;

/// <summary>
///     表示一个子频道。
/// </summary>
public interface IChannel : IEntity<string>
{
    /// <summary>
    ///     获取此频道中的用户。
    /// </summary>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务结果包含表示找到的用户；如果没有找到，则返回 <c>null</c>。 </returns>
    Task<IUser?> GetUserAsync(string id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);
}
