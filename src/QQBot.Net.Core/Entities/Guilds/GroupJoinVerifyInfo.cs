namespace QQBot;

/// <summary>
///     表示入群申请人的入群验证信息。
/// </summary>
public class GroupJoinVerifyInfo
{
    /// <summary>
    ///     获取入群验证方式。
    /// </summary>
    public GroupJoinVerifyMethod Method { get; }

    /// <summary>
    ///     获取验证消息内容；仅当验证方式为验证消息时可能存在。
    /// </summary>
    public string? VerifyMessage { get; }

    /// <summary>
    ///     获取管理员审核问答列表；仅当验证方式为管理员审核问答时可能存在。
    /// </summary>
    public IReadOnlyCollection<GroupJoinReviewQuestion> ReviewQuestions { get; }

    internal GroupJoinVerifyInfo(GroupJoinVerifyMethod method, string? verifyMessage,
        IReadOnlyCollection<GroupJoinReviewQuestion> reviewQuestions)
    {
        Method = method;
        VerifyMessage = verifyMessage;
        ReviewQuestions = reviewQuestions;
    }
}
