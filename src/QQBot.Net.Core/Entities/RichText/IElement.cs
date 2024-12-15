namespace QQBot;

/// <summary>
///     表示一个富文本元素。
/// </summary>
public interface IElement
{
    /// <summary>
    ///     获取此元素的类型。
    /// </summary>
    ElementType Type { get; }
}
