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

    /// <inheritdoc />
    public Task DeleteAsync(bool? hideTip = null, RequestOptions? options = null) =>
        MessageHelper.DeleteAsync(this, Client, hideTip, options);
}
