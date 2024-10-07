namespace QQBot;

/// <summary>
///     表示一个通用的 Markdown 内容构建器
/// </summary>
public interface IMarkdownContentBuilder
{
    /// <summary>
    ///     将当前构建器构建为一个 <see cref="IMarkdownContent"/> 实例。
    /// </summary>
    /// <returns> <see cref="IMarkdownContent"/> 实例。 </returns>
    IMarkdownContent Build();
}