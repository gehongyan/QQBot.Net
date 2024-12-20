namespace QQBot;

/// <summary>
///     表示一个子频道身份组。
/// </summary>
public interface IRole : IEntity<uint>, IDeletable
{
    /// <summary>
    ///     获取拥有此角色的频道。
    /// </summary>
    IGuild Guild { get; }

    /// <summary>
    ///     获取此身份组的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此身份组的类型。
    /// </summary>
    RoleType Type { get; }

    /// <summary>
    ///     获取此身份组的颜色。
    /// </summary>
    AlphaColor Color { get; }

    /// <summary>
    ///     获取拥有此身份组的用户是否在用户列表中与普通在线成员分开显示。
    /// </summary>
    bool IsHoisted { get; }

    /// <summary>
    ///     获取拥有此身分组的用户数量。
    /// </summary>
    int MemberCount { get; }

    /// <summary>
    ///     获取可以拥有此身份组的最大用户数量。
    /// </summary>
    int MaxMembers { get; }

    /// <summary>
    ///     获取拥有此身份组的所有用户。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的可枚举集合，结果包含所有拥有此身份组的用户。 </returns>
    IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> GetUsersAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     修改此角色。
    /// </summary>
    /// <remarks>
    ///     此方法使用指定的属性修改当前角色信息。要查看可用的属性，请参考 <see cref="QQBot.RoleProperties"/>。
    /// </remarks>
    /// <param name="func"> 一个包含修改角色属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改操作的任务。 </returns>
    Task ModifyAsync(Action<RoleProperties> func, RequestOptions? options = null);
}
