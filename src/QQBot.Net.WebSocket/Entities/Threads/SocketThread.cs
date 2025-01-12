using System.Diagnostics;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的论坛主题。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketThread : SocketEntity<string>, IThread
{
    /// <inheritdoc cref="QQBot.IThread.Guild" />
    public SocketGuild Guild { get; }

    /// <inheritdoc cref="QQBot.IThread.Channel" />
    public SocketForumChannel Channel { get; }

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
    private SocketThread(QQBotSocketClient client, string id, SocketForumChannel channel, ulong authorId)
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

    internal static SocketThread Create(QQBotSocketClient client,
        SocketForumChannel channel, ulong authorId, API.ThreadInfo model)
    {
        SocketThread thread = new(client, model.ThreadId, channel, authorId);
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

    /// <inheritdoc cref="SocketThread.Content" />
    public override string ToString() => RawContent;

    /// <inheritdoc />
    IGuild IThread.Guild => Guild;

    /// <inheritdoc />
    IForumChannel IThread.Channel => Channel;
}
