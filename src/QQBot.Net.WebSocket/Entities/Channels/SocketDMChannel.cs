using System.Collections.Immutable;
using System.Diagnostics;
using QQBot.Rest;

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
        : base(client, id.ToIdString())
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

    /// <summary>
    ///     向此频道发送消息。
    /// </summary>
    /// <param name="content"> 要发送的消息内容。 </param>
    /// <param name="markdown"> 要发送的 Markdown 消息内容。 </param>
    /// <param name="attachment"> 要发送的文件附件。 </param>
    /// <param name="embed"> 要发送的嵌入式消息内容。 </param>
    /// <param name="ark"> 要发送的模板消息内容。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="passiveSource"> 被动消息来源。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    public Task<Cacheable<IUserMessage, string>> SendMessageAsync(string? content = null, IMarkdown? markdown = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null,
        MessageReference? messageReference = null, IUserMessage? passiveSource = null, RequestOptions? options = null) =>
        ChannelHelper.SendMessageAsync(this, Client, content, markdown, attachment, embed, ark, messageReference, passiveSource, options);

    #endregion

    #region IMessageChannel

    /// <inheritdoc />
    Task<Cacheable<IUserMessage, string>> IMessageChannel.SendMessageAsync(string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        if (keyboard is not null)
            throw new NotSupportedException("Cannot send a keyboard to IDMChannel.");
        return SendMessageAsync(content, markdown, attachment, embed, ark, messageReference, passiveSource, options);
    }

    #endregion
}
