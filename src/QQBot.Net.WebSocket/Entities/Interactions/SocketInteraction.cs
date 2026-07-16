using System.Diagnostics;
using Model = QQBot.API.Gateway.InteractionEvent;
using ResolvedModel = QQBot.API.Gateway.InteractionResolvedData;

namespace QQBot.WebSocket;

/// <summary>
///     表示由 QQ Bot 网关接收的互动事件。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketInteraction : SocketEntity<string>
{
    private int _responseState;

    /// <summary>
    ///     获取互动类型。
    /// </summary>
    public InteractionType Type { get; }

    /// <summary>
    ///     获取互动发生的场景。
    /// </summary>
    public InteractionScene Scene { get; }

    /// <summary>
    ///     获取网关返回的原始场景值。
    /// </summary>
    public string? RawScene { get; }

    /// <summary>
    ///     获取互动发生的聊天类型。
    /// </summary>
    public InteractionChatType? ChatType { get; }

    /// <summary>
    ///     获取互动触发时间。
    /// </summary>
    public DateTimeOffset? Timestamp { get; }

    /// <summary>
    ///     获取互动发生的频道 ID；仅频道场景提供。
    /// </summary>
    public ulong? GuildId { get; }

    /// <summary>
    ///     获取互动发生的文字子频道 ID；仅频道场景提供。
    /// </summary>
    public ulong? ChannelId { get; }

    /// <summary>
    ///     获取触发互动的用户 OpenID；仅单聊场景提供。
    /// </summary>
    public Guid? UserOpenId { get; }

    /// <summary>
    ///     获取互动发生的群 OpenID；仅群聊场景提供。
    /// </summary>
    public Guid? GroupOpenId { get; }

    /// <summary>
    ///     获取触发互动的群成员 OpenID；仅群聊场景提供。
    /// </summary>
    public Guid? GroupMemberOpenId { get; }

    /// <summary>
    ///     获取缓存中的互动频道；如果频道不在缓存中，则为 <see langword="null"/>。
    /// </summary>
    public SocketGuild? Guild { get; }

    /// <summary>
    ///     获取缓存中的互动子频道；如果子频道不在缓存中，则为 <see langword="null"/>。
    /// </summary>
    public ISocketMessageChannel? Channel { get; }

    /// <summary>
    ///     获取缓存中的互动用户；如果用户不在缓存中，则为 <see langword="null"/>。
    /// </summary>
    public SocketUser? User { get; }

    /// <summary>
    ///     获取缓存中的互动消息；如果消息不在缓存中，则为 <see langword="null"/>。
    /// </summary>
    public SocketMessage? Message { get; }

    /// <summary>
    ///     获取互动携带的数据。
    /// </summary>
    public SocketInteractionData Data { get; }

    /// <summary>
    ///     获取被点击按钮的 ID。
    /// </summary>
    public string? ButtonId => Data.Resolved.ButtonId;

    /// <summary>
    ///     获取发送按钮时设置的回调数据。
    /// </summary>
    public string? ButtonData => Data.Resolved.ButtonData;

    /// <summary>
    ///     获取用于发送被动消息的网关事件 ID。
    /// </summary>
    public string? EventId { get; }

    /// <summary>
    ///     获取此互动是否已回应。
    /// </summary>
    public bool HasResponded => Volatile.Read(ref _responseState) == 2;

    internal SocketInteraction(QQBotSocketClient client, Model model, string? eventId)
        : base(client, model.Id)
    {
        Type = (InteractionType)model.Type;
        RawScene = model.Scene;
        Scene = ParseScene(model.Scene, model.ChatType);
        ChatType = model.ChatType.HasValue ? (InteractionChatType)model.ChatType.Value : null;
        Timestamp = model.Timestamp;
        GuildId = model.GuildId;
        ChannelId = model.ChannelId;
        UserOpenId = model.UserOpenId;
        GroupOpenId = model.GroupOpenId;
        GroupMemberOpenId = model.GroupMemberOpenId;
        Guild = model.GuildId.HasValue ? client.State.GetGuild(model.GuildId.Value) : null;
        Channel = ResolveChannel(client, model, Scene);
        User = ResolveUser(client, model, Scene, Guild, Channel);
        Message = model.Data?.Resolved?.MessageId is { } messageId
            ? Channel?.GetCachedMessage(messageId)
            : null;
        int dataType = model.Data?.Type ?? 0;
        Data = new SocketInteractionData(dataType != 0 ? dataType : model.Type, model.Data?.Resolved);
        EventId = eventId;
    }

    internal static SocketInteraction Create(QQBotSocketClient client, Model model, string? eventId) =>
        new(client, model, eventId);

    /// <summary>
    ///     向 QQ Bot API 成功回应此互动事件，并向互动发生的聊天上下文发送一条消息。
    /// </summary>
    /// <param name="content"> 要发送的消息内容。 </param>
    /// <param name="markdown"> 要发送的 Markdown 消息内容。 </param>
    /// <param name="attachment"> 要发送的文件附件。 </param>
    /// <param name="embed"> 要发送的嵌入式消息内容。 </param>
    /// <param name="ark"> 要发送的模板消息内容。 </param>
    /// <param name="keyboard"> 要发送的按钮。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <exception cref="InvalidOperationException"> 此互动已经回应或正在回应。 </exception>
    /// <exception cref="NotSupportedException"> 此互动没有提供可用于发送消息的聊天上下文。 </exception>
    public async Task RespondAsync(string? content = null, IMarkdown? markdown = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null, IKeyboard? keyboard = null,
        MessageReference? messageReference = null, RequestOptions? options = null)
    {
        if (!TryBeginResponse())
            throw new InvalidOperationException("This interaction has already been acknowledged.");

        try
        {
            await SendMessageAsync(content, markdown, attachment, embed, ark, keyboard, messageReference, options)
                .ConfigureAwait(false);
        }
        catch
        {
            try
            {
                await FinishResponseAsync(InteractionResponseCode.Failed, options).ConfigureAwait(false);
            }
            catch
            {
                Volatile.Write(ref _responseState, 0);
                // Preserve the original message response exception.
            }
            throw;
        }

        try
        {
            await FinishResponseAsync(InteractionResponseCode.Success, options).ConfigureAwait(false);
        }
        catch
        {
            Volatile.Write(ref _responseState, 0);
            throw;
        }
    }

    /// <summary>
    ///     向 QQ Bot API 确认此互动事件。
    /// </summary>
    /// <param name="responseCode"> 互动事件的处理结果。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <exception cref="InvalidOperationException"> 此互动已经回应或正在回应。 </exception>
    public async Task AcknowledgeAsync(InteractionResponseCode responseCode = InteractionResponseCode.Success,
        RequestOptions? options = null)
    {
        if (!TryBeginResponse())
            throw new InvalidOperationException("This interaction has already been acknowledged.");

        try
        {
            await FinishResponseAsync(responseCode, options).ConfigureAwait(false);
        }
        catch
        {
            Volatile.Write(ref _responseState, 0);
            throw;
        }
    }

    /// <summary>
    ///     向互动发生的聊天上下文发送一条消息。
    /// </summary>
    /// <param name="content"> 要发送的消息内容。 </param>
    /// <param name="markdown"> 要发送的 Markdown 消息内容。 </param>
    /// <param name="attachment"> 要发送的文件附件。 </param>
    /// <param name="embed"> 要发送的嵌入式消息内容。 </param>
    /// <param name="ark"> 要发送的模板消息内容。 </param>
    /// <param name="keyboard"> 要发送的按钮。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送的消息。 </returns>
    /// <exception cref="NotSupportedException"> 此互动没有提供可用于发送消息的聊天上下文。 </exception>
    public Task<IUserMessage> SendMessageAsync(string? content = null, IMarkdown? markdown = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null, IKeyboard? keyboard = null,
        MessageReference? messageReference = null, RequestOptions? options = null) =>
        SocketInteractionHelper.SendMessageAsync(this, content, markdown, attachment, embed, ark, keyboard,
            messageReference, options);

    internal async Task<bool> TryAcknowledgeAsync(InteractionResponseCode responseCode,
        RequestOptions? options = null)
    {
        if (!TryBeginResponse())
            return false;

        try
        {
            await FinishResponseAsync(responseCode, options).ConfigureAwait(false);
            return true;
        }
        catch
        {
            Volatile.Write(ref _responseState, 0);
            return false;
        }
    }

    private bool TryBeginResponse() => Interlocked.CompareExchange(ref _responseState, 1, 0) == 0;

    private async Task FinishResponseAsync(InteractionResponseCode responseCode, RequestOptions? options)
    {
        await Client.ApiClient.RespondInteractionAsync(Id, responseCode, options).ConfigureAwait(false);
        Volatile.Write(ref _responseState, 2);
    }

    private static InteractionScene ParseScene(string? scene, int? chatType) => scene?.ToLowerInvariant() switch
    {
        "c2c" => InteractionScene.C2C,
        "group" => InteractionScene.Group,
        "guild" => InteractionScene.Guild,
        _ => chatType switch
        {
            (int)InteractionChatType.C2C => InteractionScene.C2C,
            (int)InteractionChatType.Group => InteractionScene.Group,
            (int)InteractionChatType.Guild => InteractionScene.Guild,
            _ => InteractionScene.Unknown
        }
    };

    private static ISocketMessageChannel? ResolveChannel(QQBotSocketClient client, Model model,
        InteractionScene scene) => scene switch
    {
        InteractionScene.Guild when model.ChannelId.HasValue =>
            client.State.GetGuildChannel(model.ChannelId.Value) as ISocketMessageChannel,
        InteractionScene.Group when model.GroupOpenId.HasValue =>
            client.State.GetGroupChannel(model.GroupOpenId.Value),
        InteractionScene.C2C when model.UserOpenId.HasValue =>
            client.State.GetUserChannel(model.UserOpenId.Value),
        _ => null
    };

    private static SocketUser? ResolveUser(QQBotSocketClient client, Model model, InteractionScene scene,
        SocketGuild? guild, ISocketMessageChannel? channel) => scene switch
    {
        InteractionScene.Guild when ulong.TryParse(model.Data?.Resolved?.UserId, out ulong userId) =>
            guild?.GetUser(userId) ?? client.GetGuildUser(userId),
        InteractionScene.Group when model.GroupMemberOpenId.HasValue =>
            client.GetUser(model.GroupMemberOpenId.Value.ToIdString()),
        InteractionScene.C2C when model.UserOpenId.HasValue =>
            client.GetUser(model.UserOpenId.Value.ToIdString()) ?? (channel as SocketUserChannel)?.Recipient,
        _ => null
    };

    private string DebuggerDisplay => $"{Type} ({Id}, {Scene})";
}

