namespace QQBot;

/// <summary>
///     表示一个用户单聊子频道。
/// </summary>
/// <remarks>
///     这可以包括 QQ 好友发起的聊天，或 QQ 群内用户发起的私聊，不包括子频道内用户发起的私聊。
/// </remarks>
public interface IUserChannel : IMessageChannel, IPrivateChannel, IMediaUploadChannel, IEntity<Guid>
{
    /// <summary>
    ///     获取此用户单聊子频道的唯一标识符。
    /// </summary>
    new Guid Id { get; }

    /// <summary>
    ///     显示机器人正在输入的状态。
    /// </summary>
    /// <remarks>
    ///     此方法仅适用于 QQ 单聊。未指定持续时间时，状态默认持续 60 秒；有效范围为大于零且不超过 60 秒。
    ///     可选地提供触发输入状态的用户消息，以便作为该消息的被动回复发送。
    /// </remarks>
    /// <param name="duration"> 输入中状态的持续时间；为 <see langword="null"/> 时使用 60 秒。非整秒持续时间会向上取整到下一整秒。 </param>
    /// <param name="passiveSource"> 可选的被动回复消息来源。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。 </returns>
    Task TriggerTypingAsync(TimeSpan? duration = null, IUserMessage? passiveSource = null,
        RequestOptions? options = null);

    /// <summary>
    ///     发送一个流式消息分片。
    /// </summary>
    /// <remarks>
    ///     此方法仅适用于 QQ 单聊，并保留分片序号、输入模式和完成状态的完整控制权。多数场景应使用
    ///     <see cref="StartStreamMessageAsync"/> 创建由 QQBot.Net 管理的流式消息会话。
    /// </remarks>
    /// <param name="chunk"> 要发送的流式消息分片。 </param>
    /// <param name="passiveSource"> 可选的被动回复消息来源。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 分片发送结果。 </returns>
    Task<StreamMessageChunkResult> SendStreamMessageChunkAsync(StreamMessageChunk chunk,
        IUserMessage? passiveSource = null, RequestOptions? options = null);

    /// <summary>
    ///     发送一个单聊互动召回流式消息分片。
    /// </summary>
    /// <remarks>
    ///     此方法仅适用于 QQ 单聊互动召回，不能作为对用户消息或事件的被动回复发送。平台的互动召回额度
    ///     和时间窗口限制与 <see cref="SendWakeupMessageAsync"/> 相同；QQBot.Net 不预判平台额度。
    /// </remarks>
    /// <param name="chunk"> 要发送的流式消息分片。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 分片发送结果。 </returns>
    Task<StreamMessageChunkResult> SendWakeupStreamMessageChunkAsync(StreamMessageChunk chunk,
        RequestOptions? options = null);

    /// <summary>
    ///     开始一个由 QQBot.Net 管理的流式消息会话。
    /// </summary>
    /// <remarks>
    ///     此方法仅适用于 QQ 单聊。返回的会话会自动维护分片序号和流式消息标识符；使用
    ///     <see cref="IUserMessageStream.AppendAsync"/> 追加内容，并使用
    ///     <see cref="IUserMessageStream.CompleteAsync"/> 显式结束消息。
    /// </remarks>
    /// <param name="initialContent"> 首个内容分片。 </param>
    /// <param name="contentType"> 内容格式。 </param>
    /// <param name="passiveSource"> 可选的被动回复消息来源。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 已开始的流式消息会话。 </returns>
    Task<IUserMessageStream> StartStreamMessageAsync(string initialContent,
        StreamMessageContentType contentType = StreamMessageContentType.Text,
        IUserMessage? passiveSource = null, RequestOptions? options = null);

    /// <summary>
    ///     开始一个单聊互动召回流式消息会话。
    /// </summary>
    /// <remarks>
    ///     此方法仅适用于 QQ 单聊互动召回，不能作为对用户消息或事件的被动回复发送。平台的互动召回额度
    ///     和时间窗口限制与 <see cref="SendWakeupMessageAsync"/> 相同；QQBot.Net 不预判平台额度。
    /// </remarks>
    /// <param name="initialContent"> 首个内容分片。 </param>
    /// <param name="contentType"> 内容格式。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 已开始的流式消息会话。 </returns>
    Task<IUserMessageStream> StartWakeupStreamMessageAsync(string initialContent,
        StreamMessageContentType contentType = StreamMessageContentType.Text,
        RequestOptions? options = null);

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
