namespace QQBot;

/// <summary>
///     表示一个空元素的构建器。
/// </summary>
public class EmptyElementBuilder : IElementBuilder
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Empty;
}
