using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    internal void CreateRestSelfUser(API.SelfUser user) =>
        base.CurrentUser = RestSelfUser.Create(this, user);

    #endregion
}
