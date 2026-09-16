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

    /// <summary>
    ///     禁言此成员。
    /// </summary>
    /// <remarks>
    ///     机器人需拥有群管理员身份，最大禁言时长为 30 天，且仅可禁言普通成员。
    /// </remarks>
    /// <param name="expiresAt"> 禁言到期时间。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步禁言操作的任务。 </returns>
    Task MuteAsync(DateTimeOffset expiresAt, RequestOptions? options = null);

    /// <summary>
    ///     禁言此成员。
    /// </summary>
    /// <remarks>
    ///     机器人需拥有群管理员身份，最大禁言时长为 30 天，且仅可禁言普通成员。
    /// </remarks>
    /// <param name="duration"> 自当前时间起的禁言时长。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步禁言操作的任务。 </returns>
    Task MuteAsync(TimeSpan duration, RequestOptions? options = null);

    /// <summary>
    ///     解除此成员的禁言。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步解除禁言操作的任务。 </returns>
    Task UnmuteAsync(RequestOptions? options = null);
}
