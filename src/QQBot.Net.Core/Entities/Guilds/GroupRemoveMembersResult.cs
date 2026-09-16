using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示批量移除群成员的结果。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class GroupRemoveMembersResult
{
    /// <summary>
    ///     获取成员移除是否成功。
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    ///     获取在移除时同步加入群黑名单失败的成员标识符。
    /// </summary>
    public IReadOnlyCollection<Guid> BlacklistFailedMemberIds { get; }

    internal GroupRemoveMembersResult(bool isSuccess, IReadOnlyCollection<Guid> blacklistFailedMemberIds)
    {
        IsSuccess = isSuccess;
        BlacklistFailedMemberIds = blacklistFailedMemberIds;
    }

    private string DebuggerDisplay =>
        $"{(IsSuccess ? "Success" : "Failed")} ({BlacklistFailedMemberIds.Count} Blacklist Failures)";
}
