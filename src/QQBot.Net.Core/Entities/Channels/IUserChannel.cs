namespace QQBot;

/// <summary>
///     表示一个用户单聊子频道。
/// </summary>
/// <remarks>
///     这可以包括 QQ 好友发起的聊天，或 QQ 群内用户发起的私聊，不包括子频道内用户发起的私聊。
/// </remarks>
public interface IUserChannel : IMessageChannel, IPrivateChannel, IEntity<Guid>
{
    /// <summary>
    ///     获取此用户单聊子频道的唯一标识符。
    /// </summary>
    new Guid Id { get; }

    /// <summary>
    ///     向此单聊发送一条互动召回消息。
    /// </summary>
    /// <remarks>
    ///     此方法仅适用于 QQ 单聊互动召回，不能作为对用户消息或事件的被动回复发送。
    ///     QQ 平台在用户主动与机器人互动后的未来 30 天内，按当天、1–3 天、3–7 天、7–30 天四个窗口各允许一条召回消息；
    ///     SDK 不缓存或预判该额度，平台拒绝时会抛出 <see cref="QQBot.Net.HttpException"/>。
    /// </remarks>
    /// <param name="content"> 要发送的消息内容。 </param>
    /// <param name="markdown"> 要发送的 Markdown 消息内容。 </param>
    /// <param name="attachment"> 要发送的文件附件。 </param>
    /// <param name="embed"> 要发送的嵌入式消息内容。 </param>
    /// <param name="ark"> 要发送的模板消息内容。 </param>
    /// <param name="keyboard"> 要发送的按钮。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    Task<IUserMessage> SendWakeupMessageAsync(string? content = null, IMarkdown? markdown = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null, IKeyboard? keyboard = null,
        MessageReference? messageReference = null, RequestOptions? options = null);
}