/// <summary>
///     表示互动事件携带的数据。
/// </summary>
public class SocketInteractionData
{
    /// <summary>
    ///     获取互动数据类型。
    /// </summary>
    public InteractionType Type { get; }

    /// <summary>
    ///     获取互动中已解析的数据。
    /// </summary>
    public SocketInteractionResolvedData Resolved { get; }

    internal SocketInteractionData(int type, ResolvedModel? resolved)
    {
        Type = (InteractionType)type;
        Resolved = new SocketInteractionResolvedData(resolved);
    }
}

/// <summary>
///     表示互动事件中已解析的数据。
/// </summary>
public class SocketInteractionResolvedData
{
    /// <summary>
    ///     获取发送按钮时设置的回调数据。
    /// </summary>
    public string? ButtonData { get; }

    /// <summary>
    ///     获取被点击按钮的 ID。
    /// </summary>
    public string? ButtonId { get; }

    /// <summary>
    ///     获取触发互动的频道用户 ID；仅频道场景提供。
    /// </summary>
    public string? UserId { get; }

    /// <summary>
    ///     获取快捷菜单功能 ID；仅自定义菜单提供。
    /// </summary>
    public string? FeatureId { get; }

    /// <summary>
    ///     获取互动关联的消息 ID。
    /// </summary>
    public string? MessageId { get; }

