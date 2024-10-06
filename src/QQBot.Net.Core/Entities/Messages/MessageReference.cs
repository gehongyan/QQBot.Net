using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个消息引用。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MessageReference
{
    /// <summary>
    ///     使用指定的消息 ID 创建一个新的 <see cref="MessageReference"/> 实例。
    /// </summary>
    /// <param name="messageId"> 要引用的消息的 ID。 </param>
    /// <param name="failIfNotExists"> 是否在引用的消息不存在时引发错误而不是发送为普通消息，未指定时同 <see langword="true"/>。 </param>
    public MessageReference(string messageId, bool? failIfNotExists = null)
    {
        MessageId = messageId;
    }

    /// <summary>
    ///     获取要引用的消息的 ID。
    /// </summary>
    public string MessageId { get; }

    /// <summary>
    ///     获取或设置是否在引用的消息不存在时引发错误而不是发送为普通消息，未指定时同 <see langword="true"/>。
    /// </summary>
    public bool? FailIfNotExists { get; internal set; }

    private string DebuggerDisplay => $"MessageId: {MessageId}"
     + $"{(FailIfNotExists.HasValue ? $", FailIfNotExists: ({FailIfNotExists.Value})" : "")}";
}
