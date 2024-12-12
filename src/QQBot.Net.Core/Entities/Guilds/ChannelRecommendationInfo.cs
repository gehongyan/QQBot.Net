namespace QQBot;

/// <summary>
///     表示一个频道推荐。
/// </summary>
public class ChannelRecommendation
{
    /// <summary>
    ///     获取要推荐的频道的 ID。
    /// </summary>
    public ulong ChannelId { get; init; }

    /// <summary>
    ///     获取推荐语。
    /// </summary>
    public string Introduction { get; init; }

    /// <summary>
    ///     初始化一个类 <see cref="ChannelRecommendation"/> 的新实例。
    /// </summary>
    /// <param name="channelId"> 要推荐的频道的 ID。 </param>
    /// <param name="introduction"> 推荐语。 </param>
    public ChannelRecommendation(ulong channelId, string introduction)
    {
        ChannelId = channelId;
        Introduction = introduction;
    }

    /// <summary>
    ///     初始化一个类 <see cref="ChannelRecommendation"/> 的新实例。
    /// </summary>
    /// <param name="channel"> 要推荐的频道。 </param>
    /// <param name="introduction"> 推荐语。 </param>
    public ChannelRecommendation(IGuildChannel channel, string introduction)
        : this(channel.Id, introduction)
    {
    }
}
