namespace QQBot;

/// <summary>
///     表示消息的来源标识符。
/// </summary>
public readonly struct MessageSourceIdentifier
{
    /// <summary>
    ///     表示下发消息所属的事件 ID。
    /// </summary>
    public string? Dispatch { get; init; }

    /// <summary>
    ///     表示消息的 ID。
    /// </summary>
    public string? MessageId { get; init; }
}
