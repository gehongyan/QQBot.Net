namespace QQBot;

/// <summary>
///     表示支持上传富媒体文件的消息子频道。
/// </summary>
public interface IMediaUploadChannel : IChannel
{
    /// <summary>
    ///     自动上传一个本地富媒体文件。
    /// </summary>
    /// <remarks>
    ///     此方法会计算校验值、上传并确认所有分片、完成合并。分片上传细节由 QQBot.Net 按服务端
    ///     下发的并发和重试配置管理，不会直接发送消息。
    /// </remarks>
    /// <param name="source"> 上传源。 </param>
    /// <param name="progress"> 已确认分片的进度接收器。 </param>
    /// <param name="uploadOptions"> 自动上传配置。 </param>
    /// <param name="options"> 发送请求时要使用的选项；上传控制面请求的超时小于 5 秒时会自动提升至 5 秒，其中的取消令牌也适用于哈希和分片上传。 </param>
    /// <returns> 可直接发送的附件及可选临时下载 URL。 </returns>
    Task<MediaUploadResult> UploadMediaAsync(MediaUploadSource source,
        IProgress<MediaUploadProgress>? progress = null, MediaUploadOptions? uploadOptions = null,
        RequestOptions? options = null);
}
