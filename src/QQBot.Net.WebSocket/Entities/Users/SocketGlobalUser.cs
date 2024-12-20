using System.Diagnostics;
using QQBot.API;

namespace QQBot.WebSocket;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
internal class SocketGlobalUser : SocketUser
{
    private readonly object _lockObj = new();
    private ushort _references;
    private readonly UserScope _scope;

    private string? _avatar;

    /// <inheritdoc />
    internal override SocketGlobalUser GlobalUser => this;

    /// <inheritdoc />
    public override string? Avatar
    {
        get => GetAvatarUrl();
        internal set => _avatar = value;
    }

    private string? GetAvatarUrl()
    {
        if (!string.IsNullOrEmpty(_avatar)) return _avatar;
        return _avatar ?? (Client.ApiClient.AppId.HasValue && OpenId.HasValue
            ? UrlUtils.GetUserAvatarUrl(Client.ApiClient.AppId.Value, OpenId.Value)
            : null);
    }

    internal Guid? OpenId { get; set; }

    public SocketGlobalUser(QQBotSocketClient client, string id, UserScope scope)
        : base(client, id)
    {
        _scope = scope;
    }

    internal static SocketGlobalUser Create(QQBotSocketClient client, ClientState state, User model)
    {
        SocketGlobalUser entity = new(client, model.Id.ToString(), UserScope.Guild);
        entity.Update(state, model);
        return entity;
    }

    internal static SocketGlobalUser Create(QQBotSocketClient client, ClientState state, API.Gateway.Author model)
    {
        SocketGlobalUser entity = new(client, model.Id.ToString("N").ToUpperInvariant(), UserScope.Normal);
        entity.Update(state, model);
        return entity;
    }

    /// <inheritdoc />
    internal override void Update(ClientState state, API.Gateway.Author model)
    {
        base.Update(state, model);
        OpenId = model.MemberOpenId ?? model.UserOpenId ?? model.UnionOpenId;
    }

    internal void AddRef()
    {
        checked
        {
            lock (_lockObj)
            {
                _references++;
            }
        }
    }

    internal void RemoveRef(QQBotSocketClient client)
    {
        lock (_lockObj)
        {
            if (--_references <= 0)
                client.RemoveUser(Id);
        }
    }

    private string DebuggerDisplay => $"Unknown ({Id} Global)";
}
