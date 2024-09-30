namespace QQBot;

/// <summary>
///     表示一个消息内的附件。
/// </summary>
public interface IAttachment
{
    /// <summary>
    ///     获取此附件的类型。
    /// </summary>
    AttachmentType Type { get; }

    /// <summary>
    ///     获取此附件的 URL。
    /// </summary>
    string Url { get; }

    /// <summary>
    ///     获取此附件的文本内容。
    /// </summary>
    string? Content { get; }

    /// <summary>
    ///     获取此附件的文件名。
    /// </summary>
    string? Filename { get; }

    /// <summary>
    ///     获取此附件的文件大小。
    /// </summary>
    int? Size { get; }

    /// <summary>
    ///     获取此附件的内容类型。
    /// </summary>
    string? ContentType { get; }

    /// <summary>
    ///     如果此附件表示的内容包含画面，则获取其宽度。
    /// </summary>
    int? Width { get; }

    /// <summary>
    ///     如果此附件表示的内容包含画面，则获取其高度。
    /// </summary>
    int? Height { get; }
}
