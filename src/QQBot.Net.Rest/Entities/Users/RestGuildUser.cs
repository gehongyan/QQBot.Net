using QQBot.API;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的子频道用户。
/// </summary>
public class RestGuildUser : RestUser, IGuildUser
{
    /// <inheritdoc cref="QQBot.IGuildUser.Id" />
    public new ulong Id { get; }

    /// <inheritdoc />
    public string Username { get; private set; }

    /// <inheritdoc />
    public bool? IsBot { get; private set; }

    /// <inheritdoc />
    public string? UnionOpenId { get; private set; }

    /// <inheritdoc />
    public string? UnionUserAccount { get; private set; }

    /// <inheritdoc />
    internal RestGuildUser(BaseQQBotClient client, ulong id)
        : base(client, id.ToIdString())
    {
        Id = id;
        Username = string.Empty;
        Avatar = string.Empty;
    }

    internal static RestGuildUser Create(BaseQQBotClient client, User model)
    {
        RestGuildUser entity = new(client, model.Id);
        entity.Update(model);
        return entity;
    }

    internal override void Update(User model)
    {
        Username = model.Username;
        Avatar = model.Avatar;
        IsBot = model.IsBot;
        UnionOpenId = model.UnionOpenId;
        UnionUserAccount = model.UnionUserAccount;
    }
}
