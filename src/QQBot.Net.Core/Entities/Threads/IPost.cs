namespace QQBot;

/// <summary>
///     表示一个通用的论坛主题评论。
/// </summary>
public interface IPost : IEntity<string>
{
    /// <summary>
    ///     获取此主题评论所属的频道。
    /// </summary>
    IGuild Guild { get; }

    /// <summary>
    ///     获取此主题评论所属的论坛子频道。
    /// </summary>
    IForumChannel Channel { get; }

    /// <summary>
    ///     获取此主题评论的作者用户的 ID。
    /// </summary>
    ulong AuthorId { get; }

    /// <summary>
    ///     获取此主题评论所属的主题的 ID。
    /// </summary>
    string ThreadId { get; }

    /// <summary>
    ///     获取此主题的原始内容。
    /// </summary>
    string RawContent { get; }

    /// <summary>
    ///     获取此主题评论的富文本内容。
    /// </summary>
    RichText Content { get; }

    /// <summary>
    ///     获取此主题评论的创建时间。
    /// </summary>
    DateTimeOffset CreatedAt { get; }
}
