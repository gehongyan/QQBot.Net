namespace QQBot;

/// <summary>
///     表示一个消息中通用的标签。
/// </summary>
public interface ITag
{
    /// <summary>
    ///     获取消息中标签的位置。
    /// </summary>
    int Index { get; }

    /// <summary>
    ///     获取标签的长度。
    /// </summary>
    int Length { get; }

    /// <summary>
    ///     获取标签的类型。
    /// </summary>
    TagType Type { get; }

    /// <summary>
    ///     获取标签的键。
    /// </summary>
    object Key { get; }

    /// <summary>
    ///     获取标签的值。
    /// </summary>
    object? Value { get; }
}
