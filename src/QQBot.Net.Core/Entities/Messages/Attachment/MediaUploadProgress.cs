namespace QQBot;

/// <summary>
///     表示自动分片上传的已确认进度。
/// </summary>
public class MediaUploadProgress
{
    /// <summary> 获取要上传的总字节数。 </summary>
    public long TotalBytes { get; }
    /// <summary> 获取已完成 PUT 且已被服务端确认的字节数。 </summary>
    public long ConfirmedBytes { get; }
    /// <summary> 获取已被服务端确认的分片数量。 </summary>
    public int ConfirmedPartCount { get; }
    /// <summary> 获取分片总数量。 </summary>
    public int TotalPartCount { get; }

    internal MediaUploadProgress(long totalBytes, long confirmedBytes, int confirmedPartCount, int totalPartCount)
    {
        TotalBytes = totalBytes;
        ConfirmedBytes = confirmedBytes;
        ConfirmedPartCount = confirmedPartCount;
        TotalPartCount = totalPartCount;
    }
}