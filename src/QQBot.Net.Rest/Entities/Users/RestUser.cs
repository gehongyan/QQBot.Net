using QQBot.API;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的用户。
/// </summary>
public abstract class RestUser : RestEntity<string>, IUser
{
    /// <inheritdoc />
    public string Mention => MentionUtils.MentionUser(this);

    /// <inheritdoc />
    public string? Avatar { get; internal set; }
    /// <inheritdoc />
    protected RestUser(BaseQQBotClient client, string id)
        : base(client, id)
    {
    }

    internal virtual void Update(User model) { }
}
