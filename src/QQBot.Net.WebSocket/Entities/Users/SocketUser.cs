using Model = QQBot.API.User;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的用户。
/// </summary>
public class SocketUser : SocketEntity<ulong>, IUser
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
    internal SocketUser(QQBotSocketClient client, ulong id)
        : base(client, id)
    {
        Username = string.Empty;
        Avatar = string.Empty;
    }

    internal static SocketUser Create(QQBotSocketClient client, Model model)
    {
        SocketUser entity = new(client, model.Id);
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
