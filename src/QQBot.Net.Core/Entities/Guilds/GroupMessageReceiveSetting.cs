namespace QQBot;

/// <summary>
///     表示机器人在群内接收消息的设置。
/// </summary>
public enum GroupMessageReceiveSetting
{
    /// <summary>
    ///     接收群内的所有消息。
    /// </summary>
    All,

    /// <summary>
    ///     仅接收提及机器人的消息。
    /// </summary>
    OnlyMention,

    /// <summary>
    ///     接收提及机器人的消息及其上下文消息。
    /// </summary>
    MentionAndContext
}
