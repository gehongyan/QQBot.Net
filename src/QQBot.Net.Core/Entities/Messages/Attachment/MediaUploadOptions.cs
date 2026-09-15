namespace QQBot;

/// <summary>
///     配置自动分片上传行为。
/// </summary>
public class MediaUploadOptions
{
    /// <summary>
    ///     获取或设置允许的最大并发分片数。
    /// </summary>
    /// <remarks> 实际并发数不会超过服务端下发的限制。 </remarks>
    public int? MaximumConcurrency { get; set; }
}