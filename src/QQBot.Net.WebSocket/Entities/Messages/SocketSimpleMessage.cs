using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的群聊或单聊内的简单消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketSimpleMessage : SocketMessage, ISimpleMessage
{
    internal SocketSimpleMessage(QQBotSocketClient client, string id,
        ISocketMessageChannel channel, SocketUser author, MessageSource source)
        : base(client, id, channel, author, source)
    {
    }

    internal static new SocketSimpleMessage Create(QQBotSocketClient client, ClientState state,
        SocketUser author, ISocketMessageChannel channel, API.Gateway.MessageCreatedEvent model)
    {
        SocketSimpleMessage entity = new(client, model.Id, channel, author, MessageSource.User);
        entity.Update(state, model);
        return entity;
    }

    internal override void Update(ClientState state, API.Gateway.MessageCreatedEvent model)
    {
        base.Update(state, model);
    }
}
