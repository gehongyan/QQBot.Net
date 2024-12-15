namespace QQBot;

/// <summary>
///     表示一个通用的论坛主题。
/// </summary>
public interface IForumThread : IEntity<string>
{
    /// <summary>
    ///     获取此主题所属的频道。
    /// </summary>
    IGuild Guild { get; }

    /// <summary>
    ///     获取次主题所属的论坛子频道。
    /// </summary>
    IForumChannel Channel { get; }

    /// <summary>
    ///     获取此主题的作者用户的 ID。
    /// </summary>
    ulong AuthorId { get; }

    /// <summary>
    ///     获取此主题的标题。
    /// </summary>
    string Title { get; }

    /// <summary>
    ///     获取此主题的原始内容。
    /// </summary>
    string RawContent { get; }

    /// <summary>
    ///     获取此主题的富文本内容。
    /// </summary>
    RichText Content { get; }

    /// <summary>
    ///     获取此主题的创建时间。
    /// </summary>
    DateTimeOffset CreatedAt { get; }
}
