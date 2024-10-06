namespace QQBot;

/// <summary>
///     表示一个文件附件。
/// </summary>
public struct FileAttachment : IDisposable
{
    /// <summary>
    ///     获取创建此文件附件的方式。
    /// </summary>
    public CreateAttachmentMode Mode { get; }

    /// <summary>
    ///     获取此附件的类型。
    /// </summary>
    public AttachmentType Type { get; }

    /// <summary>
    ///     获取此附件的文件路径。
    /// </summary>
    public string? FilePath { get; }

    /// <summary>
    ///     获取此附件的文件名。
    /// </summary>
    public string? Filename { get; }

    /// <summary>
    ///     获取包含文件内容的流。
    /// </summary>
    public Stream? Stream { get; }

    /// <summary>
    ///     获取指向文件的 URL。
    /// </summary>
    public Uri? Uri { get; set; }

    /// <summary>
    ///     获取用于单聊的富媒体文件信息。
    /// </summary>
    public MediaFileInfo? UserMediaFileInfo { get; internal set; }

    /// <summary>
    ///     获取用于群聊的富媒体文件信息。
    /// </summary>
    public MediaFileInfo? GroupMediaFileInfo { get; internal set; }

    /// <summary>
    ///     通过流创建附件。
    /// </summary>
    /// <param name="stream"> 创建附件所使用的流。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 附件的类型。 </param>
    public FileAttachment(Stream stream, string? filename = null, AttachmentType type = AttachmentType.Image)
    {
        IsDisposed = false;
        Mode = CreateAttachmentMode.Stream;
        Type = type;
        FilePath = null;
        Filename = filename;
        Stream = stream;
        Uri = null;
        UserMediaFileInfo = null;
        GroupMediaFileInfo = null;
        try
        {
            Stream.Position = 0;
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    ///     通过文件路径创建附件。
    /// </summary>
    /// <param name="filePath"> 文件的路径。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 附件的类型。 </param>
    /// <remarks>
    ///     此构造函数不会校验文件路径的格式，<paramref name="filePath"/> 的值将会直接传递给
    ///     <see cref="System.IO.File.OpenRead(System.String)"/> 方法。
    /// </remarks>
    /// <seealso cref="System.IO.File.OpenRead(System.String)"/>
    public FileAttachment(string filePath, string? filename = null, AttachmentType type = AttachmentType.Image)
    {
        IsDisposed = false;
        Mode = CreateAttachmentMode.FilePath;
        Type = type;
        FilePath = filePath;
        Stream = null;
        Filename = filename ?? Path.GetFileName(filePath);
        Uri = null;
        UserMediaFileInfo = null;
        GroupMediaFileInfo = null;
    }

    /// <summary>
    ///     通过 URL 创建附件。
    /// </summary>
    /// <param name="uri"> 文件的 URL。 </param>
    /// <param name="filename"> 文件名。 </param>
    /// <param name="type"> 附件的类型。 </param>
    /// <seealso cref="QQBot.UrlValidation.Validate(System.String)"/>
    public FileAttachment(Uri uri, string? filename = null, AttachmentType type = AttachmentType.Image)
    {
        IsDisposed = false;
        Mode = CreateAttachmentMode.Uri;
        Type = type;
        FilePath = null;
        Stream = null;
        Filename = filename;
        Uri = uri;
        UserMediaFileInfo = null;
        GroupMediaFileInfo = null;
    }

    /// <summary>
    ///     通过富媒体文件信息创建附件。
    /// </summary>
    /// <param name="userMediaFileInfo"> 用于单聊的富媒体文件信息。 </param>
    /// <param name="groupMediaFileInfo"> 用于群聊的富媒体文件信息。 </param>
    /// <param name="filename"> 文件名。 </param>
    public FileAttachment(MediaFileInfo? userMediaFileInfo, MediaFileInfo? groupMediaFileInfo, string? filename = null)
    {
        if (!userMediaFileInfo.HasValue && !groupMediaFileInfo.HasValue)
            throw new ArgumentException("At least one of the media file info must be provided.");
        if (userMediaFileInfo.HasValue && groupMediaFileInfo.HasValue
            && userMediaFileInfo.Value.AttachmentType != groupMediaFileInfo.Value.AttachmentType)
            throw new ArgumentException("The attachment type of the media file info must be the same if both are provided.");
        if (userMediaFileInfo is { HasExpired: true })
            throw new ArgumentException("The user media file info has expired.");
        if (groupMediaFileInfo is { HasExpired: true })
            throw new ArgumentException("The group media file info has expired.");
        IsDisposed = false;
        Mode = CreateAttachmentMode.MediaFileInfo;
        // It's safe to use the value property here because the above checks have ensured that at least one of them has a value.
        Type = userMediaFileInfo?.AttachmentType ?? groupMediaFileInfo!.Value.AttachmentType;
        FilePath = null;
        Stream = null;
        Filename = filename;
        Uri = null;
        UserMediaFileInfo = userMediaFileInfo;
        GroupMediaFileInfo = groupMediaFileInfo;
    }

    internal bool IsDisposed { get; private set; }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!IsDisposed)
        {
            Stream?.Dispose();
            IsDisposed = true;
        }
    }
}
