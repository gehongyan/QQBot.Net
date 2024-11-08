using System.Diagnostics;
using QQBot.API;
using QQBot.API.Gateway;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个由用户发送的消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketUserMessage : SocketMessage, IUserMessage
{
    internal SocketUserMessage(QQBotSocketClient client, string id,
        ISocketMessageChannel channel, SocketUser author, MessageSource source)
        : base(client, id, channel, author, source)
    {
    }

    internal static new SocketUserMessage Create(QQBotSocketClient client, ClientState state,
        ISocketMessageChannel channel, SocketUser author, MessageCreatedEvent model, string dispatch)
    {
        SocketUserMessage entity = new(client, model.Id, channel, author, MessageSource.User);
        entity.Update(state, model);
        return entity;
    }

    internal static new SocketUserMessage Create(QQBotSocketClient client, ClientState state,
        ISocketMessageChannel channel, SocketUser author, ChannelMessage model, string dispatch)
    {
        SocketUserMessage entity = new(client, model.Id, channel, author, MessageSource.User);
        entity.Update(state, model);
        return entity;
    }

    /// <summary>
    ///     删除此对实体象及其所有子实体对象。
    /// </summary>
    /// <param name="hideTip"> 是否隐藏删除提示，仅在 <see cref="ITextChannel"/> 或 <see cref="IDMChannel"/> 内的消息支持。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    public Task DeleteAsync(bool? hideTip = null, RequestOptions? options = null) =>
        MessageHelper.DeleteAsync(this, Client, hideTip, options);

    /// <inheritdoc />
    Task IDeletable.DeleteAsync(RequestOptions? options) => DeleteAsync(null, options);
}
