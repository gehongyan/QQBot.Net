using System.Diagnostics;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的论坛主题。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestForumThread : RestEntity<string>, IForumThread
{
    /// <inheritdoc />
    public IGuild Guild { get; }

    /// <inheritdoc />
    public IForumChannel Channel { get; }

    /// <inheritdoc />
    public ulong AuthorId { get; }

    /// <inheritdoc />
    public string Title { get; private set; }

    /// <inheritdoc />
    public string RawContent { get; private set; }

    /// <inheritdoc />
    public RichText Content { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; private set; }

    /// <inheritdoc />
    private RestForumThread(BaseQQBotClient client, string id,
        IForumChannel channel, ulong authorId, string title, string content, DateTimeOffset createdAt)
        : base(client, id)
    {
        Guild = channel.Guild;
        Channel = channel;
        AuthorId = authorId;
        Title = title;
        RawContent = content;
        Content = ForumHelper.ParseContent(content);
        CreatedAt = createdAt;
    }

    internal static RestForumThread Create(BaseQQBotClient client,
        IForumChannel channel, ulong authorId, API.ThreadInfo model) =>
        new(client, model.ThreadId, channel, authorId, model.Title, model.Content, model.DateTime);

    internal void Update(API.ThreadInfo model)
    {
        Title = model.Title;
        RawContent = model.Content;
        Content = ForumHelper.ParseContent(model.Content);
        CreatedAt = model.DateTime;
    }

    private string DebuggerDisplay => $"{Title} ({Id}, {Content.DebuggerDisplay})";

    /// <inheritdoc cref="QQBot.Rest.RestForumThread.Content" />
    public override string ToString() => RawContent;
}
