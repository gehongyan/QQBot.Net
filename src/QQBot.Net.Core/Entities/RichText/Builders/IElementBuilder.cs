namespace QQBot;

/// <summary>
///     表示一个富文本元素构建器。
/// </summary>
public interface IElementBuilder
{
    /// <summary>
    ///     获取此构建器构建的元素的类型。
    /// </summary>
    ElementType Type { get; }
}
