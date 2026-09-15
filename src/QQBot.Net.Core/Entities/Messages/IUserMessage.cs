namespace QQBot;

/// <summary>
///     表示一个通用的由用户发送的消息。
/// </summary>
public interface IUserMessage : IMessage, IDeletable
{
    /// <summary>
    ///     获取可用于引用此消息的消息引用。
    /// </summary>
    /// <remarks>
    ///     此引用仅在 QQ 单聊或群聊发送响应提供时可用，可直接作为后续发送消息的
    ///     <see cref="IMessageChannel.SendMessageAsync"/> 的消息引用参数。平台未提供该信息、
    ///     频道消息、频道私信和从网关接收的消息均可能返回 <see langword="null"/>。
    /// </remarks>
    MessageReference? ReplyReference { get; }

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
