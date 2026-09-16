namespace QQBot;

/// <summary>
///     表示一个 QQ 群内的成员。
/// </summary>
public interface IGroupMember : IUser
{
    /// <summary>
    ///     获取此成员的唯一标识符。
    /// </summary>
    new Guid Id { get; }

    /// <summary>
    ///     获取此成员的昵称。
    /// </summary>
    string Username { get; }

    /// <summary>
    ///     获取此成员在群内的角色。
    /// </summary>
    GroupMemberRole Role { get; }

    /// <summary>
    ///     获取此成员是否为机器人。
    /// </summary>
    bool IsBot { get; }

    /// <summary>
    ///     获取此成员加入群的时间。
    /// </summary>
    DateTimeOffset JoinedAt { get; }

    /// <summary>
    ///     获取此成员机器人关联的互联应用的用户信息。
    /// </summary>
    /// <remarks>
    ///     此字段需要特殊申请并配置后才会返回。如需申请，请联系平台运营人员。
    /// </remarks>
    string? UnionOpenId { get; }
}
