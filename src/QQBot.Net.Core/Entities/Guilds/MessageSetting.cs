namespace QQBot;

/// <summary>
///     表示一个频道的消息设置。
/// </summary>
public readonly struct MessageSetting
{
    /// <summary>
    ///     获取是否允许创建私信。
    /// </summary>
    public bool AllowDirectMessage { get; init; }

    /// <summary>
    ///     获取是否允许发主动消息
    /// </summary>
    public bool AllowPushMessage { get; init; }

    /// <summary>
    ///     获取所有子频道的 ID。
    /// </summary>
    public IReadOnlyCollection<ulong> ChannelIds { get; init; }

    /// <summary>
    ///     每个子频道允许主动推送消息最大消息条数。
    /// </summary>
    public uint MaxPushMessagesPerChannel { get; init; }

    internal MessageSetting(bool allowDirectMessage, bool allowPushMessage,
        IReadOnlyCollection<ulong> channelIds, uint maxPushMessagesPerChannel)
    {
        AllowDirectMessage = allowDirectMessage;
        AllowPushMessage = allowPushMessage;
        ChannelIds = channelIds;
        MaxPushMessagesPerChannel = maxPushMessagesPerChannel;
    }
}
