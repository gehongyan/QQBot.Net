namespace QQBot;

/// <summary>
///     表示一个群组子频道，即 QQ 群。
/// </summary>
public interface IGroupChannel : IMessageChannel, IEntity<Guid>
{
    /// <summary>
    ///     获取此群组子频道的唯一标识符。
    /// </summary>
    new Guid Id { get; }

    /// <summary>
    ///     撤回此群组子频道内的消息。
    /// </summary>
    /// <param name="messageId"> 要撤回的消息 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤回操作的任务。 </returns>
    Task DeleteMessageAsync(string messageId, RequestOptions? options = null);

    /// <summary>
    ///     撤回此群组子频道内的消息。
    /// </summary>
    /// <param name="message"> 要撤回的消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤回操作的任务。 </returns>
    Task DeleteMessageAsync(IUserMessage message, RequestOptions? options = null);
}
