using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个 QQ 群黑名单中的用户。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class GroupBlacklistUser
{
    /// <summary>
    ///     获取此用户的标识符。
    /// </summary>
    public Guid MemberId { get; }

    /// <summary>
    ///     获取此用户的昵称。
    /// </summary>
    public string Username { get; }

    /// <summary>
    ///     获取此用户被加入黑名单的时间。
    /// </summary>
    public DateTimeOffset BannedAt { get; }

    /// <summary>
    ///     获取此用户是否为机器人。
    /// </summary>
    public bool IsBot { get; }

    /// <summary>
    ///     获取此用户关联的互联应用统一标识；若无则为 <see langword="null"/>。
    /// </summary>
    public string? UnionOpenId { get; }

    internal GroupBlacklistUser(Guid memberId, string username, DateTimeOffset bannedAt,
        bool isBot, string? unionOpenId)
    {
        MemberId = memberId;
        Username = username;
        BannedAt = bannedAt;
        IsBot = isBot;
        UnionOpenId = unionOpenId;
    }

    private string DebuggerDisplay => $"{Username} ({MemberId}, Blacklisted)";
}
