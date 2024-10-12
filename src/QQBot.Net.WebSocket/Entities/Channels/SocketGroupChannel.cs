using System.Diagnostics;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的群组子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGroupChannel : SocketChannel, IGroupChannel, ISocketMessageChannel
{
    /// <inheritdoc cref="QQBot.IGroupChannel.Id" />
    public new Guid Id { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> CachedMessages => [];

    /// <inheritdoc />
    internal SocketGroupChannel(QQBotSocketClient client, Guid id)
        : base(client, id.ToIdString())
    {
        Id = id;
    }

    internal static SocketGroupChannel Create(QQBotSocketClient client, ClientState state, Guid id)
    {
        SocketGroupChannel channel = new(client, id);
        return channel;
    }

    internal void AddMessage(SocketMessage message) { }

    private string DebuggerDisplay => $"Unknown ({Id}, Group)";

    #region Messages

    /// <inheritdoc cref="QQBot.IMessageChannel.SendMessageAsync(System.String,IMarkdown,System.Nullable{QQBot.FileAttachment},QQBot.Embed,QQBot.Ark,QQBot.IKeyboard,QQBot.MessageReference,QQBot.IUserMessage,QQBot.RequestOptions)" />
    public Task<IUserMessage> SendMessageAsync(string? content = null, IMarkdown? markdown = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null, IKeyboard? keyboard = null,
        MessageReference? messageReference = null, IUserMessage? passiveSource = null, RequestOptions? options = null) =>
        ChannelHelper.SendMessageAsync(this, Client, content, markdown, attachment, embed, ark, keyboard, messageReference, passiveSource, options);

    #endregion

    #region ISocketMessageChannel

    /// <inheritdoc />
    SocketMessage? ISocketMessageChannel.GetCachedMessage(string id) => null;

    #endregion

    #region IMessageChannel

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendMessageAsync(string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options) =>
        SendMessageAsync(content, markdown, attachment, embed, ark, keyboard, messageReference, passiveSource, options);

    /// <inheritdoc />
    Task<IMessage?> IMessageChannel.GetMessageAsync(string id, CacheMode mode, RequestOptions? options)
        => Task.FromResult<IMessage?>(null);

    #endregion
}
