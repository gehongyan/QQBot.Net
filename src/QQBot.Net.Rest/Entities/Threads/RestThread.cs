using System.Diagnostics;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的论坛主题。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestThread : RestEntity<string>, IThread
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
    private RestThread(BaseQQBotClient client, string id, IForumChannel channel, ulong authorId)
        : base(client, id)
    {
        Guild = channel.Guild;
        Channel = channel;
        AuthorId = authorId;
        Title = string.Empty;
        RawContent = string.Empty;
        Content = RichText.Empty;
        CreatedAt = DateTimeOffset.Now;
    }

    internal static RestThread Create(BaseQQBotClient client,
        IForumChannel channel, ulong authorId, API.ThreadInfo model)
    {
        RestThread thread = new(client, model.ThreadId, channel, authorId);
        thread.Update(model);
        return thread;
    }

    internal void Update(API.ThreadInfo model)
    {
        Title = model.Title;
        RawContent = model.Content;
        Content = ForumHelper.ParseContent(model.Content);
        CreatedAt = model.DateTime;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(RequestOptions? options = null)
    {
        API.Thread model = await ChannelHelper.GetThreadAsync(Channel, Client, Id, options).ConfigureAwait(false);
        Update(model.ThreadInfo);
    }

    /// <inheritdoc />
    public Task DeleteAsync(RequestOptions? options = null) =>
        ChannelHelper.DeleteThreadAsync(Channel, Client, Id, options);

    private string DebuggerDisplay => $"{Title} ({Id}, {Content.DebuggerDisplay})";

    /// <inheritdoc cref="RestThread.Content" />
    public override string ToString() => RawContent;
}
