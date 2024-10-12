namespace QQBot;

/// <summary>
///     提供用于各种消息实体的扩展方法。
/// </summary>
public static class MessageExtensions
{
    /// <summary>
    ///     向消息所属的子频道回复消息。
    /// </summary>
    /// <param name="message"> 要回复的消息。 </param>
    /// <param name="markdown"> 要回复的 Markdown 消息内容。 </param>
    /// <param name="attachment"> 要发送的文件附件。 </param>
    /// <param name="content"> 要发送的消息内容。 </param>
    /// <param name="embed"> 要发送的嵌入式消息内容。 </param>
    /// <param name="ark"> 要发送的模板消息内容。 </param>
    /// <param name="keyboard"> 要发送的按钮。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    public static async Task<IUserMessage> ReplyAsync(this IUserMessage message,
        string? content = null, IMarkdown? markdown = null, FileAttachment? attachment = null,
        Embed? embed = null, Ark? ark = null, IKeyboard? keyboard = null,
        MessageReference? messageReference = null, RequestOptions? options = null) =>
        await message.Channel
            .SendMessageAsync(content, markdown, attachment, embed, ark, keyboard, messageReference, message, options)
            .ConfigureAwait(false);
}
