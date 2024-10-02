using System.Collections.Immutable;

namespace QQBot;

/// <summary>
///     表示一个通用的嵌入式消息。
/// </summary>
public interface IEmbed
{
    /// <summary>
    ///     获取此嵌入式消息的标题。
    /// </summary>
    string? Title { get; }

    /// <summary>
    ///     获取此嵌入式消息的弹窗内容。
    /// </summary>
    string? Prompt { get; }

    /// <summary>
    ///     获取此嵌入式消息的缩略图。
    /// </summary>
    EmbedThumbnail? Thumbnail { get; }

    /// <summary>
    ///     获取此嵌入式消息的字段。
    /// </summary>
    ImmutableArray<EmbedField> Fields { get; }
}
