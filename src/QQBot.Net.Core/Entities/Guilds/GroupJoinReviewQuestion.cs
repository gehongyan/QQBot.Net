namespace QQBot;

/// <summary>
///     表示入群申请中管理员设置的一条审核问答。
/// </summary>
public class GroupJoinReviewQuestion
{
    /// <summary>
    ///     获取管理员设置的问题。
    /// </summary>
    public string Question { get; }

    /// <summary>
    ///     获取申请人填写的答案。
    /// </summary>
    public string Answer { get; }

    internal GroupJoinReviewQuestion(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }
}
