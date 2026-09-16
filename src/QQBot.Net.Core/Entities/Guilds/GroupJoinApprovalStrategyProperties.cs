namespace QQBot;

/// <summary>
///     提供用于创建入群自动审批策略的属性。
/// </summary>
public class GroupJoinApprovalStrategyProperties
{
    /// <summary>
    ///     获取或设置是否启用此策略。默认为 <see langword="true"/>。
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    ///     获取或设置此策略的过期时间；若不设置则默认一年后过期。
    /// </summary>
    public DateTimeOffset? ExpiresAt { get; set; }

    /// <summary>
    ///     获取或设置此策略的备注，最多 255 个汉字。
    /// </summary>
    public string? Remark { get; set; }
}
