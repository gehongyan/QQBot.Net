namespace QQBot;

/// <summary>
///     表示一个通用的消息。
/// </summary>
public interface IMessage : IEntity<string>
{
    /// <summary>
    ///     获取消息的来源频道。
    /// </summary>
    IMessageChannel Channel { get; }

    /// <summary>
    ///     获取消息的发送者。
    /// </summary>
    IUser Author { get; }

    /// <summary>
    ///     获取消息的来源
    /// </summary>
    MessageSource Source { get; }

    /// <summary>
    ///     获取消息的来源标识符。
    /// </summary>
    MessageSourceIdentifier? SourceIdentifier { get; }

    /// <summary>
    ///     获取消息的内容。
    /// </summary>
    string Content { get; }

    /// <summary>
    ///     获取消息的创建时间。
    /// </summary>
    DateTimeOffset Timestamp { get; }

    /// <summary>
    ///     获取此消息中包含的所有附件。
    /// </summary>
    IReadOnlyCollection<IAttachment> Attachments { get; }

    /// <summary>
    ///     获取此消息是否提及了全体成员。
    /// </summary>
    bool? MentionedEveryone { get; }

    /// <summary>
    ///     获取此消息内包含的所有嵌入式内容。
    /// </summary>
    IReadOnlyCollection<Embed> Embeds { get; }
}
