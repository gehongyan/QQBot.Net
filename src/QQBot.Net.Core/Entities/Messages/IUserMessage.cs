namespace QQBot;

/// <summary>
///     表示一个通用的由用户发送的消息。
/// </summary>
public interface IUserMessage : IMessage, IDeletable
{
    /// <summary>
    ///     置顶消息。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task PinAsync(RequestOptions? options = null);

    /// <summary>
    ///     取消置顶消息。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task UnpinAsync(RequestOptions? options = null);
}
