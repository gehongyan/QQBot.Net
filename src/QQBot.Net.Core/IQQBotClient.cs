namespace QQBot;

/// <summary>
///     表示一个通用的 QQ Bot 客户端。
/// </summary>
public interface IQQBotClient : IDisposable
{
    #region General

    /// <summary>
    ///     获取当前连接的状态。
    /// </summary>
    ConnectionState ConnectionState { get; }

    /// <summary>
    ///     获取当前已登录的用户；如果没有用户登录，则为 <c>null</c>。
    /// </summary>
    ISelfUser? CurrentUser { get; }

    /// <summary>
    ///     获取已登录用户的令牌类型。
    /// </summary>
    TokenType TokenType { get; }

    /// <summary>
    ///     启动客户端与 QQ 之间的连接。
    /// </summary>
    /// <remarks>
    ///     当前方法会初始化客户端与 QQ 之间的连接。 <br />
    ///     <note type="important">
    ///         此方法会在调用后立即返回，因为它会在另一个线程上初始化连接。
    ///     </note>
    /// </remarks>
    /// <returns> 一个表示异步启动操作的任务。 </returns>
    Task StartAsync();

    /// <summary>
    ///     停止客户端与 QQ 之间的连接。
    /// </summary>
    /// <returns> 一个表示异步停止操作的任务。 </returns>
    Task StopAsync();

    /// <summary>
    ///     登录到 QQ API。
    /// </summary>
    /// <param name="appId"> 要使用的应用 ID。 </param>
    /// <param name="tokenType"> 要使用的令牌类型。 </param>
    /// <param name="token"> 要使用的令牌。 </param>
    /// <param name="validateToken"> 是否验证令牌。 </param>
    /// <returns> 一个表示异步登录操作的任务。 </returns>
    /// <remarks>
    ///     验证令牌的操作是通过 <see cref="QQBot.TokenUtils.ValidateToken(QQBot.TokenType,System.String)"/> 方法完成的。 <br />
    ///     此方法用于向当前客户端设置后续 API 请求的身份验证信息，获取并设置当前所登录用户的信息。
    /// </remarks>
    Task LoginAsync(int appId, TokenType tokenType, string token, bool validateToken = true);

    /// <summary>
    ///     从 QQ API 退出登录。
    /// </summary>
    /// <returns> 一个表示异步退出登录操作的任务。 </returns>
    /// <remarks>
    ///     此方法用于清除当前客户端的身份验证信息及所缓存的当前所登录的用户信息。
    /// </remarks>
    Task LogoutAsync();

    #endregion

    /// <summary>
    ///     获取与机器人相关的网关信息。
    /// </summary>
    /// <param name="options"> 请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务，其结果包含与机器人相关的网关信息。 </returns>
    Task<BotGateway> GetBotGatewayAsync(RequestOptions? options = null);

    /// <summary>
    ///     获取与机器人相关带分片信息的网关信息。
    /// </summary>
    /// <param name="options"> 请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务，其结果包含与机器人相关的带分片信息的网关信息。 </returns>
    Task<BotShardedGateway> GetBotShardedGatewayAsync(RequestOptions? options = null);

    #region Guilds

    /// <summary>
    ///     获取当前用户所在的所有频道。
    /// </summary>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务，其结果包含当前用户所在的所有频道。 </returns>
    Task<IReadOnlyCollection<IGuild>> GetGuildsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    /// <summary>
    ///     获取具有指定 ID 的频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务，其结果包含与指定的 <paramref name="id"/> 关联的频道；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IGuild?> GetGuildAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

    #endregion

    /// <summary>
    ///     获取当前 Bot 资料页的分享链接。
    /// </summary>
    /// <param name="callbackData"> 要在用户添加当前 Bot 为好友时通过网关发送给 Bot 的回调数据，不应超过 32 字符，不应包含特殊字符。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务，其结果为包含当前 Bot 资料页的分享链接的 <see cref="Uri"/> 对象。 </returns>
    Task<Uri> GenerateProfileUrlAsync(string? callbackData = null, RequestOptions? options = null);

    #region Group Join Approval Strategies

    /// <summary>
    ///     获取当前机器人生效中的入群自动审批策略，以分页形式返回。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的可枚举集合，包含当前机器人生效中的入群自动审批策略。 </returns>
    IAsyncEnumerable<IReadOnlyCollection<IGroupJoinApprovalStrategy>> GetJoinApprovalStrategiesAsync(RequestOptions? options = null);

