namespace QQBot;

/// <summary>
///     表示一个指令面板。
/// </summary>
public interface ICommandPanel : IEntity<string>, IDeletable
{
    /// <summary>
    ///     获取此面板的唯一标识符。
    /// </summary>
    new string Id { get; }

    /// <summary>
    ///     获取此面板的生效场景。
    /// </summary>
    CommandPanelScope Scope { get; }

    /// <summary>
    ///     获取此面板的作用范围。
    /// </summary>
    CommandPanelTargetType TargetType { get; }

    /// <summary>
    ///     获取此面板的元素列表。
    /// </summary>
    IReadOnlyCollection<CommandPanelItem> Items { get; }

    /// <summary>
    ///     获取此面板的备注。
    /// </summary>
    string? Remark { get; }

    /// <summary>
    ///     获取此面板的版本号。
    /// </summary>
    int Version { get; }

    /// <summary>
    ///     获取此面板的创建时间。
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    ///     获取此面板的最近更新时间。
    /// </summary>
    DateTimeOffset UpdatedAt { get; }

    /// <summary>
    ///     修改此面板的元素与备注。
    /// </summary>
    /// <remarks>
    ///     传入的元素列表将覆盖原有配置，不影响已关联的用户或群列表。
    /// </remarks>
    /// <param name="items"> 要设置的面板元素列表，最多 20 个。 </param>
    /// <param name="func"> 一个包含面板额外配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改操作的任务。 </returns>
    Task ModifyAsync(IEnumerable<CommandPanelItem> items,
        Action<CommandPanelProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     为此面板添加关联的用户或群。
    /// </summary>
    /// <remarks>
    ///     仅当面板的作用范围为 <see cref="CommandPanelTargetType.Specific"/> 时可用。单次最多操作 20 个。
    /// </remarks>
    /// <param name="targetIds"> 要添加的用户或群的标识符集合。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task AddTargetsAsync(IEnumerable<Guid> targetIds, RequestOptions? options = null);

    /// <summary>
    ///     从此面板移除关联的用户或群。
    /// </summary>
    /// <remarks>
    ///     仅当面板的作用范围为 <see cref="CommandPanelTargetType.Specific"/> 时可用。单次最多操作 20 个。
    /// </remarks>
    /// <param name="targetIds"> 要移除的用户或群的标识符集合。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RemoveTargetsAsync(IEnumerable<Guid> targetIds, RequestOptions? options = null);
}
