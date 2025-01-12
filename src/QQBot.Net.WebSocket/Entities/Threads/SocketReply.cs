using System.Diagnostics;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的论坛主题评论回复。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketReply : SocketEntity<string>, IReply
{
    /// <inheritdoc cref="QQBot.IReply.Guild" />
    public SocketGuild Guild { get; }

    /// <inheritdoc cref="QQBot.IReply.Channel" />
    public SocketForumChannel Channel { get; }

    /// <inheritdoc />
    public ulong AuthorId { get; }

    /// <inheritdoc />
    public string ThreadId { get; }

    /// <inheritdoc />
    public string PostId { get; }

    /// <inheritdoc />
    public string RawContent { get; private set; }

    /// <inheritdoc />
    public RichText Content { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; private set; }

    /// <inheritdoc />
    private SocketReply(QQBotSocketClient client, string id, string threadId, string postId, SocketForumChannel channel, ulong authorId)
        : base(client, id)
    {
        Guild = channel.Guild;
        Channel = channel;
        AuthorId = authorId;
        ThreadId = threadId;
        PostId = postId;
        RawContent = string.Empty;
        Content = RichText.Empty;
        CreatedAt = DateTimeOffset.Now;
    }

    internal static SocketReply Create(QQBotSocketClient client,
        SocketForumChannel channel, ulong authorId, API.ReplyInfo model)
    {
        SocketReply Reply = new(client, model.ReplyId, model.ThreadId, model.PostId, channel, authorId);
        Reply.Update(model);
        return Reply;
    }

    private void Update(API.ReplyInfo model)
    {
        RawContent = model.Content;
        Content = ForumHelper.ParseContent(model.Content);
        CreatedAt = model.DateTime;
    }

    private string DebuggerDisplay => $"{Content} ({Id}, {Content.DebuggerDisplay})";

    /// <inheritdoc cref="SocketThread.Content" />
    public override string ToString() => RawContent;

    /// <inheritdoc />
    IGuild IReply.Guild => Guild;

    /// <inheritdoc />
    IForumChannel IReply.Channel => Channel;
}

// internal class ForumPublishAuditResultEvent
// {
//     [JsonPropertyName("guild_id")]
//     public required ulong GuildId { get; init; }
//
//     [JsonPropertyName("channel_id")]
//     public required ulong ChannelId { get; init; }
//
//     [JsonPropertyName("author_id")]
//     public required ulong AuthorId { get; init; }
//
//     [JsonPropertyName("thread_id")]
//     public required string? ThreadId { get; init; }
//
//     [JsonPropertyName("post_id")]
//     public required string? PostId { get; init; }
//
//     [JsonPropertyName("reply_id")]
//     public required string? ReplyId { get; init; }
//
//     [JsonPropertyName("type")]
//     public AuditType AuditType { get; init; }
//
//     [JsonPropertyName("result")]
//     [NumberBooleanConverter]
//     public int Failed { get; init; }
//
//     [JsonPropertyName("err_msg")]
//     public string? ErrorMessage { get; init; }
// }
