namespace QQBot;

/// <summary>
///     表示已准备好的分片上传会话。
/// </summary>
internal interface IMediaUploadSession
{
    /// <summary> 获取服务端分配的上传任务 ID。 </summary>
    string UploadId { get; }
    /// <summary> 获取服务端建议的默认分块大小（字节）。 </summary>
    long BlockSize { get; }
    /// <summary> 获取所有待上传分片及其临时上传 URL。 </summary>
    IReadOnlyList<MediaUploadPart> Parts { get; }
    /// <summary> 获取服务端下发的上传并发与重试配置。 </summary>
    MediaUploadConfiguration Configuration { get; }

    /// <summary> 通知服务端一个已完成 PUT 的分片。 </summary>
    /// <param name="completion"> 已上传分片的确认信息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    Task ConfirmPartAsync(MediaUploadPartCompletion completion, RequestOptions? options = null);

    /// <summary> 合并全部已确认分片并创建富媒体文件。 </summary>
    /// <param name="completionOptions"> 最终合并配置。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 上传结果。 </returns>
    Task<MediaUploadResult> CompleteAsync(MediaUploadCompletionOptions? completionOptions = null,
        RequestOptions? options = null);
}