namespace QQBot;

/// <summary>
///     表示表情表态所针对的目标对象的类型。
/// </summary>
public enum ReactionTargetType
{
    /// <summary>
    ///     表态针对一条消息。
    /// </summary>
    Message = 0,

    /// <summary>
    ///     表态针对一个论坛主题。
    /// </summary>
    Thread = 1,

    /// <summary>
    ///     表态针对一条论坛主题评论。
    /// </summary>
    Post = 2,

    /// <summary>
    ///     表态针对一条论坛主题评论的回复。
    /// </summary>
    Reply = 3
}