    /// <summary>
    ///     创建一个关联指定群的入群自动审批策略。
    /// </summary>
    /// <remarks>
    ///     一个机器人最多创建 20 个策略。
    /// </remarks>
    /// <param name="groupIds"> 要关联的群的标识符列表（最多 100 个）。 </param>
    /// <param name="func"> 一个包含新策略额外配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的策略。 </returns>
    Task<IGroupJoinApprovalStrategy> CreateJoinApprovalStrategyAsync(
        IEnumerable<Guid> groupIds, Action<GroupJoinApprovalStrategyProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     创建一个关联指定 QQ 群号的入群自动审批策略。
    /// </summary>
    /// <remarks>
    ///     一个机器人最多创建 20 个策略。
    /// </remarks>
    /// <param name="groupNumbers"> 要关联的 QQ 群号列表（最多 100 个）。 </param>
    /// <param name="func"> 一个包含新策略额外配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的策略。 </returns>
    Task<IGroupJoinApprovalStrategy> CreateJoinApprovalStrategyAsync(
        IEnumerable<ulong> groupNumbers, Action<GroupJoinApprovalStrategyProperties>? func = null, RequestOptions? options = null);

    #endregion

    #region Menus

    /// <summary>
    ///     获取当前机器人已设置的全局自定义菜单。
    /// </summary>
    /// <remarks>
    ///     若尚未设置过菜单，返回的 <see cref="BotMenu.Items"/> 为空集合。
    /// </remarks>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含当前的全局自定义菜单。 </returns>
    Task<BotMenu> GetMenuAsync(RequestOptions? options = null);

    /// <summary>
    ///     修改当前机器人的全局自定义菜单。
    /// </summary>
    /// <remarks>
    ///     传入的菜单项将覆盖原有的完整菜单配置。全局自定义菜单仅在 QQ 单聊（C2C）场景生效。
    /// </remarks>
    /// <param name="items"> 要设置的菜单项列表，最多 10 个。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改操作的任务。任务的结果包含修改后的全局自定义菜单。 </returns>
    Task<BotMenu> ModifyMenuAsync(IEnumerable<MenuItem> items, RequestOptions? options = null);

    #endregion

    #region Command Panels

    /// <summary>
    ///     获取指定场景下当前生效的指令面板，以分页形式返回。
    /// </summary>
    /// <param name="scope"> 要筛选的生效场景。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的可枚举集合，包含指定场景下的指令面板。 </returns>
    IAsyncEnumerable<IReadOnlyCollection<ICommandPanel>> GetCommandPanelsAsync(
        CommandPanelScope scope, RequestOptions? options = null);

    /// <summary>
    ///     获取指定的指令面板。
    /// </summary>
    /// <param name="panelId"> 要获取的面板的标识符。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含指定的指令面板。 </returns>
    Task<ICommandPanel> GetCommandPanelAsync(string panelId, RequestOptions? options = null);

    /// <summary>
    ///     创建一个对指定场景下所有用户或群生效的指令面板。
    /// </summary>
    /// <remarks>
    ///     一个机器人最多创建 20 个指令面板。
    /// </remarks>
    /// <param name="scope"> 面板的生效场景。 </param>
    /// <param name="items"> 面板的元素列表，最多 20 个。 </param>
    /// <param name="func"> 一个包含面板额外配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的指令面板。 </returns>
    Task<ICommandPanel> CreateCommandPanelAsync(CommandPanelScope scope, IEnumerable<CommandPanelItem> items,
        Action<CommandPanelProperties>? func = null, RequestOptions? options = null);

    /// <summary>
    ///     创建一个仅对指定用户或群生效的指令面板。
    /// </summary>
    /// <remarks>
    ///     仅 <see cref="CommandPanelScope.C2C"/> 与 <see cref="CommandPanelScope.Group"/> 场景支持按指定对象生效。
    ///     一个机器人最多创建 20 个指令面板。
    /// </remarks>
    /// <param name="scope"> 面板的生效场景，仅支持 C2C 或 Group。 </param>
    /// <param name="targetIds"> 面板生效的用户或群的标识符集合，单次最多 20 个。 </param>
    /// <param name="items"> 面板的元素列表，最多 20 个。 </param>
    /// <param name="func"> 一个包含面板额外配置的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步创建操作的任务。任务的结果包含新创建的指令面板。 </returns>
    Task<ICommandPanel> CreateCommandPanelAsync(CommandPanelScope scope, IEnumerable<Guid> targetIds,
        IEnumerable<CommandPanelItem> items, Action<CommandPanelProperties>? func = null, RequestOptions? options = null);

    #endregion
}
