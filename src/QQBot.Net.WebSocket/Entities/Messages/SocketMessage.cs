using System.Diagnostics;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的通用消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class SocketMessage : SocketEntity<string>, IMessage
{
    /// <inheritdoc cref="QQBot.IMessage.Channel" />
    public ISocketMessageChannel Channel { get; private set; }

    /// <inheritdoc cref="QQBot.IMessage.Author" />
    public SocketUser Author { get; private set; }

    /// <inheritdoc />
    public MessageSource Source { get; }

    /// <inheritdoc />
    public string Content { get; internal set; } = string.Empty;

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    /// <inheritdoc cref="QQBot.IMessage.Attachments" />
    public virtual IReadOnlyCollection<Attachment> Attachments { get; private set; }

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
        SocketUser author, ISocketMessageChannel channel, API.Gateway.UserMessageCreatedEvent model) =>
        SocketSimpleMessage.Create(client, state, author, channel, model);

    internal virtual void Update(ClientState state, API.Gateway.UserMessageCreatedEvent model)
    {
        Content = model.Content;
        CreatedAt = model.Timestamp;
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

    #endregion

}
