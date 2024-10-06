namespace QQBot;

/// <summary>
///     表示一个富媒体文件信息。
/// </summary>
public readonly record struct MediaFileInfo
{
    /// <summary>
    ///     获取此富媒体文件信息的 ID。
    /// </summary>
    public string? FileId { get; internal init; }

    // /// <summary>
    // ///     获取此富媒体文件信息的网络地址。
    // /// </summary>
    // public Uri? Uri { get; internal init; }

    /// <summary>
    ///     获取此富媒体文件信息的类型。
    /// </summary>
    public required AttachmentType AttachmentType { get; init; }

    /// <summary>
    ///     获取此富媒体文件信息的创建时间。
    /// </summary>
    public DateTimeOffset? CreatedAt { get; internal init; }

    /// <summary>
    ///     获取此富媒体文件信息的存活时间。
    /// </summary>
    public TimeSpan? LifeTime { get; internal init; }

    /// <summary>
    ///     获取此富媒体文件信息的过期时间，如果为 <see langword="null"/> 则表示过期时间未知，或永不过期。
    /// </summary>
    public DateTimeOffset? ExpiresAt =>
        LifeTime == Timeout.InfiniteTimeSpan || LifeTime == TimeSpan.Zero ? null : CreatedAt + LifeTime;

    /// <summary>
    ///     获取一个值，指示此富媒体文件信息是否已经过期；如果为 <see langword="null"/> 则表示过期时间未知，或永不过期。
    /// </summary>
    public bool? HasExpired =>
        LifeTime != Timeout.InfiniteTimeSpan && LifeTime != TimeSpan.Zero
        && DateTimeOffset.UtcNow >= CreatedAt + LifeTime;

    /// <summary>
    ///     获取此富媒体文件信息的文件信息。
    /// </summary>
    public required string FileInfo { get; init; }
}
