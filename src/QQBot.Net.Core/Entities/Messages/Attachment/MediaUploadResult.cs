namespace QQBot;

/// <summary>
///     表示富媒体上传完成后的开发者可用结果。
/// </summary>
public class MediaUploadResult
{
    /// <summary>
    ///     获取可直接传给 <see cref="IMessageChannel.SendMessageAsync"/> 的附件。
    /// </summary>
    public FileAttachment Attachment { get; }

    /// <summary>
    ///     获取上传图片、视频或语音时可能由平台返回的临时下载 URL。
    /// </summary>
    /// <remarks>
    ///     此 URL 具有时效性，不应持久化。图片 URL 可在有效期内用于 Markdown。
    /// </remarks>
    public Uri? DownloadUri { get; }

    internal MediaFileInfo MediaFileInfo { get; }

    internal MediaUploadResult(FileAttachment attachment, MediaFileInfo mediaFileInfo, Uri? downloadUri)
    {
        Attachment = attachment;
        MediaFileInfo = mediaFileInfo;
        DownloadUri = downloadUri;
    }
}