namespace QQBot;

/// <summary>
///     表示一个频道内的子频道。
/// </summary>
public interface IGuildChannel : IChannel
{
    /// <summary>
    ///     获取此频道所属的服务器。
    /// </summary>
    IGuild Guild { get; }

    /// <summary>
    ///     获取与此频道所属的服务器的 ID。
    /// </summary>
    ulong GuildId { get; }

    /// <summary>
    ///     获取此子频道的类型。
    /// </summary>
    ChannelType Type { get; }

    /// <summary>
    ///     获取此子频道在子频道列表中的位置。
    /// </summary>
    /// <remarks>
    ///     更小的数值表示更靠近列表顶部的位置。
    /// </remarks>
    int Position { get; }

    /// <summary>
    ///     获取创建此频道的用户的 ID。
    /// </summary>
    ulong? CreatorId { get; }
}
