namespace QQBot;

/// <summary>
///     表示一个已上传分片的确认信息。
/// </summary>
internal class MediaUploadPartCompletion
{
    /// <summary> 获取分片序号。 </summary>
    public int Index { get; }
    /// <summary> 获取该分片实际长度（字节）。 </summary>
    public long Length { get; }
    /// <summary> 获取该分片的 MD5 校验值。 </summary>
    public string Md5 { get; }

    /// <summary>
    ///     初始化一个 <see cref="MediaUploadPartCompletion"/> 类的新实例。
    /// </summary>
    /// <param name="index"> 分片序号。 </param>
    /// <param name="length"> 该分片实际长度（字节）。 </param>
    /// <param name="md5"> 该分片的 MD5 校验值。 </param>
    public MediaUploadPartCompletion(int index, long length, string md5)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        ArgumentException.ThrowIfNullOrWhiteSpace(md5);
        Index = index;
        Length = length;
        Md5 = md5;
    }
}