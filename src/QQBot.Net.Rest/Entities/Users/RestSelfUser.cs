using Model = QQBot.API.SelfUser;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的当前用户。
/// </summary>
public class RestSelfUser : RestUser, ISelfUser
{
    /// <inheritdoc cref="QQBot.ISelfUser.Id" />
    public new ulong Id { get; }

    /// <inheritdoc />
    internal RestSelfUser(BaseQQBotClient client, ulong id)
        : base(client, id.ToIdString())
    {
        Id = id;
    }

    internal static RestSelfUser Create(BaseQQBotClient client, Model model)
    {
        RestSelfUser entity = new(client, model.Id);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
    {
        base.Update(model);
    }
}
