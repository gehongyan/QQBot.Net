namespace QQBot;

/// <summary>
///     表示一个子频道内用户
/// </summary>
public interface IGuildMember : IGuildUser
{
    /// <summary>
    ///     获取此频道用户所属的频道。
    /// </summary>
    IGuild Guild { get; }

    /// <summary>
    ///     获取此用户所属频道的 ID。
    /// </summary>
    ulong GuildId { get; }

    /// <summary>
    ///     获取此用户的昵称。
    /// </summary>
    string? Nickname { get; }

    /// <summary>
    ///     获取此用户在该子频道内拥有的所有身份组的 ID。
    /// </summary>
    IReadOnlyCollection<uint>? RoleIds { get; }

    /// <summary>
    ///     获取此用户加入该子频道的时间。
    /// </summary>
    DateTimeOffset? JoinedAt { get; }
}
