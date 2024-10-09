using System.Collections.Immutable;
using System.Diagnostics;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的通用消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class RestMessage : RestEntity<string>, IMessage
{
    private ImmutableArray<Embed> _embeds = [];

    /// <inheritdoc />
    public IMessageChannel Channel { get; }

    /// <inheritdoc />
    public IUser Author { get; }

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
    protected RestMessage(BaseQQBotClient client, string id,
        IMessageChannel channel, IUser author, MessageSource source)
        : base(client, id)
    {
        Channel = channel;
        Author = author;
        Source = source;
        Attachments = [];
    }

    internal static RestMessage Create(BaseQQBotClient client,
        IMessageChannel channel, IUser author, API.ChannelMessage model) =>
        RestUserMessage.Create(client, channel, author, model);

    internal virtual void Update(API.ChannelMessage model)
    {
        Content = model.Content;
        Timestamp = model.Timestamp;
        if (model.Attachments is { Length: > 0 } attachments)
            Attachments = [..attachments.Select(MessageHelper.CreateAttachment)];
        MentionedEveryone = model.MentionEveryone;
        if (model.Embeds is { Length: > 0 } embedModels)
            _embeds = [..embedModels.Select(x => x.ToEntity())];
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
    IReadOnlyCollection<IAttachment> IMessage.Attachments => Attachments;

    /// <inheritdoc />
    IReadOnlyCollection<IEmbed> IMessage.Embeds => Embeds;

    #endregion
}
