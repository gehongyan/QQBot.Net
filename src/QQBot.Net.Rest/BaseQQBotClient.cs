using QQBot.Logging;

namespace QQBot.Rest;

/// <summary>
///     表示一个可以连接到 QQ API 的通用的 QQ Bot 客户端。
/// </summary>
public abstract class BaseQQBotClient : IQQBotClient
{
    #region BaseQQBotClient

    /// <summary>
    ///     当生成一条日志消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.LogMessage"/> 参数是描述日志消息的结构。 </item>
    ///     </list>
    /// </remarks>
    public event Func<LogMessage, Task> Log
    {
        add => _logEvent.Add(value);
        remove => _logEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<LogMessage, Task>> _logEvent = new();

    /// <summary>
    ///     当客户端登录成功时引发。
    /// </summary>
    public event Func<Task> LoggedIn
    {
        add => _loggedInEvent.Add(value);
        remove => _loggedInEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Task>> _loggedInEvent = new();

    /// <summary>
    ///     当客户端退出登录时引发。
    /// </summary>
    public event Func<Task> LoggedOut
    {
        add => _loggedOutEvent.Add(value);
        remove => _loggedOutEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<Task>> _loggedOutEvent = new();

    /// <summary>
    ///     当向 API 发送 REST 请求时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="System.Net.Http.HttpMethod"/> 参数是 HTTP 方法。 </item>
    ///     <item> <see cref="System.String"/> 参数是终结点。 </item>
    ///     <item> <see cref="System.Double"/> 参数是完成请求所花费的时间，以毫秒为单位。 </item>
    ///     </list>
    /// </remarks>
    public event Func<HttpMethod, string, double, Task> SentRequest
    {
        add => _sentRequest.Add(value);
        remove => _sentRequest.Remove(value);
    }

    internal readonly AsyncEvent<Func<HttpMethod, string, double, Task>> _sentRequest = new();

    internal readonly Logger _restLogger;
    private readonly SemaphoreSlim _stateLock;
    private bool _isFirstLogin, _isDisposed;

    internal API.QQBotRestApiClient ApiClient { get; }

    internal LogManager LogManager { get; }

    /// <summary>
    ///     获取登录到此客户端的当前用户；如果未登录，则为 <c>null</c>。
    /// </summary>
    public ISelfUser? CurrentUser { get; protected set; }

    /// <summary>
    ///     获取此客户端的登录状态。
    /// </summary>
    public LoginState LoginState { get; protected set; }

    /// <inheritdoc />
    public TokenType TokenType => ApiClient.AuthTokenType;

    internal MessageSequenceGenerationParameters MessageSequenceGenerationParameters { get; }

    internal BaseQQBotClient(QQBotRestConfig config, API.QQBotRestApiClient client)
    {
        ApiClient = client;
        LogManager = new LogManager(config.LogLevel);
        LogManager.Message += async msg => await _logEvent.InvokeAsync(msg).ConfigureAwait(false);

        _stateLock = new SemaphoreSlim(1, 1);
        _restLogger = LogManager.CreateLogger("Rest");
        _isFirstLogin = config.DisplayInitialLog;
        MessageSequenceGenerationParameters = config.MessageSequenceGenerationParameters;

        ApiClient.RequestQueue.RateLimitTriggered += async (id, info, endpoint) =>
        {
            if (info == null)
                await _restLogger.VerboseAsync($"Preemptive Rate limit triggered: {endpoint} {(id.IsHashBucket ? $"(Bucket: {id.BucketHash})" : "")}").ConfigureAwait(false);
            else
                await _restLogger.WarningAsync($"Rate limit triggered: {endpoint} {(id.IsHashBucket ? $"(Bucket: {id.BucketHash})" : "")}").ConfigureAwait(false);
        };
        ApiClient.SentRequest += async (method, endpoint, millis) =>
            await _restLogger.VerboseAsync($"{method} {endpoint}: {millis} ms").ConfigureAwait(false);
        ApiClient.SentRequest += (method, endpoint, millis) =>
            _sentRequest.InvokeAsync(method, endpoint, millis);
    }

    internal virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            _isDisposed = true;
        }
    }

    /// <inheritdoc />
    public void Dispose() => Dispose(true);

    /// <inheritdoc />
    public async Task LoginAsync(int appId, TokenType tokenType, string token, bool validateToken = true)
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await LoginInternalAsync(appId, tokenType, token, validateToken).ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    internal virtual async Task LoginInternalAsync(int appId, TokenType tokenType, string token, bool validateToken)
    {
        if (_isFirstLogin)
        {
            _isFirstLogin = false;
            await LogManager.WriteInitialLog().ConfigureAwait(false);
        }

        if (LoginState != LoginState.LoggedOut)
            await LogoutInternalAsync().ConfigureAwait(false);

        LoginState = LoginState.LoggingIn;

        try
        {
            // If token validation is enabled, validate the token and let it throw any ArgumentExceptions
            // that result from invalid parameters
            if (validateToken)
            {
                try
                {
                    TokenUtils.ValidateToken(tokenType, token);
                }
                catch (ArgumentException ex)
                {
                    // log these ArgumentExceptions and allow for the client to attempt to log in anyways
                    await LogManager.WarningAsync("QQBot", "A supplied token was invalid.", ex).ConfigureAwait(false);
                }
            }

            await ApiClient.LoginAsync(appId, tokenType, token).ConfigureAwait(false);
            await OnLoginAsync(appId, tokenType, token).ConfigureAwait(false);
            LoginState = LoginState.LoggedIn;
        }
        catch
        {
            await LogoutInternalAsync().ConfigureAwait(false);
            throw;
        }

        await _loggedInEvent.InvokeAsync().ConfigureAwait(false);
    }

    internal virtual Task OnLoginAsync(int appId, TokenType tokenType, string token) => Task.CompletedTask;

    /// <inheritdoc />
    public async Task LogoutAsync()
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await LogoutInternalAsync().ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    internal virtual async Task LogoutInternalAsync()
    {
        if (LoginState == LoginState.LoggedOut)
            return;
        LoginState = LoginState.LoggingOut;
        await ApiClient.LogoutAsync().ConfigureAwait(false);
        await OnLogoutAsync().ConfigureAwait(false);
        CurrentUser = null;
        LoginState = LoginState.LoggedOut;
        await _loggedOutEvent.InvokeAsync().ConfigureAwait(false);
    }

    internal virtual Task OnLogoutAsync() => Task.CompletedTask;

    /// <inheritdoc />
    public virtual ConnectionState ConnectionState => ConnectionState.Disconnected;

    /// <inheritdoc />
    public Task<BotGateway> GetBotGatewayAsync(RequestOptions? options = null) =>
        ClientHelper.GetBotGatewayAsync(this, options);

    /// <inheritdoc />
    public Task<BotShardedGateway> GetBotShardedGatewayAsync(RequestOptions? options = null) =>
        ClientHelper.GetBotShardedGatewayAsync(this, options);

    #endregion

    #region IQQBotClient

    /// <inheritdoc />
    ISelfUser? IQQBotClient.CurrentUser => CurrentUser;

    /// <inheritdoc />
    Task IQQBotClient.StartAsync() => Task.CompletedTask;

    /// <inheritdoc />
    Task IQQBotClient.StopAsync() => Task.CompletedTask;

    /// <inheritdoc />
    Task<IReadOnlyCollection<IGuild>> IQQBotClient.GetGuildsAsync(CacheMode mode, RequestOptions? options)
        => Task.FromResult<IReadOnlyCollection<IGuild>>([]);

    /// <inheritdoc />
    Task<IGuild?> IQQBotClient.GetGuildAsync(ulong id, CacheMode mode, RequestOptions? options)
        => Task.FromResult<IGuild?>(null);

    /// <inheritdoc />
    Task<Uri> IQQBotClient.GenerateProfileUrlAsync(string? callbackData, RequestOptions? options) =>
        Task.FromResult<Uri>(null!);

    #endregion

    #region Group Join Approval Strategies

    /// <inheritdoc cref="QQBot.IQQBotClient.GetJoinApprovalStrategiesAsync(QQBot.RequestOptions)" />
    public IAsyncEnumerable<IReadOnlyCollection<RestGroupJoinApprovalStrategy>> GetJoinApprovalStrategiesAsync(
        RequestOptions? options = null) =>
        StrategyHelper.GetStrategiesAsync(this, options);

    /// <inheritdoc cref="QQBot.IQQBotClient.CreateJoinApprovalStrategyAsync(System.Collections.Generic.IEnumerable{System.Guid},System.Action{QQBot.GroupJoinApprovalStrategyProperties},QQBot.RequestOptions)" />
    public Task<RestGroupJoinApprovalStrategy> CreateJoinApprovalStrategyAsync(
        IEnumerable<Guid> groupIds, Action<GroupJoinApprovalStrategyProperties>? func = null, RequestOptions? options = null) =>
        StrategyHelper.CreateStrategyAsync(this, groupIds, func, options);

    /// <inheritdoc cref="QQBot.IQQBotClient.CreateJoinApprovalStrategyAsync(System.Collections.Generic.IEnumerable{System.UInt64},System.Action{QQBot.GroupJoinApprovalStrategyProperties},QQBot.RequestOptions)" />
    public Task<RestGroupJoinApprovalStrategy> CreateJoinApprovalStrategyAsync(
        IEnumerable<ulong> groupNumbers, Action<GroupJoinApprovalStrategyProperties>? func = null, RequestOptions? options = null) =>
        StrategyHelper.CreateStrategyAsync(this, groupNumbers, func, options);

    /// <inheritdoc />
    IAsyncEnumerable<IReadOnlyCollection<IGroupJoinApprovalStrategy>> IQQBotClient.GetJoinApprovalStrategiesAsync(
        RequestOptions? options) =>
        GetJoinApprovalStrategiesAsync(options);

    /// <inheritdoc />
    async Task<IGroupJoinApprovalStrategy> IQQBotClient.CreateJoinApprovalStrategyAsync(
        IEnumerable<Guid> groupIds, Action<GroupJoinApprovalStrategyProperties>? func, RequestOptions? options) =>
        await CreateJoinApprovalStrategyAsync(groupIds, func, options).ConfigureAwait(false);

    /// <inheritdoc />
    async Task<IGroupJoinApprovalStrategy> IQQBotClient.CreateJoinApprovalStrategyAsync(
        IEnumerable<ulong> groupNumbers, Action<GroupJoinApprovalStrategyProperties>? func, RequestOptions? options) =>
        await CreateJoinApprovalStrategyAsync(groupNumbers, func, options).ConfigureAwait(false);

    #endregion

    #region Menus

    /// <inheritdoc cref="QQBot.IQQBotClient.GetMenuAsync(QQBot.RequestOptions)" />
    public Task<BotMenu> GetMenuAsync(RequestOptions? options = null) =>
        MenuHelper.GetMenuAsync(this, options);

    /// <inheritdoc cref="QQBot.IQQBotClient.ModifyMenuAsync(System.Collections.Generic.IEnumerable{QQBot.MenuItem},QQBot.RequestOptions)" />
    public Task<BotMenu> ModifyMenuAsync(IEnumerable<MenuItem> items, RequestOptions? options = null) =>
        MenuHelper.ModifyMenuAsync(this, items, options);

    #endregion

    #region Command Panels

    /// <inheritdoc cref="QQBot.IQQBotClient.GetCommandPanelsAsync(QQBot.CommandPanelScope,QQBot.RequestOptions)" />
    public IAsyncEnumerable<IReadOnlyCollection<RestCommandPanel>> GetCommandPanelsAsync(
        CommandPanelScope scope, RequestOptions? options = null) =>
        PanelHelper.GetPanelsAsync(this, scope, options);

    /// <inheritdoc cref="QQBot.IQQBotClient.GetCommandPanelAsync(System.String,QQBot.RequestOptions)" />
    public Task<RestCommandPanel> GetCommandPanelAsync(string panelId, RequestOptions? options = null) =>
        PanelHelper.GetPanelAsync(this, panelId, options);

    /// <inheritdoc cref="QQBot.IQQBotClient.CreateCommandPanelAsync(QQBot.CommandPanelScope,System.Collections.Generic.IEnumerable{QQBot.CommandPanelItem},System.Action{QQBot.CommandPanelProperties},QQBot.RequestOptions)" />
    public Task<RestCommandPanel> CreateCommandPanelAsync(CommandPanelScope scope, IEnumerable<CommandPanelItem> items,
        Action<CommandPanelProperties>? func = null, RequestOptions? options = null) =>
        PanelHelper.CreatePanelAsync(this, scope, null, items, func, options);

    /// <inheritdoc cref="QQBot.IQQBotClient.CreateCommandPanelAsync(QQBot.CommandPanelScope,System.Collections.Generic.IEnumerable{System.Guid},System.Collections.Generic.IEnumerable{QQBot.CommandPanelItem},System.Action{QQBot.CommandPanelProperties},QQBot.RequestOptions)" />
    public Task<RestCommandPanel> CreateCommandPanelAsync(CommandPanelScope scope, IEnumerable<Guid> targetIds,
        IEnumerable<CommandPanelItem> items, Action<CommandPanelProperties>? func = null, RequestOptions? options = null) =>
        PanelHelper.CreatePanelAsync(this, scope, targetIds, items, func, options);

    /// <inheritdoc />
    IAsyncEnumerable<IReadOnlyCollection<ICommandPanel>> IQQBotClient.GetCommandPanelsAsync(
        CommandPanelScope scope, RequestOptions? options) =>
        GetCommandPanelsAsync(scope, options);

    /// <inheritdoc />
    async Task<ICommandPanel> IQQBotClient.GetCommandPanelAsync(string panelId, RequestOptions? options) =>
        await GetCommandPanelAsync(panelId, options).ConfigureAwait(false);

    /// <inheritdoc />
    async Task<ICommandPanel> IQQBotClient.CreateCommandPanelAsync(CommandPanelScope scope,
        IEnumerable<CommandPanelItem> items, Action<CommandPanelProperties>? func, RequestOptions? options) =>
        await CreateCommandPanelAsync(scope, items, func, options).ConfigureAwait(false);

    /// <inheritdoc />
    async Task<ICommandPanel> IQQBotClient.CreateCommandPanelAsync(CommandPanelScope scope,
        IEnumerable<Guid> targetIds, IEnumerable<CommandPanelItem> items,
        Action<CommandPanelProperties>? func, RequestOptions? options) =>
        await CreateCommandPanelAsync(scope, targetIds, items, func, options).ConfigureAwait(false);

    #endregion
}
