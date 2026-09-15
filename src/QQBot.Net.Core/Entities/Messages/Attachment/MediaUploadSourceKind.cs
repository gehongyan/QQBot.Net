namespace QQBot;

/// <summary>
///     指定富媒体上传源的类型。
/// </summary>
internal enum MediaUploadSourceKind
{
    /// <summary>
    ///     文件系统路径。
    /// </summary>
    FilePath = 0,

    /// <summary>
    ///     可定位的流。
    /// </summary>
    Stream = 1,

    /// <summary>
    ///     内存中的二进制数据。
    /// </summary>
    Memory = 2
}