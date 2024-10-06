namespace QQBot;

/// <summary>
///     表示一个消息频道，可以发送和接收消息。
/// </summary>
public interface IMessageChannel : IChannel
{
    /// <summary>
    ///     向此频道发送消息。
    /// </summary>
    /// <param name="content"> 要发送的消息内容。 </param>
    /// <param name="attachment"> 要发送的文件附件。 </param>
    /// <param name="embed"> 要发送的嵌入式消息内容。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="passiveSource"> 被动消息来源。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    Task<Cacheable<IUserMessage, string>> SendMessageAsync(string? content = null,
        FileAttachment? attachment = null, Embed? embed = null,
        MessageReference? messageReference = null, IUserMessage? passiveSource = null,
        RequestOptions? options = null);
}
