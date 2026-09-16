namespace QQBot;

/// <summary>
///     提供用于修改入群自动审批策略的属性。
/// </summary>
public class ModifyGroupJoinApprovalStrategyProperties
{
    /// <summary>
    ///     获取或设置是否启用此策略；若为 <see langword="null"/> 则不修改。
    /// </summary>
    public bool? IsEnabled { get; set; }

    /// <summary>
    ///     获取或设置此策略的过期时间；若为 <see langword="null"/> 则不修改。
    /// </summary>
    public DateTimeOffset? ExpiresAt { get; set; }

    /// <summary>
    ///     获取或设置此策略的备注，最多 255 个汉字；若为 <see langword="null"/> 则不修改。
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    ///     获取或设置要新增关联的群；若为 <see langword="null"/> 则不新增。群标识形式须与创建时一致。
    /// </summary>
    public GroupJoinApprovalStrategyGroupAction? AddGroups { get; set; }

    /// <summary>
    ///     获取或设置要移除关联的群；若为 <see langword="null"/> 则不移除。群标识形式须与创建时一致。
    /// </summary>
    public GroupJoinApprovalStrategyGroupAction? RemoveGroups { get; set; }
}
