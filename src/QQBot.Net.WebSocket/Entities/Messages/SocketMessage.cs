using System.Collections.Immutable;
using System.Diagnostics;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的通用消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class SocketMessage : SocketEntity<string>, IMessage
{
    private ImmutableArray<Embed> _embeds = [];
    private ImmutableArray<ITag> _tags = [];

    /// <inheritdoc cref="QQBot.IMessage.Channel" />
    public ISocketMessageChannel Channel { get; }

    /// <inheritdoc cref="QQBot.IMessage.Author" />
    public SocketUser Author { get; }

    /// <inheritdoc />
    public MessageSource Source { get; }

     /// <inheritdoc />
    public string Content { get; private set; } = string.Empty;

    /// <inheritdoc />
    public DateTimeOffset Timestamp { get; private set; } = DateTimeOffset.Now;

    /// <inheritdoc cref="QQBot.IMessage.Attachments" />
    public virtual IReadOnlyCollection<Attachment> Attachments { get; private set; }

    /// <inheritdoc />
    public bool? MentionedEveryone { get; private set; }

    /// <inheritdoc cref="QQBot.IMessage.Embeds" />
    public IReadOnlyCollection<Embed> Embeds => _embeds;

    /// <inheritdoc />
    public IReadOnlyCollection<ITag> Tags => _tags;

    /// <inheritdoc />
    protected SocketMessage(QQBotSocketClient client, string id,
        ISocketMessageChannel channel, SocketUser author, MessageSource source)
        : base(client, id)
    {
        Channel = channel;
        Author = author;
        Source = source;
        Attachments = [];
    }

    internal static SocketMessage Create(QQBotSocketClient client, ClientState state,
        ISocketMessageChannel channel, SocketUser author, API.Gateway.MessageCreatedEvent model, string dispatch) =>
        SocketUserMessage.Create(client, state, channel, author, model, dispatch);

    internal static SocketMessage Create(QQBotSocketClient client, ClientState state,
        ISocketMessageChannel channel, SocketUser author, API.ChannelMessage model, string dispatch) =>
        SocketUserMessage.Create(client, state, channel, author, model, dispatch);

    internal virtual void Update(ClientState state, API.ChannelMessage model)
    {
        Content = model.Content;
        Timestamp = model.Timestamp;
        if (model.Attachments is { Length: > 0 } attachments)
            Attachments = [..attachments.Select(MessageHelper.CreateAttachment)];
        MentionedEveryone = model.MentionEveryone;
        if (model.Embeds is { Length: > 0 } embedModels)
            _embeds = [..embedModels.Select(x => x.ToEntity())];
    }

    internal virtual void Update(ClientState state, API.Gateway.MessageCreatedEvent model)
    {
        Content = model.Content;
        Timestamp = model.Timestamp;
        if (model.Attachments is { Length: > 0 } attachments)
            Attachments = [..attachments.Select(SocketMessageHelper.CreateAttachment)];
    }

    private string DebuggerDisplay => $"{Author}: {Content} ({Id}{
        Attachments.Count switch
        {
            0 => string.Empty,
            1 => ", 1 Attachment",
            _ => $", {Attachments.Count} Attachments"
        }})";

    #region IMessage

    /// <inheritdoc />
    IMessageChannel IMessage.Channel => Channel;

    /// <inheritdoc />
    IUser IMessage.Author => Author;

    /// <inheritdoc />
    IReadOnlyCollection<IAttachment> IMessage.Attachments => Attachments;

    /// <inheritdoc />
    IReadOnlyCollection<IEmbed> IMessage.Embeds => Embeds;

    #endregion
}
