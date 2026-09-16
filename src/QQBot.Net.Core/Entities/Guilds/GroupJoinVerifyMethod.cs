namespace QQBot;

/// <summary>
///     表示入群申请的验证方式。
/// </summary>
public enum GroupJoinVerifyMethod
{
    /// <summary>
    ///     通过验证消息进行验证。
    /// </summary>
    VerifyMessage,

    /// <summary>
    ///     通过管理员审核问答进行验证。
    /// </summary>
    AdminReviewQa
}
