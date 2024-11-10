namespace QQBot;

/// <summary>
///     表示一个子频道内用户
/// </summary>
public interface IGuildMember : IGuildUser
{
    /// <summary>
    ///     获取此频道用户所属的频道。
    /// </summary>
    IGuild Guild { get; }

    /// <summary>
    ///     获取此用户所属频道的 ID。
    /// </summary>
    ulong GuildId { get; }

    /// <summary>
    ///     获取此用户的昵称。
    /// </summary>
    string? Nickname { get; }

    /// <summary>
    ///     获取此用户在该子频道内拥有的所有身份组的 ID。
    /// </summary>
    IReadOnlyCollection<uint>? RoleIds { get; }

    /// <summary>
    ///     获取此用户加入该子频道的时间。
    /// </summary>
    DateTimeOffset? JoinedAt { get; }

    /// <summary>
    ///     将此用户从此子频道中踢出。
    /// </summary>
    /// <param name="addBlacklist"> 是否在踢出成员的同时将其加入黑名单。 </param>
    /// <param name="pruneDays">
    ///     撤回指定时间范围内的消息，支持的值有：
    ///     <list type="table">
    ///         <listheader> <term> 值 </term> <description> 释义 </description> </listheader>
    ///         <item> <term> <c>0</c> </term> <description> 不撤回任何消息。 </description> </item>
    ///         <item> <term> <c>3</c>、<c>7</c>、<c>15</c>、<c>30</c> </term> <description> 撤回指定天数之前至当前的所有消息。 </description> </item>
    ///         <item> <term> <c>-1</c> </term> <description> 撤回全部消息。 </description> </item>
    ///     </list>
    /// </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步踢出操作的任务。 </returns>
    Task KickAsync(bool addBlacklist = false, int pruneDays = 0, RequestOptions? options = null);

    #region Roles

    /// <summary>
    ///     在该服务器内授予此用户指定的角色。
    /// </summary>
    /// <param name="roleId"> 要在该服务器内为此用户授予的角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步授予操作的任务。 </returns>
    Task AddRoleAsync(uint roleId, RequestOptions? options = null);

    /// <summary>
    ///     在该服务器内授予此用户指定的角色。
    /// </summary>
    /// <param name="role"> 要在该服务器内为此用户授予的角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步授予操作的任务。 </returns>
    Task AddRoleAsync(IRole role, RequestOptions? options = null);

    /// <summary>
    ///     在该服务器内授予此用户指定的一些角色。
    /// </summary>
    /// <param name="roleIds"> 要在该服务器内为此用户授予的所有角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步授予操作的任务。 </returns>
    Task AddRolesAsync(IEnumerable<uint> roleIds, RequestOptions? options = null);

    /// <summary>
    ///     在该服务器内授予此用户指定的一些角色。
    /// </summary>
    /// <param name="roles"> 要在该服务器内为此用户授予的所有角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步授予操作的任务。 </returns>
    Task AddRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null);

    /// <summary>
    ///     在该服务器内撤销此用户指定的角色。
    /// </summary>
    /// <param name="roleId"> 要在该服务器内为此用户撤销的角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤销操作的任务。 </returns>
    Task RemoveRoleAsync(uint roleId, RequestOptions? options = null);

    /// <summary>
    ///     在该服务器内撤销此用户指定的角色。
    /// </summary>
    /// <param name="role"> 要在该服务器内为此用户撤销的角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤销操作的任务。 </returns>
    Task RemoveRoleAsync(IRole role, RequestOptions? options = null);

    /// <summary>
    ///     在该服务器内撤销此用户指定的一些角色。
    /// </summary>
    /// <param name="roleIds"> 要在该服务器内为此用户撤销的所有角色的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤销操作的任务。 </returns>
    Task RemoveRolesAsync(IEnumerable<uint> roleIds, RequestOptions? options = null);

    /// <summary>
    ///     在该服务器内撤销此用户指定的一些角色。
    /// </summary>
    /// <param name="roles"> 要在该服务器内为此用户撤销的所有角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤销操作的任务。 </returns>
    Task RemoveRolesAsync(IEnumerable<IRole> roles, RequestOptions? options = null);

    #endregion
}
