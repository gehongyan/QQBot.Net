using System.Collections.Immutable;
using System.Diagnostics;
using QQBot.Rest;
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

    /// <summary>
    ///     获取此频道内的所有用户。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此频道内的所有用户。 </returns>
    public IAsyncEnumerable<IReadOnlyCollection<RestGuildMember>> GetUsersAsync(RequestOptions? options = null) =>
        RoleHelper.GetUsersAsync(this, Client, null, options);

    /// <inheritdoc cref="QQBot.WebSocket.SocketRole.Name" />
    public override string ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id})";
    internal SocketRole Clone() => (SocketRole)MemberwiseClone();

    #region IRole

    /// <inheritdoc />
    IGuild IRole.Guild => Guild;

    IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> IRole.GetUsersAsync(CacheMode mode, RequestOptions? options)
    {
        if (!Guild.HasAllMembers && mode == CacheMode.AllowDownload)
            return GetUsersAsync(options);
        IReadOnlyCollection<SocketGuildMember> userCollections = Guild.Users
            .Where(u => u.Roles?.Contains(this) is true)
            .ToImmutableList();
        return ImmutableArray.Create(userCollections).ToAsyncEnumerable();
    }

    #endregion
}
