namespace QQBot;

/// <summary>
///     表示富媒体上传的数据源。
/// </summary>
public class MediaUploadSource
{
    /// <summary>
    ///     获取上传源类型。
    /// </summary>
    internal MediaUploadSourceKind Kind { get; }

    /// <summary>
    ///     获取富媒体类型。
    /// </summary>
    public AttachmentType Type { get; }

    /// <summary>
    ///     获取上传时使用的文件名。
    /// </summary>
    public string FileName { get; }

    /// <summary>
    ///     获取文件路径来源的路径。
    /// </summary>
    internal string? FilePath { get; }

    /// <summary>
    ///     获取流来源的流。
    /// </summary>
    /// <remarks>
    ///     QQBot.Net 不拥有或关闭此流。流必须可读且可定位。
    /// </remarks>
    internal Stream? Stream { get; }

    /// <summary>
    ///     获取内存来源的数据。
    /// </summary>
    internal ReadOnlyMemory<byte> Memory { get; }

    private MediaUploadSource(MediaUploadSourceKind kind, AttachmentType type, string fileName,
        string? filePath, Stream? stream, ReadOnlyMemory<byte> memory)
    {
        Kind = kind;
        Type = type;
        FileName = fileName;
        FilePath = filePath;
        Stream = stream;
        Memory = memory;
    }

    /// <summary>
    ///     从文件路径创建上传源。
    /// </summary>
    public static MediaUploadSource FromFile(string filePath, AttachmentType type = AttachmentType.Image,
        string? fileName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        string effectiveFileName = fileName ?? Path.GetFileName(filePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(effectiveFileName);
        return new MediaUploadSource(MediaUploadSourceKind.FilePath, type, effectiveFileName,
            filePath, null, default);
    }

    /// <summary>
    ///     从可定位流创建上传源。
    /// </summary>
    public static MediaUploadSource FromStream(Stream stream, string fileName,
        AttachmentType type = AttachmentType.Image)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        if (!stream.CanRead || !stream.CanSeek)
            throw new ArgumentException("The upload stream must be readable and seekable.", nameof(stream));
        return new MediaUploadSource(MediaUploadSourceKind.Stream, type, fileName,
            null, stream, default);
    }

    /// <summary>
    ///     从内存数据创建上传源。
    /// </summary>
    public static MediaUploadSource FromMemory(ReadOnlyMemory<byte> memory, string fileName,
        AttachmentType type = AttachmentType.Image)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        return new MediaUploadSource(MediaUploadSourceKind.Memory, type, fileName,
            null, null, memory);
    }
}
