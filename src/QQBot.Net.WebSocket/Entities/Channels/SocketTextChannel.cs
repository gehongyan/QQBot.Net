using System.Diagnostics;
using QQBot.Rest;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示频道中一个基于网关的具有文字聊天能力的子频道，可以发送和接收消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketTextChannel : SocketGuildChannel, ITextChannel, ISocketMessageChannel
{
    private readonly MessageCache? _messages;

    /// <inheritdoc />
    public ChannelSubType? SubType { get; private set; }

    /// <inheritdoc />
    public ulong? CategoryId { get; private set; }

    /// <inheritdoc />
    public PrivateType? PrivateType { get; private set; }

    /// <inheritdoc />
    public SpeakPermission? SpeakPermission { get; private set; }

    /// <inheritdoc />
    public ChannelPermission? Permission { get; private set; }

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> CachedMessages => _messages?.Messages ?? [];

    /// <inheritdoc />
    public string Mention => MentionUtils.MentionChannel(this);

    internal SocketTextChannel(QQBotSocketClient client, ulong id, SocketGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.Text;
        if (Client.MessageCacheSize > 0)
            _messages = new MessageCache(Client);
    }

    internal static new SocketTextChannel Create(SocketGuild guild, ClientState state, Model model)
    {
        SocketTextChannel entity = new(guild.Client, model.Id, guild);
        entity.Update(state, model);
        return entity;
    }

    internal override void Update(ClientState state, Model model)
    {
        base.Update(state, model);
        CategoryId = model.ParentId;
        SubType = model.SubType;
        PrivateType = model.PrivateType;
        SpeakPermission = model.SpeakPermission;
        Permission = model.Permissions is not null ? Enum.Parse<ChannelPermission>(model.Permissions) : null; // TODO
    }

    internal void AddMessage(SocketMessage msg) => _messages?.Add(msg);

    internal SocketMessage? RemoveMessage(string id) => _messages?.Remove(id);

    private string DebuggerDisplay => $"{Name} ({Id}, Text)";

    #region Messages

    /// <inheritdoc />
    public SocketMessage? GetCachedMessage(string id) => _messages?.Get(id);

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

    /// <summary>
    ///     从此消息子频道获取一条消息。
    /// </summary>
    /// <param name="id"> 消息的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务结果包含检索到的消息；如果未找到具有指定 ID 的消息，则返回 <c>null</c>。 </returns>
    public async Task<RestUserMessage> GetMessageAsync(string id, RequestOptions? options = null) =>
        await ChannelHelper.GetMessageAsync(this, Client, id, options);

    #endregion

    #region IMessageChannel

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendMessageAsync(string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        if (keyboard is not null)
            throw new NotSupportedException("Cannot send a keyboard to ITextChannel.");
        return SendMessageAsync(content, markdown, attachment, embed, ark, messageReference, passiveSource, options);
    }

    /// <inheritdoc />
    async Task<IMessage?> IMessageChannel.GetMessageAsync(string id, CacheMode mode, RequestOptions? options)
    {
        IMessage? message = GetCachedMessage(id);
        if (message is not null || mode is CacheMode.CacheOnly)
            return message;
        return await GetMessageAsync(id, options);
    }

    #endregion
}
