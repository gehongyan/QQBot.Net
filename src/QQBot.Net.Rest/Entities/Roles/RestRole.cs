using System.Collections.Immutable;
using System.Diagnostics;
using Model = QQBot.API.Role;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于网关的频道角色。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestRole : RestEntity<uint>, IRole
{
    /// <inheritdoc cref="QQBot.IRole.Guild" />
    public IGuild Guild { get; }

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

    internal RestRole(IGuild guild, BaseQQBotClient client, uint id)
        : base(client, id)
    {
        Name = string.Empty;
        Guild = guild;
    }

    internal static RestRole Create(RestGuild guild, Model model) =>
        Create(guild, guild.Client, model);

    internal static RestRole Create(IGuild guild, BaseQQBotClient client, Model model)
    {
        RestRole entity = new(guild, client, model.Id);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
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

    /// <inheritdoc cref="Name" />
    public override string ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id})";
    internal RestRole Clone() => (RestRole)MemberwiseClone();

    #region IRole

    /// <inheritdoc />
    IGuild IRole.Guild => Guild;

    IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> IRole.GetUsersAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? GetUsersAsync(options) : ImmutableArray<IReadOnlyCollection<IGuildMember>>.Empty.ToAsyncEnumerable();

    #endregion
}
