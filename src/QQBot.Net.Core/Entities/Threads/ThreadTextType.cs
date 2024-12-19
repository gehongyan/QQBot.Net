namespace QQBot;

/// <summary>
///     表示一个帖子文本的格式。
/// </summary>
public enum ThreadTextType
{
    /// <summary>
    ///     普通文本。
    /// </summary>
    Text = 1,

    /// <summary>
    ///     HTML。
    /// </summary>
    Html = 2,

    /// <summary>
    ///     Markdown。
    /// </summary>
    Markdown = 3,

    /// <summary>
    ///     JSON 富文本。
    /// </summary>
    Json = 4
}
