namespace QQBot;

/// <summary>
///     表示一个通用的 Markdown 内容构建器
/// </summary>
public interface IMarkdownBuilder
{
    /// <summary>
    ///     将当前构建器构建为一个 <see cref="IMarkdown"/> 实例。
    /// </summary>
    /// <returns> <see cref="IMarkdown"/> 实例。 </returns>
    IMarkdown Build();
}
