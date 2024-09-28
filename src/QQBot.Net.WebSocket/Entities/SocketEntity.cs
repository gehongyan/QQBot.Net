namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的具有唯一标识符的实体。
/// </summary>
/// <typeparam name="TId"> 唯一标识符的类型。 </typeparam>
public abstract class SocketEntity<TId> : IEntity<TId>
    where TId : IEquatable<TId>
{
    internal QQBotSocketClient Client { get; }

    /// <inheritdoc />
    public TId Id { get; }

    internal SocketEntity(QQBotSocketClient client, TId id)
    {
        Client = client;
        Id = id;
    }
}
