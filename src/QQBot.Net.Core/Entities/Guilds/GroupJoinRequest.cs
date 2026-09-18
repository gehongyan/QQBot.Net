using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一条 QQ 群入群申请。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class GroupJoinRequest : IEntity<string>
{
    /// <summary>
    ///     获取此申请所属的群组子频道。
    /// </summary>
    public IGroupChannel Channel { get; }

    /// <summary>
    ///     获取此申请所属群的标识符。
    /// </summary>
    public Guid GroupId => Channel.Id;

    /// <inheritdoc />
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

    internal GroupJoinRequest(IGroupChannel channel, string id, Guid memberId,
        string username, string? unionOpenId, bool isBot, DateTimeOffset appliedAt,
        GroupJoinSource source, Guid? invitedBy, string? riskTips, GroupJoinVerifyInfo? verifyInfo)
    {
        Channel = channel;
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

    /// <summary>
    ///     通过此入群申请。
    /// </summary>
    /// <remarks>
    ///     机器人需拥有群管理员身份。
    /// </remarks>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步审批操作的任务。 </returns>
    public Task ApproveAsync(RequestOptions? options = null) =>
        Channel.ApproveJoinRequestAsync(this, options);

    /// <summary>
    ///     拒绝此入群申请。
    /// </summary>
    /// <remarks>
    ///     机器人需拥有群管理员身份。
    /// </remarks>
    /// <param name="reason"> 拒绝理由。 </param>
    /// <param name="addToBlacklist"> 是否在拒绝的同时将申请人加入群黑名单。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步审批操作的任务。 </returns>
    public Task DeclineAsync(string? reason = null, bool addToBlacklist = false, RequestOptions? options = null) =>
        Channel.DeclineJoinRequestAsync(this, reason, addToBlacklist, options);

    private string DebuggerDisplay => $"{Username} ({MemberId}, {Source})";
}
