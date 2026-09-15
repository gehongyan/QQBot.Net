namespace QQBot;

/// <summary>
///     表示要发送的一个流式消息分片。
/// </summary>
public class StreamMessageChunk
{
    /// <summary>
    ///     获取分片内容。
    /// </summary>
    public string Content { get; }

    /// <summary>
    ///     获取内容格式。
    /// </summary>
    public StreamMessageContentType ContentType { get; }

    /// <summary>
    ///     获取输入模式。
    /// </summary>
    public StreamMessageInputMode InputMode { get; }

    /// <summary>
    ///     获取输入状态。
    /// </summary>
    public StreamMessageInputState InputState { get; }

    /// <summary>
    ///     获取分片序号。
    /// </summary>
    /// <remarks>
    ///     同一流式消息的分片序号必须从零开始递增。
    /// </remarks>
    public int Index { get; }

    /// <summary>
    ///     获取流式消息的唯一标识符。
    /// </summary>
    /// <remarks>
    ///     首个分片应为 <see langword="null"/>；后续分片应使用首个分片响应中的标识符。
    /// </remarks>
    public string? StreamMessageId { get; }

    /// <summary>
    ///     获取用于去重的消息序号。
    /// </summary>
    /// <remarks>
    ///     为 <see langword="null"/> 时，由 QQBot.Net 自动生成。
    /// </remarks>
    public int? MessageSequence { get; }

    /// <summary>
    ///     初始化一个 <see cref="StreamMessageChunk"/> 类的新实例。
    /// </summary>
    /// <param name="content"> 分片内容。 </param>
    /// <param name="contentType"> 内容格式。 </param>
    /// <param name="inputMode"> 输入模式。 </param>
    /// <param name="inputState"> 输入状态。 </param>
    /// <param name="index"> 分片序号。 </param>
    /// <param name="streamMessageId"> 流式消息标识符。 </param>
    /// <param name="messageSequence"> 用于去重的消息序号。 </param>
    /// <exception cref="ArgumentNullException"> <paramref name="content"/> 为 <see langword="null"/> 时引发。 </exception>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index"/> 小于零时引发。 </exception>
    /// <exception cref="ArgumentException"> <paramref name="streamMessageId"/> 为空白字符，或与 <paramref name="index"/> 不匹配时引发。 </exception>
    public StreamMessageChunk(string content, StreamMessageContentType contentType = StreamMessageContentType.Text,
        StreamMessageInputMode inputMode = StreamMessageInputMode.Append,
        StreamMessageInputState inputState = StreamMessageInputState.Generating, int index = 0,
        string? streamMessageId = null, int? messageSequence = null)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        if (streamMessageId is not null && string.IsNullOrWhiteSpace(streamMessageId))
            throw new ArgumentException("The stream message ID cannot be whitespace.", nameof(streamMessageId));
        if (index == 0 && streamMessageId is not null)
            throw new ArgumentException("The first stream message chunk must not specify a stream message ID.", nameof(streamMessageId));
        if (index > 0 && streamMessageId is null)
            throw new ArgumentException("A continuation stream message chunk must specify a stream message ID.", nameof(streamMessageId));

        Content = content;
        ContentType = contentType;
        InputMode = inputMode;
        InputState = inputState;
        Index = index;
        StreamMessageId = streamMessageId;
        MessageSequence = messageSequence;
    }
}
