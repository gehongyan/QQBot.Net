namespace QQBot;

/// <summary>
///     表示一个入群自动审批策略。
/// </summary>
public interface IGroupJoinApprovalStrategy : IEntity<string>, IDeletable
{
    /// <summary>
    ///     获取此策略的唯一标识符。
    /// </summary>
    new string Id { get; }

    /// <summary>
    ///     获取此策略关联的群的标识符列表。
    /// </summary>
    /// <remarks>
    ///     此列表与 <see cref="GroupNumbers"/> 互斥：仅当创建策略时使用群标识符关联时才会返回，否则为空集合。
    /// </remarks>
    IReadOnlyCollection<Guid> GroupIds { get; }

    /// <summary>
    ///     获取此策略关联的 QQ 群号列表。
    /// </summary>
    /// <remarks>
    ///     此列表与 <see cref="GroupIds"/> 互斥：仅当创建策略时使用 QQ 群号关联时才会返回，否则为空集合。
    /// </remarks>
    IReadOnlyCollection<ulong> GroupNumbers { get; }

    /// <summary>
    ///     获取此策略白名单中的号码数量（估算，可能存在少量误差）。
    /// </summary>
    int WhitelistUserCount { get; }

    /// <summary>
    ///     获取此策略是否启用。
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    ///     获取此策略的过期时间。
    /// </summary>
    DateTimeOffset ExpiresAt { get; }

    /// <summary>
    ///     获取此策略的创建时间。
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    ///     获取此策略的最近更新时间。
    /// </summary>
    DateTimeOffset UpdatedAt { get; }

    /// <summary>
    ///     获取此策略的备注。
    /// </summary>
    string? Remark { get; }

    /// <summary>
    ///     修改此策略。
    /// </summary>
    /// <param name="func"> 一个包含修改策略属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改操作的任务。 </returns>
    Task ModifyAsync(Action<ModifyGroupJoinApprovalStrategyProperties> func, RequestOptions? options = null);

    /// <summary>
    ///     执行此策略。
    /// </summary>
    /// <remarks>
    ///     对策略关联的全部群发起全量扫描，命中白名单号码的入群申请将被自动审批通过。此操作为异步执行，约 10 分钟完成。
    /// </remarks>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步执行操作的任务。 </returns>
    Task ExecuteAsync(RequestOptions? options = null);

    /// <summary>
    ///     将指定 QQ 号码加入此策略的白名单。
    /// </summary>
    /// <remarks>
    ///     单次最多操作 10000 个号码，号码总数上限为 10 万。
    /// </remarks>
    /// <param name="qqNumbers"> 要加入白名单的 QQ 号码列表。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task AddWhitelistAsync(IEnumerable<string> qqNumbers, RequestOptions? options = null);

    /// <summary>
    ///     将指定 QQ 号码移出此策略的白名单。
    /// </summary>
    /// <remarks>
    ///     单次最多操作 10000 个号码。
    /// </remarks>
    /// <param name="qqNumbers"> 要移出白名单的 QQ 号码列表。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RemoveWhitelistAsync(IEnumerable<string> qqNumbers, RequestOptions? options = null);
}
