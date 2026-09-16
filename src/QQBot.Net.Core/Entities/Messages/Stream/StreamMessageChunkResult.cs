using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示流式消息分片发送后的结果。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class StreamMessageChunkResult
{
    /// <summary>
    ///     获取流式消息的唯一标识符。
    /// </summary>
    public string StreamMessageId { get; }

    /// <summary>
    ///     获取平台记录的消息发送时间。
    /// </summary>
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    ///     获取流式消息剩余可发送长度。
    /// </summary>
    /// <remarks>
    ///     平台未提供该信息时为 <see langword="null"/>。
    /// </remarks>
    public int? RemainingMessageLength { get; }

    /// <summary>
    ///     获取可用于引用此流式消息的消息引用。
    /// </summary>
    /// <remarks>
    ///     平台未提供引用信息时为 <see langword="null"/>。
    /// </remarks>
    public MessageReference? ReplyReference { get; }

    internal StreamMessageChunkResult(string streamMessageId, DateTimeOffset timestamp,
        int? remainingMessageLength, MessageReference? replyReference)
    {
        StreamMessageId = streamMessageId;
        Timestamp = timestamp;
        RemainingMessageLength = remainingMessageLength;
        ReplyReference = replyReference;
    }

    private string DebuggerDisplay => $"{StreamMessageId} ({Timestamp:u})";
}
