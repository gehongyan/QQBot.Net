namespace QQBot;

/// <summary>
///     表示预上传时需要的文件元数据和校验值。
/// </summary>
internal class MediaUploadDescriptor
{
    /// <summary> 获取富媒体类型。 </summary>
    public AttachmentType Type { get; }
    /// <summary> 获取文件名。 </summary>
    public string FileName { get; }
    /// <summary> 获取文件长度（字节）。 </summary>
    public long Length { get; }
    /// <summary> 获取完整文件的 MD5 校验值。 </summary>
    public string Md5 { get; }
    /// <summary> 获取完整文件的 SHA1 校验值。 </summary>
    public string Sha1 { get; }
    /// <summary> 获取文件前 10002432 字节的 MD5 校验值。 </summary>
    public string Md5First10MiB { get; }

    /// <summary>
    ///     初始化一个 <see cref="MediaUploadDescriptor"/> 类的新实例。
    /// </summary>
    /// <param name="type"> 富媒体类型。 </param>
    /// <param name="fileName"> 文件名。 </param>
    /// <param name="length"> 文件长度（字节）。 </param>
    /// <param name="md5"> 完整文件的 MD5 校验值。 </param>
    /// <param name="sha1"> 完整文件的 SHA1 校验值。 </param>
    /// <param name="md5First10MiB"> 文件前 10002432 字节的 MD5 校验值。 </param>
    public MediaUploadDescriptor(AttachmentType type, string fileName, long length,
        string md5, string sha1, string md5First10MiB)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        ArgumentException.ThrowIfNullOrWhiteSpace(md5);
        ArgumentException.ThrowIfNullOrWhiteSpace(sha1);
        ArgumentException.ThrowIfNullOrWhiteSpace(md5First10MiB);
        Type = type;
        FileName = fileName;
        Length = length;
        Md5 = md5;
        Sha1 = sha1;
        Md5First10MiB = md5First10MiB;
    }
}