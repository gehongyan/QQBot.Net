using System.Collections.Immutable;
using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的子频道用户私聊频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketDMChannel : SocketChannel, IDMChannel, ISocketPrivateChannel, ISocketMessageChannel
{
    /// <inheritdoc cref="QQBot.IDMChannel.Id" />
    public new ulong Id { get; }

    /// <inheritdoc cref="QQBot.IDMChannel.Recipient" />
    public SocketUser Recipient { get; }

    // /// <inheritdoc cref="QQBot.WebSocket.SocketChannel.Users" />
    // public new IReadOnlyCollection<SocketUser> Users => ImmutableArray.Create(Client.CurrentUser, Recipient);

    /// <inheritdoc />
    /// <remarks>
    ///     <note type="important">
    ///         私聊消息频道不支持缓存消息，此属性将始终返回空集合。
    ///     </note>
    /// </remarks>
    public IReadOnlyCollection<SocketMessage> CachedMessages => [];

    /// <inheritdoc />
    internal SocketDMChannel(QQBotSocketClient client, ulong id, SocketUser recipient)
        : base(client, id.ToString())
    {
        Id = id;
        Recipient = recipient;
    }

    internal static SocketDMChannel Create(QQBotSocketClient client, ClientState state, ulong id, SocketUser recipient)
    {
        SocketDMChannel channel = new(client, id, recipient);
        return channel;
    }

    internal void AddMessage(SocketMessage message) { }

    #region IDMChannel

    /// <inheritdoc />
    IUser IDMChannel.Recipient => Recipient;

    #endregion

    #region ISocketPrivateChannel

    /// <inheritdoc />
    IReadOnlyCollection<SocketUser> ISocketPrivateChannel.Recipients => [Recipient];

    #endregion

    #region IPrivateChannel

    /// <inheritdoc />
    IReadOnlyCollection<IUser> IPrivateChannel.Recipients => [Recipient];

    #endregion

    private string DebuggerDisplay => $"@{Recipient} ({Id}, DM)";

    #region Messages

    /// <inheritdoc cref="QQBot.IMessageChannel.SendMessageAsync(System.String,System.Nullable{QQBot.FileAttachment},QQBot.Embed,QQBot.MessageReference,QQBot.IUserMessage,QQBot.RequestOptions)" />
    public Task<Cacheable<IUserMessage, string>> SendMessageAsync(string? content = null,
        FileAttachment? attachment = null, Embed? embed = null,
        MessageReference? messageReference = null, IUserMessage? passiveSource = null, RequestOptions? options = null) =>
        ChannelHelper.SendMessageAsync(this, Client, content, attachment, embed, messageReference, passiveSource, options);

    #endregion

    #region IMessageChannel

    /// <inheritdoc />
    Task<Cacheable<IUserMessage, string>> IMessageChannel.SendMessageAsync(string? content,
        FileAttachment? attachment, Embed? embed,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options) =>
        SendMessageAsync(content, attachment, embed, messageReference, passiveSource, options);

    #endregion
}
