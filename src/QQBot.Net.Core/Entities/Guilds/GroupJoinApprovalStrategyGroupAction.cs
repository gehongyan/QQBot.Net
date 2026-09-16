namespace QQBot;

/// <summary>
///     表示对入群自动审批策略所关联群的一次增删操作。
/// </summary>
public class GroupJoinApprovalStrategyGroupAction
{
    /// <summary>
    ///     获取或设置要操作的群的标识符列表；与 <see cref="GroupNumbers"/> 互斥。
    /// </summary>
    public IEnumerable<Guid>? GroupIds { get; set; }

    /// <summary>
    ///     获取或设置要操作的 QQ 群号列表；与 <see cref="GroupIds"/> 互斥。
    /// </summary>
    public IEnumerable<ulong>? GroupNumbers { get; set; }
}
