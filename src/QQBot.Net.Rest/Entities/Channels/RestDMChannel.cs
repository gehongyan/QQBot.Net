using System.Diagnostics;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的子频道用户私聊子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestDMChannel : RestEntity<string>, IDMChannel
{
    /// <inheritdoc cref="QQBot.IDMChannel.Id" />
    public new ulong Id { get; }

    /// <inheritdoc cref="QQBot.IDMChannel.Recipient" />
    public RestGuildUser Recipient { get; }

    internal RestDMChannel(BaseQQBotClient client, ulong id, RestGuildUser recipient)
        : base(client, id.ToIdString())
    {
        Id = id;
        Recipient = recipient;
    }

    internal static RestDMChannel Create(BaseQQBotClient client, ulong id, RestGuildUser recipient) =>
        new(client, id, recipient);

    private string DebuggerDisplay => $"@{Recipient} ({Id}, DM)";

    #region Messages

    /// <summary>
    ///     向此子频道发送消息。
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
    public Task<IUserMessage> SendMessageAsync(string? content = null, IMarkdown? markdown = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null,
        MessageReference? messageReference = null, IUserMessage? passiveSource = null, RequestOptions? options = null) =>
        ChannelHelper.SendMessageAsync(this, Client, content, markdown, attachment, embed, ark, messageReference, passiveSource, options);

    #endregion

    #region Users

    /// <inheritdoc cref="QQBot.IChannel.GetUserAsync(System.String,QQBot.CacheMode,QQBot.RequestOptions)" />
    public Task<IUser?> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null) =>
        Task.FromResult<IUser?>(id == Recipient.Id ? Recipient : null);

    #endregion

    #region IDMChannel

    /// <inheritdoc />
    IUser IDMChannel.Recipient => Recipient;

    #endregion

    #region IPrivateChannel

    /// <inheritdoc />
    IReadOnlyCollection<IUser> IPrivateChannel.Recipients => [Recipient];

    #endregion

    #region IMessageChannel

    /// <inheritdoc />
    async Task<IUserMessage> IMessageChannel.SendMessageAsync(string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        if (keyboard is not null)
            throw new NotSupportedException("Cannot send a keyboard to IDMChannel.");
        return await SendMessageAsync(content, markdown, attachment, embed, ark,
            messageReference, passiveSource, options).ConfigureAwait(false);
    }

    /// <inheritdoc />
    Task<IMessage?> IMessageChannel.GetMessageAsync(string id, CacheMode mode, RequestOptions? options)
        => Task.FromResult<IMessage?>(null);

    #endregion

    #region IChannel

    /// <inheritdoc />
    Task<IUser?> IChannel.GetUserAsync(string id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IUser?>(ulong.TryParse(id, out ulong userId) && userId == Recipient.Id ? Recipient : null);

    #endregion
}
