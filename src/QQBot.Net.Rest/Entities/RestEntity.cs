namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的实体。
/// </summary>
/// <typeparam name="TId"> 唯一标识符的类型。 </typeparam>
public class RestEntity<TId> : IEntity<TId>
    where TId : IEquatable<TId>
{
    internal BaseQQBotClient Client { get; }

    /// <inheritdoc />
    public TId Id { get; }

    internal RestEntity(BaseQQBotClient client, TId id)
    {
        Client = client;
        Id = id;
    }
}
