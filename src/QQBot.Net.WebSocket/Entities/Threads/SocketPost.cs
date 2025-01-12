using System.Diagnostics;
using QQBot.API;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的论坛主题评论。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketPost : SocketEntity<string>, IPost
{
    /// <inheritdoc cref="QQBot.IPost.Guild" />
    public SocketGuild Guild { get; }

    /// <inheritdoc cref="QQBot.IPost.Channel" />
    public SocketForumChannel Channel { get; }

    /// <inheritdoc />
    public ulong AuthorId { get; }

    /// <inheritdoc />
    public string ThreadId { get; }

    /// <inheritdoc />
    public string RawContent { get; private set; }

    /// <inheritdoc />
    public RichText Content { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; private set; }

    /// <inheritdoc />
    private SocketPost(QQBotSocketClient client, string id, string threadId, SocketForumChannel channel, ulong authorId)
        : base(client, id)
    {
        Guild = channel.Guild;
        Channel = channel;
        AuthorId = authorId;
        ThreadId = threadId;
        RawContent = string.Empty;
        Content = RichText.Empty;
        CreatedAt = DateTimeOffset.Now;
    }

    internal static SocketPost Create(QQBotSocketClient client,
        SocketForumChannel channel, ulong authorId, API.PostInfo model)
    {
        SocketPost post = new(client, model.PostId, model.ThreadId, channel, authorId);
        post.Update(model);
        return post;
    }

    private void Update(API.PostInfo model)
    {
        RawContent = model.Content;
        Content = ForumHelper.ParseContent(model.Content);
        CreatedAt = model.DateTime;
    }

    private string DebuggerDisplay => $"{Content} ({Id}, {Content.DebuggerDisplay})";

    /// <inheritdoc cref="SocketThread.Content" />
    public override string ToString() => RawContent;

    /// <inheritdoc />
    IGuild IPost.Guild => Guild;

    /// <inheritdoc />
    IForumChannel IPost.Channel => Channel;
}
