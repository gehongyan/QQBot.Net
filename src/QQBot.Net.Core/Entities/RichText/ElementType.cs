namespace QQBot;

/// <summary>
///     表示一个富文本元素的类型。
/// </summary>
public enum ElementType
{
    /// <summary>
    ///     表示一个空元素。
    /// </summary>
    Empty = 0,

    /// <summary>
    ///     表示一个文本元素。
    /// </summary>
    Text = 1,

    /// <summary>
    ///     表示一个图片元素。
    /// </summary>
    Image = 2,

    /// <summary>
    ///     表示一个视频元素。
    /// </summary>
    Video = 3,

    /// <summary>
    ///     表示一个 URL 元素。
    /// </summary>
    Url = 4
}
