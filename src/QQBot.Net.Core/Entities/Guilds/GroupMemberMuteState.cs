namespace QQBot;

/// <summary>
///     表示一个当前处于禁言状态的群成员。
/// </summary>
public class GroupMemberMuteState
{
    /// <summary>
    ///     获取被禁言成员的标识符。
    /// </summary>
    public Guid MemberId { get; }

    /// <summary>
    ///     获取禁言到期时间。
    /// </summary>
    public DateTimeOffset ExpiresAt { get; }

    /// <summary>
    ///     获取被禁言成员的昵称。
    /// </summary>
    public string Username { get; }

    /// <summary>
    ///     获取被禁言成员关联的互联应用统一标识；若无则为 <see langword="null"/>。
    /// </summary>
    public string? UnionOpenId { get; }

    internal GroupMemberMuteState(Guid memberId, DateTimeOffset expiresAt, string username, string? unionOpenId)
    {
        MemberId = memberId;
        ExpiresAt = expiresAt;
        Username = username;
        UnionOpenId = unionOpenId;
    }
}
