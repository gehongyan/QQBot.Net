namespace QQBot;

/// <summary>
///     指示 <see cref="QQBot.FileAttachment"/> 如何创建附件。
/// </summary>
public enum CreateAttachmentMode
{
    /// <summary>
    ///     通过本地文件路径创建附件。
    /// </summary>
    FilePath,

    /// <summary>
    ///     通过 <see cref="System.IO.Stream"/> 流的实例创建附件。
    /// </summary>
    Stream,

    /// <summary>
    ///     通过网络地址创建附件。
    /// </summary>
    Uri,

    /// <summary>
    ///     通过富媒体文件信息创建附件。
    /// </summary>
    MediaFileInfo
}
