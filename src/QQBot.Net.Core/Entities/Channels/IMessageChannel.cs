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
    /// <param name="markdown"> 要发送的 Markdown 消息内容。 </param>
    /// <param name="attachment"> 要发送的文件附件。 </param>
    /// <param name="embed"> 要发送的嵌入式消息内容。 </param>
    /// <param name="ark"> 要发送的模板消息内容。 </param>
    /// <param name="keyboard"> 要发送的按钮。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="passiveSource"> 被动消息来源。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    Task<Cacheable<IUserMessage, string>> SendMessageAsync(string? content = null, IMarkdown? markdown = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null, IKeyboard? keyboard = null,
        MessageReference? messageReference = null, IUserMessage? passiveSource = null, RequestOptions? options = null);

    /// <summary>
    ///     从此消息频道获取一条消息。
    /// </summary>
    /// <param name="id"> 消息的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务结果包含检索到的消息；如果未找到具有指定 ID 的消息，则返回 <c>null</c>。 </returns>
    Task<IMessage?> GetMessageAsync(string id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);
}
