using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一条 QQ 群入群申请。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class GroupJoinRequest
{
    /// <summary>
    ///     获取此申请的唯一标识符。
    /// </summary>
    public string Id { get; }

    /// <summary>
    ///     获取申请人的用户标识符。
    /// </summary>
    public Guid MemberId { get; }

    /// <summary>
    ///     获取申请人的昵称。
    /// </summary>
    public string Username { get; }

    /// <summary>
    ///     获取申请人关联的互联应用统一标识；若无则为 <see langword="null"/>。
    /// </summary>
    public string? UnionOpenId { get; }

    /// <summary>
    ///     获取申请人是否为机器人。
    /// </summary>
    public bool IsBot { get; }

    /// <summary>
    ///     获取此申请的提交时间。
    /// </summary>
    public DateTimeOffset AppliedAt { get; }

    /// <summary>
    ///     获取此申请的来源。
    /// </summary>
    public GroupJoinSource Source { get; }

    /// <summary>
    ///     获取邀请人的用户标识符；仅当来源为受邀时有效，否则为 <see langword="null"/>。
    /// </summary>
    public Guid? InvitedBy { get; }

    /// <summary>
    ///     获取此申请的安全风险提示；若无则为 <see langword="null"/>。
    /// </summary>
    public string? RiskTips { get; }

    /// <summary>
    ///     获取申请人的入群验证信息；若无则为 <see langword="null"/>。
    /// </summary>
    public GroupJoinVerifyInfo? VerifyInfo { get; }

    internal GroupJoinRequest(string id, Guid memberId, string username, string? unionOpenId,
        bool isBot, DateTimeOffset appliedAt, GroupJoinSource source, Guid? invitedBy,
        string? riskTips, GroupJoinVerifyInfo? verifyInfo)
    {
        Id = id;
        MemberId = memberId;
        Username = username;
        UnionOpenId = unionOpenId;
        IsBot = isBot;
        AppliedAt = appliedAt;
        Source = source;
        InvitedBy = invitedBy;
        RiskTips = riskTips;
        VerifyInfo = verifyInfo;
    }

    private string DebuggerDisplay => $"{Username} ({MemberId}, {Source})";
}
