using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的群组频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketUserChannel : SocketChannel, IUserChannel, ISocketPrivateChannel, ISocketMessageChannel
{
    /// <inheritdoc cref="QQBot.IUserChannel.Id" />
    public new Guid Id { get; }

    /// <inheritdoc cref="QQBot.IDMChannel.Recipient" />
    public SocketUser Recipient { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> CachedMessages => [];

    /// <inheritdoc />
    internal SocketUserChannel(QQBotSocketClient client, Guid id, SocketUser recipient)
        : base(client, id.ToString("N").ToUpperInvariant())
    {
        Id = id;
        Recipient = recipient;
    }

    internal static SocketUserChannel Create(QQBotSocketClient client, ClientState state, Guid id, SocketUser recipient)
    {
        SocketUserChannel channel = new(client, id, recipient);
        return channel;
    }

    internal void AddMessage(SocketMessage message) { }

    #region ISocketPrivateChannel

    /// <inheritdoc />
    IReadOnlyCollection<SocketUser> ISocketPrivateChannel.Recipients => [Recipient];

    #endregion

    #region IPrivateChannel

    /// <inheritdoc />
    IReadOnlyCollection<IUser> IPrivateChannel.Recipients => [Recipient];

    #endregion

    private string DebuggerDisplay => $"Unknown ({Id}, User)";

    #region Messages

    /// <inheritdoc cref="QQBot.IMessageChannel.SendMessageAsync(System.String,System.Nullable{QQBot.FileAttachment},QQBot.Embed,QQBot.Ark,QQBot.MessageReference,QQBot.IUserMessage,QQBot.RequestOptions)" />
    public Task<Cacheable<IUserMessage, string>> SendMessageAsync(string? content = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null,
        MessageReference? messageReference = null, IUserMessage? passiveSource = null, RequestOptions? options = null) =>
        ChannelHelper.SendMessageAsync(this, Client, content, attachment, embed, ark, messageReference, passiveSource, options);

    #endregion

    #region IMessageChannel

    /// <inheritdoc />
    Task<Cacheable<IUserMessage, string>> IMessageChannel.SendMessageAsync(string? content,
        FileAttachment? attachment, Embed? embed, Ark? ark,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options) =>
        SendMessageAsync(content, attachment, embed, ark, messageReference, passiveSource, options);

    #endregion
}
