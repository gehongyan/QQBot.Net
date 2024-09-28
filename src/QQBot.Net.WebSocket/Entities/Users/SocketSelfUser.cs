using Model = QQBot.API.SelfUser;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的当前用户。
/// </summary>
public class SocketSelfUser : SocketUser, ISelfUser
{
    /// <inheritdoc />
    internal SocketSelfUser(QQBotSocketClient client, ulong id)
        : base(client, id)
    {
    }

    internal static SocketSelfUser Create(QQBotSocketClient client, Model model)
    {
        SocketSelfUser entity = new(client, model.Id);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
    {
        base.Update(model);
    }
}
