namespace QQBot;

/// <summary>
///     表示论坛发表内容审核所针对的对象类型。
/// </summary>
public enum ForumAuditType
{
    /// <summary>
    ///     审核针对一个论坛主题。
    /// </summary>
    Thread = 1,

    /// <summary>
    ///     审核针对一条论坛主题评论。
    /// </summary>
    Post = 2,

    /// <summary>
    ///     审核针对一条论坛主题评论的回复。
    /// </summary>
    Reply = 3
}
