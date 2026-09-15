namespace QQBot;

/// <summary>
///     表示由 QQBot.Net 管理的单聊流式消息会话。
/// </summary>
public interface IUserMessageStream
{
    /// <summary>
    ///     获取流式消息的唯一标识符。
    /// </summary>
    string Id { get; }

    /// <summary>
    ///     获取流式消息的内容格式。
    /// </summary>
    StreamMessageContentType ContentType { get; }

    /// <summary>
    ///     获取下一个分片序号。
    /// </summary>
    int NextIndex { get; }

    /// <summary>
    ///     获取流式消息剩余可发送长度。
    /// </summary>
    int? RemainingMessageLength { get; }

    /// <summary>
    ///     获取可用于引用此流式消息的消息引用。
    /// </summary>
    MessageReference? ReplyReference { get; }

    /// <summary>
    ///     获取流式消息是否已结束。
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    ///     追加一个仍在生成中的内容分片。
    /// </summary>
    /// <param name="content"> 要追加的内容。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 分片发送结果。 </returns>
    /// <exception cref="InvalidOperationException"> 流式消息已结束时引发。 </exception>
    Task<StreamMessageChunkResult> AppendAsync(string content, RequestOptions? options = null);

    /// <summary>
    ///     追加最后一个内容分片并结束流式消息。
    /// </summary>
    /// <param name="content"> 要追加的最终内容。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 分片发送结果。 </returns>
    /// <exception cref="InvalidOperationException"> 流式消息已结束时引发。 </exception>
    Task<StreamMessageChunkResult> CompleteAsync(string content, RequestOptions? options = null);
}
