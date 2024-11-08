namespace QQBot;

/// <summary>
///     表示子频道类型。
/// </summary>
public enum ChannelType
{
    /// <summary>
    ///     未指定。
    /// </summary>
    Unspecified = -1,

    /// <summary>
    ///     文字子频道。
    /// </summary>
    Text = 0,

    /// <summary>
    ///     语音子频道。
    /// </summary>
    Voice = 2,

    /// <summary>
    ///     分组子频道。
    /// </summary>
    Category = 4,

    /// <summary>
    ///     直播子频道。
    /// </summary>
    LiveStream = 10005,

    /// <summary>
    ///     应用子频道。
    /// </summary>
    Application = 10006,

    /// <summary>
    ///     论坛子频道。
    /// </summary>
    Forum = 10007,

    /// <summary>
    ///     日程子频道。
    /// </summary>
    Schedule = 10011,
}
