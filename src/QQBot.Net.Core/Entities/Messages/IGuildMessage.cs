namespace QQBot;

/// <summary>
///     表示一个通用的频道子频道或频道内用户私聊的消息。
/// </summary>
public interface IGuildMessage : IMessage
{
    /// <summary>
    ///     获取此消息是否提及了全体成员。
    /// </summary>
    bool? MentionedEveryone { get; }

    /// <summary>
    ///     获取此消息内包含的所有嵌入式内容。
    /// </summary>
    IReadOnlyCollection<Embed> Embeds { get; }
}
