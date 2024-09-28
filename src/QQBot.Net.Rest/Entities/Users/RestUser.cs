using Model = QQBot.API.User;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的用户。
/// </summary>
public class RestUser : RestEntity<ulong>, IUser
{
    /// <inheritdoc />
    public string Username { get; private set; }

    /// <inheritdoc />
    public string? Avatar { get; private set; }

    /// <inheritdoc />
    public bool? IsBot { get; private set; }

    /// <inheritdoc />
    public string? UnionOpenId { get; private set; }

    /// <inheritdoc />
    public string? UnionUserAccount { get; private set; }

    /// <inheritdoc />
    internal RestUser(BaseQQBotClient client, ulong id)
        : base(client, id)
    {
        Username = string.Empty;
        Avatar = string.Empty;
    }

    internal static RestUser Create(BaseQQBotClient client, Model model)
    {
        RestUser entity = new(client, model.Id);
        entity.Update(model);
        return entity;
    }

    internal virtual void Update(Model model)
    {
        Username = model.Username;
        Avatar = model.Avatar;
        IsBot = model.IsBot;
        UnionOpenId = model.UnionOpenId;
        UnionUserAccount = model.UnionUserAccount;
    }
}
