using System.Diagnostics;
using Model = QQBot.API.Role;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道角色。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketRole : SocketEntity<uint>, IRole
{
    /// <inheritdoc cref="QQBot.IRole.Guild" />
    public SocketGuild Guild { get; }

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public RoleType Type { get; private set; }

    /// <inheritdoc />
    public AlphaColor Color { get; private set; }

    /// <inheritdoc />
    public bool IsHoisted { get; private set; }

    /// <inheritdoc />
    public int MemberCount { get; private set; }

    /// <inheritdoc />
    public int MaxMembers { get; private set; }

    internal SocketRole(SocketGuild guild, uint id)
        : base(guild.Client, id)
    {
        Name = string.Empty;
        Guild = guild;
    }

    internal static SocketRole Create(SocketGuild guild, ClientState state, Model model)
    {
        SocketRole entity = new(guild, model.Id);
        entity.Update(state, model);
        return entity;
    }

    internal void Update(ClientState state, Model model)
    {
        Name = model.Name;
        Color = model.Color;
        IsHoisted = model.Hoist;
        MemberCount = model.Number;
        MaxMembers = model.MemberLimit;
    }

    /// <inheritdoc cref="QQBot.WebSocket.SocketRole.Name" />
    public override string ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id})";
    internal SocketRole Clone() => (SocketRole)MemberwiseClone();

    #region IRole

    /// <inheritdoc />
    IGuild IRole.Guild => Guild;

    #endregion
}
