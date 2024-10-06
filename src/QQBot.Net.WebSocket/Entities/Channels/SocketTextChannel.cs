using System.Diagnostics;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示服务器中一个基于网关的具有文字聊天能力的频道，可以发送和接收消息。
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
    public ChannelPrivateType? PrivateType { get; private set; }

    /// <inheritdoc />
    public SpeakPermission? SpeakPermission { get; private set; }

    /// <inheritdoc />
    public ChannelPermission? Permission { get; private set; }

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> CachedMessages => _messages?.Messages ?? [];

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
