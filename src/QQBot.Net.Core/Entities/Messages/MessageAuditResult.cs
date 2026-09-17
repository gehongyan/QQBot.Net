namespace QQBot;

/// <summary>
///     表示消息审核的结果。
/// </summary>
public enum MessageAuditResult
{
    /// <summary>
    ///     消息审核通过，已发出。
    /// </summary>
    Passed,

    /// <summary>
    ///     消息审核未通过。
    /// </summary>
    Rejected
}
