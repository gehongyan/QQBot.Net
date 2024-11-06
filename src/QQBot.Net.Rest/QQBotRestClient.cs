using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using QQBot.API;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的 QQ Bot 客户端。
/// </summary>
public class QQBotRestClient : BaseQQBotClient, IQQBotClient
{
    #region QQBotRestClient

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    /// <inheritdoc cref="QQBot.Rest.BaseQQBotClient.CurrentUser" />
    public new RestSelfUser? CurrentUser
    {
        get => base.CurrentUser as RestSelfUser;
        internal set => base.CurrentUser = value;
    }

    /// <summary>
    ///     使用默认配置初始化一个 <see cref="QQBotRestClient"/> 类的新实例。
    /// </summary>
    public QQBotRestClient()
        : this(new QQBotRestConfig())
    {
    }

    /// <summary>
    ///     使用指定的配置初始化一个 <see cref="QQBotRestClient"/> 类的新实例。
    /// </summary>
    /// <param name="config"> 用于初始化客户端的配置。 </param>
    public QQBotRestClient(QQBotRestConfig config)
        : base(config, CreateApiClient(config))
    {
    }

    internal QQBotRestClient(QQBotRestConfig config, API.QQBotRestApiClient api)
        : base(config, api)
    {
    }

    private static API.QQBotRestApiClient CreateApiClient(QQBotRestConfig config) =>
        new(config.RestClientProvider, config.AccessEnvironment, QQBotConfig.UserAgent,
            config.DefaultRetryMode, SerializerOptions);

    internal override void Dispose(bool disposing)
    {
        if (disposing) ApiClient.Dispose();
        base.Dispose(disposing);
    }

    /// <inheritdoc />
    internal override async Task OnLoginAsync(int appId, TokenType tokenType, string token)
    {
        RequestOptions requestOptions = new() { RetryMode = RetryMode.AlwaysRetry };
        API.SelfUser user = await ApiClient.GetSelfUserAsync(requestOptions).ConfigureAwait(false);
        ApiClient.CurrentUserId = user.Id;
        base.CurrentUser = RestSelfUser.Create(this, user);
    }

    #endregion

    #region Users

    [MemberNotNull(nameof(CurrentUser))]
    internal void CreateRestSelfUser(API.SelfUser user) =>
        CurrentUser = RestSelfUser.Create(this, user);

    /// <summary>
    ///     获取当前登录用户的信息。
    /// </summary>
    /// <param name="options"> 请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务，其结果包含当前登录用户的信息。 </returns>
    public async Task<RestSelfUser> GetCurrentUserAsync(RequestOptions? options = null)
    {
        SelfUser model = await ApiClient.GetSelfUserAsync(options);
        if (CurrentUser is null)
            CreateRestSelfUser(model);
        else
            CurrentUser.Update(model);
        return CurrentUser;
    }

    #endregion

    #region Guilds

    /// <summary>
    ///     获取当前用户所在的所有频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务，其结果包含当前用户所在的所有频道。 </returns>
    public async Task<IReadOnlyCollection<RestGuild>> GetGuildsAsync(RequestOptions? options = null)
    {
        IEnumerable<Guild> models = await ClientHelper.GetGuildsAsync(this, null, options).FlattenAsync().ConfigureAwait(false);
        return models.Select(x => RestGuild.Create(this, x)).ToArray();
    }

    /// <summary>
    ///     获取具有指定 ID 的频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务，其结果包含与指定的 <paramref name="id"/> 关联的频道；如果未找到，则返回 <c>null</c>。 </returns>
    public Task<RestGuild> GetGuildAsync(ulong id, RequestOptions? options = null) =>
        ClientHelper.GetGuildAsync(this, id, options);

    #endregion

    #region IQQBotClient

    /// <inheritdoc />
    async Task<IReadOnlyCollection<IGuild>> IQQBotClient.GetGuildsAsync(CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetGuildsAsync(options).ConfigureAwait(false) : [];

    async Task<IGuild?> IQQBotClient.GetGuildAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        mode == CacheMode.AllowDownload ? await GetGuildAsync(id, options).ConfigureAwait(false) : null;

    #endregion
}
