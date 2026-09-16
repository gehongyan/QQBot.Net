using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示群的禁言状态，包含全员禁言规则与当前被禁言的成员列表。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class GroupMuteSetting
{
    /// <summary>
    ///     获取此群的全员禁言规则。
    /// </summary>
    public GroupGlobalMuteRule GlobalRule { get; }

    /// <summary>
    ///     获取当前处于禁言状态的成员列表（不含已过期）。
    /// </summary>
    public IReadOnlyCollection<GroupMemberMuteState> MutedMembers { get; }

    internal GroupMuteSetting(GroupGlobalMuteRule globalRule,
        IReadOnlyCollection<GroupMemberMuteState> mutedMembers)
    {
        GlobalRule = globalRule;
        MutedMembers = mutedMembers;
    }

    private string DebuggerDisplay => $"{GlobalRule.Mode} ({MutedMembers.Count} Muted Members)";
}
