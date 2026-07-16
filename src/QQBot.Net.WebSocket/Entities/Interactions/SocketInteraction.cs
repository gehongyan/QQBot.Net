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
    public string? GuildId { get; }

    /// <summary>
    ///     获取互动发生的文字子频道 ID；仅频道场景提供。
    /// </summary>
    public string? ChannelId { get; }

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
    ///     获取互动携带的数据。
    /// </summary>
    public SocketInteractionData Data { get; }

    /// <summary>
    ///     获取事件协议版本。
    /// </summary>
    public int? Version { get; }

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
        int dataType = model.Data?.Type ?? 0;
        Data = new SocketInteractionData(dataType != 0 ? dataType : model.Type, model.Data?.Resolved);
        Version = model.Version;
        EventId = eventId;
    }

    internal static SocketInteraction Create(QQBotSocketClient client, Model model, string? eventId) =>
        new(client, model, eventId);

    /// <summary>
    ///     向 QQ Bot API 回应此互动事件。
    /// </summary>
    /// <param name="responseCode"> 互动事件的处理结果。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <exception cref="InvalidOperationException"> 此互动已经回应或正在回应。 </exception>
    public Task RespondAsync(InteractionResponseCode responseCode = InteractionResponseCode.Success,
        RequestOptions? options = null) =>
        AcknowledgeAsync(responseCode, options);

    /// <summary>
    ///     向 QQ Bot API 成功回应此互动事件，并向互动发生的聊天上下文发送一条消息。
    /// </summary>
    /// <param name="content"> 要发送的消息内容。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <exception cref="InvalidOperationException"> 此互动已经回应或正在回应。 </exception>
    /// <exception cref="NotSupportedException"> 此互动没有提供可用于发送消息的聊天上下文。 </exception>
    public async Task RespondAsync(string content, RequestOptions? options = null)
    {
        if (!TryBeginResponse())
            throw new InvalidOperationException("This interaction has already been acknowledged.");

        try
        {
            await SendMessageAsync(content, options).ConfigureAwait(false);
            await FinishResponseAsync(InteractionResponseCode.Success, options).ConfigureAwait(false);
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
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <exception cref="NotSupportedException"> 此互动没有提供可用于发送消息的聊天上下文。 </exception>
    public Task SendMessageAsync(string content, RequestOptions? options = null) =>
        SocketInteractionHelper.SendMessageAsync(this, content, options);

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
            throw;
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
