namespace QQBot;

/// <summary>
///     表示服务端分配的一个上传分片。
/// </summary>
internal class MediaUploadPart
{
    /// <summary> 获取分片序号。 </summary>
    public int Index { get; }
    /// <summary> 获取分片长度（字节）。 </summary>
    public long Length { get; }
    /// <summary> 获取用于上传此分片的预签名 URL。 </summary>
    /// <remarks> 此 URL 具有临时上传授权能力，不应记录、持久化或公开。 </remarks>
    public Uri UploadUri { get; }

    internal MediaUploadPart(int index, long length, Uri uploadUri)
    {
        Index = index;
        Length = length;
        UploadUri = uploadUri;
    }
}