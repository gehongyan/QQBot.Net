namespace QQBot;

/// <summary>
///     表示服务端下发的分片上传配置。
/// </summary>
internal class MediaUploadConfiguration
{
    /// <summary> 获取服务端允许的最大并发分片数。 </summary>
    public int Concurrency { get; }
    /// <summary> 获取服务端指定的分片重试总时限。 </summary>
    public TimeSpan RetryTimeout { get; }
    /// <summary> 获取服务端指定的分片重试间隔。 </summary>
    public TimeSpan RetryDelay { get; }

    internal MediaUploadConfiguration(int concurrency, TimeSpan retryTimeout, TimeSpan retryDelay)
    {
        Concurrency = concurrency;
        RetryTimeout = retryTimeout;
        RetryDelay = retryDelay;
    }
}