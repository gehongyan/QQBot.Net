using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的论坛发表内容审核结果。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketForumAuditResult
{
    /// <summary>
    ///     获取此审核结果所属的频道。
    /// </summary>
    public SocketGuild Guild { get; }

    /// <summary>
    ///     获取此审核结果所属的论坛子频道。
    /// </summary>
    public SocketForumChannel Channel { get; }

    /// <summary>
    ///     获取发表被审核内容的作者用户的 ID。
    /// </summary>
    public ulong AuthorId { get; }

    /// <summary>
    ///     获取此次审核所针对的对象类型。
    /// </summary>
    public ForumAuditType Type { get; }

    /// <summary>
    ///     获取被审核主题的 ID。
    /// </summary>
    /// <remarks>
    ///     当 <see cref="Type"/> 为 <see cref="QQBot.ForumAuditType.Thread"/>、<see cref="QQBot.ForumAuditType.Post"/>
    ///     或 <see cref="QQBot.ForumAuditType.Reply"/> 时，此属性表示被审核内容所属的主题的 ID；平台未提供时为 <see langword="null"/>。
    /// </remarks>
    public string? ThreadId { get; }

    /// <summary>
    ///     获取被审核主题评论的 ID。
    /// </summary>
    /// <remarks>
    ///     当 <see cref="Type"/> 为 <see cref="QQBot.ForumAuditType.Post"/> 或 <see cref="QQBot.ForumAuditType.Reply"/>
    ///     时，此属性表示被审核内容所属的主题评论的 ID；平台未提供时为 <see langword="null"/>。
    /// </remarks>
    public string? PostId { get; }

    /// <summary>
    ///     获取被审核主题评论回复的 ID。
    /// </summary>
    /// <remarks>
    ///     当 <see cref="Type"/> 为 <see cref="QQBot.ForumAuditType.Reply"/> 时，此属性表示被审核回复的 ID；平台未提供时为 <see langword="null"/>。
    /// </remarks>
    public string? ReplyId { get; }

    /// <summary>
    ///     获取此次审核的结果。
    /// </summary>
    public ForumAuditResult Result { get; }

    /// <summary>
    ///     获取审核未通过时的原因描述。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="Result"/> 为 <see cref="QQBot.ForumAuditResult.Rejected"/> 时可能包含内容；否则为 <see langword="null"/>。
    /// </remarks>
    public string? ErrorMessage { get; }

    private SocketForumAuditResult(SocketForumChannel channel, ulong authorId, ForumAuditType type,
        string? threadId, string? postId, string? replyId, ForumAuditResult result, string? errorMessage)
    {
        Guild = channel.Guild;
        Channel = channel;
        AuthorId = authorId;
        Type = type;
        ThreadId = threadId;
        PostId = postId;
        ReplyId = replyId;
        Result = result;
        ErrorMessage = errorMessage;
    }

    internal static SocketForumAuditResult Create(SocketForumChannel channel,
        API.Gateway.ForumPublishAuditResultEvent model)
    {
        ForumAuditType type = model.AuditType switch
        {
            API.Gateway.AuditType.Thread => ForumAuditType.Thread,
            API.Gateway.AuditType.Post => ForumAuditType.Post,
            API.Gateway.AuditType.Reply => ForumAuditType.Reply,
            _ => ForumAuditType.Thread
        };
        ForumAuditResult result = model.Failed ? ForumAuditResult.Rejected : ForumAuditResult.Passed;
        string? errorMessage = string.IsNullOrEmpty(model.ErrorMessage) ? null : model.ErrorMessage;
        return new SocketForumAuditResult(channel, model.AuthorId, type,
            model.ThreadId, model.PostId, model.ReplyId, result, errorMessage);
    }

    private string DebuggerDisplay => $"{Type} ({Result})";
}