    internal SocketInteractionResolvedData(ResolvedModel? model)
    {
        ButtonData = model?.ButtonData;
        ButtonId = model?.ButtonId;
        UserId = model?.UserId;
        FeatureId = model?.FeatureId;
        MessageId = model?.MessageId;
    }
}

/// <summary>
///     表示互动类型。
/// </summary>
public enum InteractionType
{
    /// <summary>
    ///     未知互动类型。
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     消息按钮互动。
    /// </summary>
    MessageButton = 11,

    /// <summary>
    ///     单聊快捷菜单互动。
    /// </summary>
    PrivateChatMenu = 12
}

/// <summary>
///     表示互动发生的场景。
/// </summary>
public enum InteractionScene
{
    /// <summary>
    ///     未知场景。
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     频道场景。
    /// </summary>
    Guild,

    /// <summary>
    ///     群聊场景。
    /// </summary>
    Group,

    /// <summary>
    ///     单聊场景。
    /// </summary>
    C2C
}

/// <summary>
///     表示互动发生的聊天类型。
/// </summary>
public enum InteractionChatType
{
    /// <summary>
    ///     频道场景。
    /// </summary>
    Guild = 0,

    /// <summary>
    ///     群聊场景。
    /// </summary>
    Group = 1,

    /// <summary>
    ///     单聊场景。
    /// </summary>
    C2C = 2
}
