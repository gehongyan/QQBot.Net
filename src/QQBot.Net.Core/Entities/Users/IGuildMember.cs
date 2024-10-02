namespace QQBot;

/// <summary>
///     表示一个频道内用户
/// </summary>
public interface IGuildMember : IGuildUser
{
    /// <summary>
    ///     获取此用户的昵称。
    /// </summary>
    string? Nickname { get; }

    /// <summary>
    ///     获取此用户在该频道内拥有的所有身份组的 ID。
    /// </summary>
    IReadOnlyCollection<uint>? RoleIds { get; }

    /// <summary>
    ///     获取此用户加入该频道的时间。
    /// </summary>
    DateTimeOffset JoinedAt { get; }
}
