using Model = QQBot.API.SelfUser;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的当前用户。
/// </summary>
public class RestSelfUser : RestGuildUser, ISelfUser
{
    /// <inheritdoc />
    internal RestSelfUser(BaseQQBotClient client, ulong id)
        : base(client, id)
    {
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
