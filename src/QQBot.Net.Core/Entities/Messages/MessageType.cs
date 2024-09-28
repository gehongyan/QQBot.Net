namespace QQBot;

/// <summary>
///     表示消息的类型。
/// </summary>
public enum MessageType
{
    /// <summary>
    ///     文本消息。
    /// </summary>
    Text = 0,

    /// <summary>
    ///     Markdown 消息。
    /// </summary>
    Markdown = 2,

    /// <summary>
    ///     Ark 消息。
    /// </summary>
    Ark = 3,

    /// <summary>
    ///     Embed 消息。
    /// </summary>
    Embed = 4,

    /// <summary>
    ///     富媒体消息。
    /// </summary>
    Media = 7
}
