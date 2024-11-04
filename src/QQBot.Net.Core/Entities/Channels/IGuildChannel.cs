namespace QQBot;

/// <summary>
///     表示一个子频道内的子频道。
/// </summary>
public interface IGuildChannel : IChannel, IUpdateable, IEntity<ulong>
{
    /// <summary>
    ///     获取此子频道的唯一标识符。
    /// </summary>
    new ulong Id { get; }

    /// <summary>
    ///     获取此子频道所属的频道。
    /// </summary>
    IGuild Guild { get; }

    /// <summary>
    ///     获取此子频道的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取与此子频道所属的频道的 ID。
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
    ///     获取创建此子频道的用户的 ID。
    /// </summary>
    ulong? CreatorId { get; }

    /// <inheritdoc cref="QQBot.IChannel.GetUserAsync(System.String,QQBot.CacheMode,QQBot.RequestOptions)" />
    Task<IGuildUser?> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);
}
