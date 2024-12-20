using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using QQBot.API.Rest;
using QQBot.Net;
using QQBot.Net.Queue;
using QQBot.Net.Rest;

namespace QQBot.API;

internal class QQBotRestApiClient : IDisposable
{
    #region QQBotRestApiClient

    private static readonly ConcurrentDictionary<string, Func<BucketIds, BucketId>> _bucketIdGenerators = new();

    public event Func<HttpMethod, string, double, Task> SentRequest
    {
        add => _sentRequestEvent.Add(value);
        remove => _sentRequestEvent.Remove(value);
    }

    private readonly AsyncEvent<Func<HttpMethod, string, double, Task>> _sentRequestEvent = new();

    protected readonly JsonSerializerOptions _serializerOptions;
    protected readonly SemaphoreSlim _stateLock;
    private readonly RestClientProvider _restClientProvider;

    protected bool _isDisposed;
    private CancellationTokenSource? _loginCancellationToken;

    public RetryMode DefaultRetryMode { get; }
    public AccessEnvironment AccessEnvironment { get; }
    public string UserAgent { get; }


    internal RequestQueue RequestQueue { get; }
    public LoginState LoginState { get; private set; }
    public int? AppId { get; private set; }
    public TokenType AuthTokenType { get; private set; }
    internal string? AuthToken { get; private set; }
    internal IRestClient RestClient { get; private set; }
    internal ulong? CurrentUserId { get; set; }
    internal Func<IRateLimitInfo, Task>? DefaultRatelimitCallback { get; set; }

    public QQBotRestApiClient(RestClientProvider restClientProvider,
        AccessEnvironment accessEnvironment, string userAgent,
        RetryMode defaultRetryMode = RetryMode.AlwaysRetry,
        JsonSerializerOptions? serializerOptions = null,
        Func<IRateLimitInfo, Task>? defaultRatelimitCallback = null)
    {
        _restClientProvider = restClientProvider;
        AccessEnvironment = accessEnvironment;
        UserAgent = userAgent;
        DefaultRetryMode = defaultRetryMode;
        _serializerOptions = serializerOptions
            ?? new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        DefaultRatelimitCallback = defaultRatelimitCallback;

        RequestQueue = new RequestQueue();
        _stateLock = new SemaphoreSlim(1, 1);
        SetBaseUrl();
    }

    [MemberNotNull(nameof(RestClient))]
    internal void SetBaseUrl()
    {
        string baseUrl = AccessEnvironment switch
        {
            AccessEnvironment.Production => QQBotConfig.APIUrl,
            AccessEnvironment.Sandbox => QQBotConfig.SandboxAPIUrl,
            _ => throw new ArgumentOutOfRangeException()
        };
        RestClient?.Dispose();
        RestClient = _restClientProvider(baseUrl);
        RestClient.SetHeader("Accept", "*/*");
        RestClient.SetHeader("User-Agent", UserAgent);
    }

    internal static string GetPrefixedToken(TokenType tokenType, int appId, string token) =>
        tokenType switch
        {
            TokenType.BotToken => $"Bot {appId}.{token}",
            TokenType.BearerToken => $"Bearer {token}",
            TokenType.AppSecret => $"QQBot {token}",
            _ => throw new ArgumentException("Unknown token type.", nameof(tokenType))
        };

    internal virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
            _isDisposed = true;
    }

    public void Dispose() => Dispose(true);

    public async Task LoginAsync(int appId, TokenType tokenType, string token)
    {
        await _stateLock.WaitAsync().ConfigureAwait(false);
        try
        {
            await LoginInternalAsync(appId, tokenType, token).ConfigureAwait(false);
        }
        finally
        {
            _stateLock.Release();
        }
    }

    private async Task LoginInternalAsync(int appId, TokenType tokenType, string token)
    {
        if (LoginState != LoginState.LoggedOut)
            await LogoutInternalAsync().ConfigureAwait(false);

        LoginState = LoginState.LoggingIn;

        try
        {
            _loginCancellationToken?.Dispose();
            _loginCancellationToken = new CancellationTokenSource();

            AuthToken = null;
            await RequestQueue.SetCancellationTokenAsync(_loginCancellationToken.Token).ConfigureAwait(false);
            RestClient.SetCancellationToken(_loginCancellationToken.Token);

            AppId = appId;
            AuthTokenType = tokenType;
            AuthToken = token.TrimEnd();
            RestClient.SetHeader("Authorization", GetPrefixedToken(AuthTokenType, appId, AuthToken));

            LoginState = LoginState.LoggedIn;
        }
        catch
        {
            await LogoutInternalAsync().ConfigureAwait(false);
            throw;
        }
    }

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

    private async Task LogoutInternalAsync()
    {
        //An exception here will lock the client into the unusable LoggingOut state, but that's probably fine since our client is in an undefined state too.
        if (LoginState == LoginState.LoggedOut) return;

        LoginState = LoginState.LoggingOut;

        try
        {
            _loginCancellationToken?.Cancel(false);
        }
        catch
        {
            // ignored
        }

        await DisconnectInternalAsync().ConfigureAwait(false);
        await RequestQueue.ClearAsync().ConfigureAwait(false);

        await RequestQueue.SetCancellationTokenAsync(CancellationToken.None).ConfigureAwait(false);
        RestClient.SetCancellationToken(CancellationToken.None);

        CurrentUserId = null;
        LoginState = LoginState.LoggedOut;
    }

    internal virtual Task ConnectInternalAsync() => Task.CompletedTask;
    internal virtual Task DisconnectInternalAsync(Exception? ex = null) => Task.CompletedTask;

    #endregion

    #region Core

    internal Task SendAsync(HttpMethod method, Expression<Func<string>> endpointExpr, BucketIds ids,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        SendAsync(method, GetEndpoint(endpointExpr),
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, options);

    public async Task SendAsync(HttpMethod method, string endpoint, BucketId? bucketId = null,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        RestRequest request = new(RestClient, method, endpoint, options);
        await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
    }

    internal Task SendJsonAsync(HttpMethod method, Expression<Func<string>> endpointExpr, object payload, BucketIds ids,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        SendJsonAsync(method, GetEndpoint(endpointExpr), payload,
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, options);

    public async Task SendJsonAsync(HttpMethod method, string endpoint, object payload, BucketId? bucketId = null,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        string? json = SerializeJson(payload);
        JsonRestRequest request = new(RestClient, method, endpoint, json, options);
        await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
    }

    internal Task SendMultipartAsync(HttpMethod method, Expression<Func<string>> endpointExpr,
        IReadOnlyDictionary<string, object> multipartArgs, BucketIds ids,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        SendMultipartAsync(method, GetEndpoint(endpointExpr), multipartArgs,
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, options);

    public async Task SendMultipartAsync(HttpMethod method, string endpoint,
        IReadOnlyDictionary<string, object> multipartArgs, BucketId? bucketId = null,
        ClientBucketType clientBucket = ClientBucketType.Unbucketed, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        MultipartRestRequest request = new(RestClient, method, endpoint, multipartArgs, options);
        await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
    }

    internal async Task<TResponse> SendAsync<TResponse>(HttpMethod method, Expression<Func<string>> endpointExpr,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        await SendAsync<TResponse>(method, GetEndpoint(endpointExpr),
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, bypassDeserialization, options);

    internal async Task<TResponse> SendAsync<TResponse, TArg1, TArg2>(HttpMethod method,
        Expression<Func<TArg1, TArg2, string>> endpointExpr, TArg1 arg1, TArg2 arg2,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        await SendAsync<TResponse>(method, GetEndpoint(endpointExpr, arg1, arg2),
            GetBucketId(method, ids, endpointExpr, arg1, arg2, funcName), clientBucket, bypassDeserialization, options);

    public async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string endpoint,
        BucketId? bucketId = null, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        RestRequest request = new(RestClient, method, endpoint, options);
        Stream response = await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
        return bypassDeserialization && response is TResponse responseObj
            ? responseObj
            : await DeserializeJsonAsync<TResponse>(response).ConfigureAwait(false);
    }

    internal async Task<TResponse> SendJsonAsync<TResponse>(HttpMethod method,
        Expression<Func<string>> endpointExpr, object payload,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        await SendJsonAsync<TResponse>(method, GetEndpoint(endpointExpr), payload,
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, bypassDeserialization, options);

    public async Task<TResponse> SendJsonAsync<TResponse>(HttpMethod method, string endpoint, object payload,
        BucketId? bucketId = null, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        string json = SerializeJson(payload);
        JsonRestRequest request = new(RestClient, method, endpoint, json, options);
        Stream response = await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
        return bypassDeserialization && response is TResponse responseObj
            ? responseObj
            : await DeserializeJsonAsync<TResponse>(response).ConfigureAwait(false);
    }

    internal Task<TResponse> SendMultipartAsync<TResponse>(HttpMethod method,
        Expression<Func<string>> endpointExpr, IReadOnlyDictionary<string, object> multipartArgs,
        BucketIds ids, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null,
        [CallerMemberName] string? funcName = null) =>
        SendMultipartAsync<TResponse>(method, GetEndpoint(endpointExpr), multipartArgs,
            GetBucketId(method, ids, endpointExpr, funcName), clientBucket, bypassDeserialization, options);

    public async Task<TResponse> SendMultipartAsync<TResponse>(HttpMethod method,
        string endpoint, IReadOnlyDictionary<string, object> multipartArgs,
        BucketId? bucketId = null, ClientBucketType clientBucket = ClientBucketType.Unbucketed,
        bool bypassDeserialization = false, RequestOptions? options = null)
    {
        options ??= new RequestOptions();
        options.BucketId = bucketId;

        MultipartRestRequest request = new(RestClient, method, endpoint, multipartArgs, options);
        Stream response = await SendInternalAsync(method, endpoint, request).ConfigureAwait(false);
        return bypassDeserialization && response is TResponse responseObj
            ? responseObj
            : await DeserializeJsonAsync<TResponse>(response).ConfigureAwait(false);
    }

    private async Task<Stream> SendInternalAsync(HttpMethod method, string endpoint, RestRequest request)
    {
        if (!request.Options.IgnoreState)
            CheckState();

        request.Options.RetryMode ??= DefaultRetryMode;
        request.Options.RatelimitCallback ??= DefaultRatelimitCallback;

        Stopwatch stopwatch = Stopwatch.StartNew();
        Stream responseStream = await RequestQueue.SendAsync(request).ConfigureAwait(false);
        stopwatch.Stop();

        double milliseconds = ToMilliseconds(stopwatch);
        await _sentRequestEvent.InvokeAsync(method, endpoint, milliseconds).ConfigureAwait(false);

        return responseStream;
    }

    #endregion

    #region Gateway

    public async Task<GetGatewayResponse> GetGatewayAsync(RequestOptions? options = null)
    {
        options = RequestOptions.CreateOrClone(options);
        return await SendAsync<GetGatewayResponse>(HttpMethod.Get,
                () => "gateway", new BucketIds(), options: options)
            .ConfigureAwait(false);
    }

    public async Task<GetBotGatewayResponse> GetBotGatewayAsync(RequestOptions? options = null)
    {
        options = RequestOptions.CreateOrClone(options);
        return await SendAsync<GetBotGatewayResponse>(HttpMethod.Get,
                () => "gateway", new BucketIds(), options: options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Guilds

    public async Task<IReadOnlyCollection<Guild>> GetGuildsAsync(GetGuildsParams args, RequestOptions? options = null)
    {
        Preconditions.AtMost(args.Limit, QQBotConfig.MaxGuildsPerBatch, nameof(args.Limit));
        options = RequestOptions.CreateOrClone(options);

        int limit = args.Limit.GetValueOrDefault(QQBotConfig.MaxGuildsPerBatch);
        string query = $"?limit={limit}";
        if (args.BeforeId.HasValue)
            query += $"&before={args.BeforeId}";
        if (args.AfterId.HasValue)
            query += $"&after={args.AfterId}";

        BucketIds ids = new();
        return await SendJsonAsync<IReadOnlyCollection<Guild>>(HttpMethod.Get,
                () => $"users/@me/guilds{query}", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<Guild> GetGuildAsync(ulong guildId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendAsync<Guild>(HttpMethod.Get,
                () => $"guilds/{guildId}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Channels

    public async Task<IReadOnlyCollection<Channel>> GetChannelsAsync(ulong guildId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendAsync<IReadOnlyCollection<Channel>>(HttpMethod.Get,
                () => $"guilds/{guildId}/channels", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<Channel> GetChannelAsync(ulong channelId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<Channel>(HttpMethod.Get,
                () => $"channels/{channelId}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<Channel> CreateChannelAsync(ulong guildId, CreateChannelParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotNullOrEmpty(args.Name, nameof(args.Name));
        if (args.Type is ChannelType.Category)
        {
            Preconditions.NotNull(args.Position, nameof(args.Position));
            Preconditions.AtLeast(args.Position.Value, 2, nameof(args.Position));
        }

        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendJsonAsync<Channel>(HttpMethod.Post,
                () => $"guilds/{guildId}/channels", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<Channel> ModifyChannelAsync(ulong channelId, ModifyChannelParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(channelId);
        return await SendJsonAsync<Channel>(HttpMethod.Patch,
                () => $"channels/{channelId}", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteChannelAsync(ulong channelId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendAsync(HttpMethod.Delete,
                () => $"channels/{channelId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Users

    public async Task<SelfUser> GetSelfUserAsync(RequestOptions? options = null)
    {
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        return await SendAsync<SelfUser>(HttpMethod.Get,
                () => "users/@me", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Messages

    public async Task<ChannelMessage> GetMessageAsync(ulong channelId, string messageId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<ChannelMessage>(HttpMethod.Get,
                () => $"channels/{channelId}/messages/{messageId}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<SendUserGroupMessageResponse> SendUserMessageAsync(Guid openId, SendUserGroupMessageParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(openId, Guid.Empty, nameof(openId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        string id = openId.ToIdString();
        return await SendJsonAsync<SendUserGroupMessageResponse>(HttpMethod.Post,
                () => $"v2/users/{id}/messages", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<SendUserGroupMessageResponse> SendGroupMessageAsync(Guid groupOpenid, SendUserGroupMessageParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(groupOpenid, Guid.Empty, nameof(groupOpenid));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        string id = groupOpenid.ToIdString();
        return await SendJsonAsync<SendUserGroupMessageResponse>(HttpMethod.Post,
                () => $"v2/groups/{id}/messages", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<ChannelMessage> SendChannelMessageAsync(ulong channelId, SendChannelMessageParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        if (args.FileImage.HasValue)
        {
            return await SendMultipartAsync<ChannelMessage>(HttpMethod.Post,
                    () => $"channels/{channelId}/messages", args.ToDictionary(_serializerOptions),
                    ids, ClientBucketType.SendEdit, false, options)
                .ConfigureAwait(false);
        }
        return await SendJsonAsync<ChannelMessage>(HttpMethod.Post,
                () => $"channels/{channelId}/messages", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<ChannelMessage> SendDirectMessageAsync(ulong directGuildId, SendChannelMessageParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(directGuildId, 0, nameof(directGuildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, directGuildId);
        if (args.FileImage.HasValue)
        {
            return await SendMultipartAsync<ChannelMessage>(HttpMethod.Post,
                    () => $"dms/{directGuildId}/messages", args.ToDictionary(_serializerOptions),
                    ids, ClientBucketType.SendEdit, false, options)
                .ConfigureAwait(false);
        }
        return await SendJsonAsync<ChannelMessage>(HttpMethod.Post,
                () => $"dms/{directGuildId}/messages", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteUserMessageAsync(Guid openId, string messageId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(openId, Guid.Empty, nameof(openId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        string id = openId.ToIdString();
        await SendAsync(HttpMethod.Delete,
                () => $"v2/users/{id}/messages/{messageId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteGroupMessageAsync(Guid groupOpenId, string messageId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(groupOpenId, Guid.Empty, nameof(groupOpenId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        string id = groupOpenId.ToIdString();
        await SendAsync(HttpMethod.Delete,
                () => $"v2/groups/{id}/messages/{messageId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteChannelMessageAsync(ulong channelId, string messageId, DeleteChannelMessageParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        options = RequestOptions.CreateOrClone(options);

        string query = args.HideTip.HasValue
            ? $"?hidetip={args.HideTip.Value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()}"
            : string.Empty;
        BucketIds ids = new(0, channelId);
        string id = channelId.ToIdString();
        await SendAsync(HttpMethod.Delete,
                () => $"channels/{id}/messages/{messageId}{query}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteDirectMessageAsync(ulong directGuildId, string messageId, DeleteDirectMessageParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(directGuildId, 0, nameof(directGuildId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        options = RequestOptions.CreateOrClone(options);

        string query = args.HideTip.HasValue
            ? $"?hidetip={args.HideTip.Value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()}"
            : string.Empty;
        BucketIds ids = new(0, directGuildId);
        await SendAsync(HttpMethod.Delete,
                () => $"dms/{directGuildId}/messages/{messageId}{query}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Files

    public async Task<SendAttachmentResponse> CreateUserAttachmentAsync(Guid openId, SendAttachmentParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(openId, Guid.Empty, nameof(openId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        string id = openId.ToString("N").ToUpperInvariant();
        return await SendJsonAsync<SendAttachmentResponse>(HttpMethod.Post,
                () => $"v2/users/{id}/files", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<SendAttachmentResponse> CreateGroupAttachmentAsync(Guid groupOpenId, SendAttachmentParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(groupOpenId, Guid.Empty, nameof(groupOpenId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        string id = groupOpenId.ToString("N").ToUpperInvariant();
        return await SendJsonAsync<SendAttachmentResponse>(HttpMethod.Post,
                () => $"v2/groups/{id}/files", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Reactions

    public async Task AddChannelMessageReactionAsync(ulong channelId, string messageId, EmojiType emojiType, string emojiId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        Preconditions.NotNullOrWhiteSpace(emojiId, nameof(emojiId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        string emojiTypeString = ((int)emojiType).ToString();
        await SendAsync(HttpMethod.Put,
                () => $"channels/{channelId}/messages/{messageId}/reactions/{emojiTypeString}/{emojiId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task RemoveChannelMessageReactionAsync(ulong channelId, string messageId, EmojiType emojiType, string emojiId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        Preconditions.NotNullOrWhiteSpace(emojiId, nameof(emojiId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        string emojiTypeString = ((int)emojiType).ToString();
        await SendAsync(HttpMethod.Delete,
                () => $"channels/{channelId}/messages/{messageId}/reactions/{emojiTypeString}/{emojiId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task<GetChannelMessageReactionUsersResponse> GetChannelMessageReactionUsersAsync(ulong channelId, string messageId, EmojiType emojiType, string emojiId, GetChannelMessageReactionUsersParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        Preconditions.NotNullOrWhiteSpace(emojiId, nameof(emojiId));
        Preconditions.AtMost(args.Limit, QQBotConfig.MaxReactionUsersPerBatch, nameof(args.Limit));
        options = RequestOptions.CreateOrClone(options);

        int limit = args.Limit.GetValueOrDefault(QQBotConfig.MaxReactionUsersPerBatch);
        string query = $"?limit={limit}";
        if (!string.IsNullOrWhiteSpace(args.Cookie))
            query += $"&cookie={args.Cookie}";

        BucketIds ids = new(0, channelId);
        string emojiTypeString = ((int)emojiType).ToString();
        return await SendAsync<GetChannelMessageReactionUsersResponse>(HttpMethod.Get,
                () => $"channels/{channelId}/messages/{messageId}/reactions/{emojiTypeString}/{emojiId}{query}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Interactions

    public async Task RespondInteractionAsync(ulong interactionId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(interactionId, 0, nameof(interactionId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new();
        await SendAsync(HttpMethod.Put,
                () => $"interactions/{interactionId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Guild Members

    public async Task<CountMediaChannelOnlineMembersResponse> CountMediaChannelOnlineMembersAsync(ulong channelId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(channelId: channelId);
        return await SendAsync<CountMediaChannelOnlineMembersResponse>(HttpMethod.Get,
                () => $"channels/{channelId}/online_nums", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<Member>> GetGuildMembersAsync(ulong guildId, GetGuildMembersParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.AtMost(args.Limit, QQBotConfig.MaxMembersPerBatch, nameof(args.Limit));
        options = RequestOptions.CreateOrClone(options);

        int limit = args.Limit.GetValueOrDefault(QQBotConfig.MaxMembersPerBatch);
        string query = $"?limit={limit}";
        if (args.AfterId.HasValue)
            query += $"&after={args.AfterId}";

        BucketIds ids = new(guildId);
        return await SendJsonAsync<IReadOnlyCollection<Member>>(HttpMethod.Get,
                () => $"guilds/{guildId}/members{query}", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<GetGuildRoleMembersResponse> GetGuildRoleMembersAsync(ulong guildId, uint roleId, GetGuildRoleMembersParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(roleId, 0, nameof(roleId));
        options = RequestOptions.CreateOrClone(options);

        int limit = args.Limit.GetValueOrDefault(QQBotConfig.MaxMembersPerBatch);
        string query = $"?limit={limit}";
        if (!string.IsNullOrWhiteSpace(args.StartIndex))
            query += $"&start_index={args.StartIndex}";

        BucketIds ids = new(guildId);
        return await SendAsync<GetGuildRoleMembersResponse>(HttpMethod.Get,
                () => $"guilds/{guildId}/roles/{roleId}/members{query}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<Member> GetGuildMemberAsync(ulong guildId, ulong userId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(userId, 0, nameof(userId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendAsync<Member>(HttpMethod.Get,
                () => $"guilds/{guildId}/members/{userId}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteGuildMemberAsync(ulong guildId, ulong userId, DeleteGuildMemberParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(userId, 0, nameof(userId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        await SendJsonAsync(HttpMethod.Delete,
                () => $"guilds/{guildId}/members/{userId}", args, ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Roles

    public async Task<GetGuildRolesResponse> GetGuildRolesAsync(ulong guildId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendAsync<GetGuildRolesResponse>(HttpMethod.Get,
                () => $"guilds/{guildId}/roles", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<CreateGuildRoleResponse> CreateGuildRoleAsync(ulong guildId, CreateGuildRoleParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendJsonAsync<CreateGuildRoleResponse>(HttpMethod.Post,
                () => $"guilds/{guildId}/roles", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<ModifyGuildRoleResponse> ModifyGuildRoleAsync(ulong guildId, uint roleId, ModifyGuildRoleParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(roleId, 0, nameof(roleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendJsonAsync<ModifyGuildRoleResponse>(HttpMethod.Patch,
                () => $"guilds/{guildId}/roles/{roleId}", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteGuildRoleAsync(ulong guildId, uint roleId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(roleId, 0, nameof(roleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        await SendAsync(HttpMethod.Delete,
                () => $"guilds/{guildId}/roles/{roleId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Member Roles

    public async Task GrantGuildRoleAsync(ulong guildId, ulong userId, uint roleId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(userId, 0, nameof(userId));
        Preconditions.NotEqual(roleId, 0, nameof(roleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        await SendAsync(HttpMethod.Put,
                () => $"guilds/{guildId}/members/{userId}/roles/{roleId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task RevokeGuildRoleAsync(ulong guildId, ulong userId, uint roleId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(userId, 0, nameof(userId));
        Preconditions.NotEqual(roleId, 0, nameof(roleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        await SendAsync(HttpMethod.Delete,
                () => $"guilds/{guildId}/members/{userId}/roles/{roleId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Permissions Overwrites

    public async Task<ChannelPermissions> GetMemberPermissionsAsync(ulong channelId, ulong userId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotEqual(userId, 0, nameof(userId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<ChannelPermissions>(HttpMethod.Get,
                () => $"channels/{channelId}/members/{userId}/permissions", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task ModifyMemberPermissionsAsync(ulong channelId, ulong userId, ModifyMemberPermissionsParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotEqual(userId, 0, nameof(userId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendJsonAsync(HttpMethod.Put,
                () => $"channels/{channelId}/members/{userId}/permissions", args, ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task<ChannelPermissions> GetRolePermissionsAsync(ulong channelId, uint roleId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotEqual(roleId, 0, nameof(roleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<ChannelPermissions>(HttpMethod.Get,
                () => $"channels/{channelId}/roles/{roleId}/permissions", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task ModifyRolePermissionsAsync(ulong channelId, uint roleId, ModifyRolePermissionsParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotEqual(roleId, 0, nameof(roleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendJsonAsync(HttpMethod.Put,
                () => $"channels/{channelId}/roles/{roleId}/permissions", args, ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Application Guild Permissions

    public async Task<IReadOnlyCollection<ApiPermission>> GetApplicationGuildPermissionsAsync(ulong guildId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        GetApplicationGuildPermissionResponse response = await SendAsync<GetApplicationGuildPermissionResponse>(HttpMethod.Get,
                () => $"guilds/{guildId}/api_permission", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
        return response.ApiPermissions;
    }

    public async Task RequestApplicationGuildPermissionAsync(ulong guildId, RequestApplicationGuildPermissionParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(args.ChannelId, 0, nameof(args.ChannelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId, args.ChannelId);
        await SendJsonAsync(HttpMethod.Post,
                () => $"guilds/{guildId}/api_permission/demand", args, ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Message Settings

    public async Task<MessageSetting> GetMessageSettingAsync(ulong guildId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendAsync<MessageSetting>(HttpMethod.Get,
                () => $"guilds/{guildId}/message/setting", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task MuteAllAsync(ulong guildId, MuteAllParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        await SendJsonAsync(HttpMethod.Patch,
                () => $"guilds/{guildId}/mute", args, ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task MuteMemberAsync(ulong guildId, ulong userId, MuteMemberParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotEqual(userId, 0, nameof(userId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        await SendJsonAsync(HttpMethod.Patch,
                () => $"guilds/{guildId}/members/{userId}/mute", args, ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task MuteMembersAsync(ulong guildId, MuteMembersParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        await SendJsonAsync(HttpMethod.Patch,
                () => $"guilds/{guildId}/mute", args, ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Announcements

    public async Task<Announces> CreateAnnouncementAsync(ulong guildId, CreateAnnouncementParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        return await SendJsonAsync<Announces>(HttpMethod.Post,
                () => $"guilds/{guildId}/announces", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteAnnouncementAsync(ulong guildId, string messageId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(guildId, 0, nameof(guildId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(guildId);
        await SendAsync(HttpMethod.Delete,
                () => $"guilds/{guildId}/announces/{messageId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Messages

    public async Task<PinsMessage> PinMessageAsync(ulong channelId, string messageId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<PinsMessage>(HttpMethod.Put,
                () => $"channels/{channelId}/pins/{messageId}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task UnpinMessageAsync(ulong channelId, string messageId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrWhiteSpace(messageId, nameof(messageId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendAsync(HttpMethod.Delete,
                () => $"channels/{channelId}/pins/{messageId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task<PinsMessage> GetPinedMessagesAsync(ulong channelId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<PinsMessage>(HttpMethod.Get,
                () => $"channels/{channelId}/pins", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Schedules

    public async Task<IReadOnlyCollection<Schedule>> GetSchedulesAsync(ulong channelId, DateTimeOffset? since = null, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        string query = since.HasValue
            ? $"?since={since.Value.ToUnixTimeMilliseconds()}"
            : string.Empty;
        JsonElement response = await SendAsync<JsonElement>(HttpMethod.Get,
                () => $"channels/{channelId}/schedules{query}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
        if (response.ValueKind is JsonValueKind.Array)
            return DeserializeJsonAsync<IReadOnlyCollection<Schedule>>(response);
        return [];
    }

    public async Task<Schedule> GetScheduleAsync(ulong channelId, ulong scheduleId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotEqual(scheduleId, 0, nameof(scheduleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<Schedule>(HttpMethod.Get,
                () => $"channels/{channelId}/schedules/{scheduleId}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<Schedule> CreateScheduleAsync(ulong channelId, CreateScheduleParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNull(args, nameof(args));
        Preconditions.NotNull(args.Schedule, nameof(args.Schedule));
        Preconditions.NotNullOrEmpty(args.Schedule.Name, nameof(args.Schedule.Name));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendJsonAsync<Schedule>(HttpMethod.Post,
                () => $"channels/{channelId}/schedules", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<Schedule> ModifyScheduleAsync(ulong channelId, ulong scheduleId, ModifyScheduleParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotEqual(scheduleId, 0, nameof(scheduleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendJsonAsync<Schedule>(HttpMethod.Patch,
                () => $"channels/{channelId}/schedules/{scheduleId}", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteScheduleAsync(ulong channelId, ulong scheduleId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotEqual(scheduleId, 0, nameof(scheduleId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendAsync(HttpMethod.Delete,
                () => $"channels/{channelId}/schedules/{scheduleId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Audio

    public async Task ControlAudioAsync(ulong channelId, AudioControl args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendJsonAsync(HttpMethod.Post,
                () => $"channels/{channelId}/audio", args, ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task JoinMicrophoneAsync(ulong channelId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendAsync(HttpMethod.Put,
                () => $"channels/{channelId}/mic", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    public async Task LeaveMicrophoneAsync(ulong channelId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendAsync(HttpMethod.Delete,
                () => $"channels/{channelId}/mic", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Forum Threads

    public async Task<GetForumThreadsResponse> GetForumThreadsAsync(ulong channelId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<GetForumThreadsResponse>(HttpMethod.Get,
                () => $"channels/{channelId}/threads", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<GetForumThreadResponse> GetForumThreadAsync(ulong channelId, string threadId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrEmpty(threadId, nameof(threadId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendAsync<GetForumThreadResponse>(HttpMethod.Get,
                () => $"channels/{channelId}/threads/{threadId}", ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task<CreateForumThreadResponse> CreateForumThreadAsync(ulong channelId, CreateForumThreadParams args, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        return await SendJsonAsync<CreateForumThreadResponse>(HttpMethod.Put,
                () => $"channels/{channelId}/threads", args, ids, ClientBucketType.SendEdit, false, options)
            .ConfigureAwait(false);
    }

    public async Task DeleteForumThreadAsync(ulong channelId, string threadId, RequestOptions? options = null)
    {
        Preconditions.NotEqual(channelId, 0, nameof(channelId));
        Preconditions.NotNullOrEmpty(threadId, nameof(threadId));
        options = RequestOptions.CreateOrClone(options);

        BucketIds ids = new(0, channelId);
        await SendAsync(HttpMethod.Delete,
                () => $"channels/{channelId}/threads/{threadId}", ids, ClientBucketType.SendEdit, options)
            .ConfigureAwait(false);
    }

    #endregion

    #region Helpers

    protected void CheckState()
    {
        if (LoginState != LoginState.LoggedIn)
            throw new InvalidOperationException("Client is not logged in.");
    }

    protected static double ToMilliseconds(Stopwatch stopwatch) =>
        Math.Round((double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000.0, 2);

    [return: NotNullIfNotNull(nameof(payload))]
    protected string? SerializeJson(object? payload) =>
        payload is null ? null : JsonSerializer.Serialize(payload, _serializerOptions);

    protected async Task<T> DeserializeJsonAsync<T>(Stream jsonStream)
    {
        try
        {
            T? jsonObject = await JsonSerializer.DeserializeAsync<T>(jsonStream, _serializerOptions).ConfigureAwait(false);
            if (jsonObject is null)
                throw new JsonException($"Failed to deserialize JSON to type {typeof(T).FullName}");
            return jsonObject;
        }
        catch (JsonException ex)
        {
            if (jsonStream is MemoryStream memoryStream)
            {
                string json = Encoding.UTF8.GetString(memoryStream.ToArray());
                throw new JsonException($"Failed to deserialize JSON to type {typeof(T).FullName}\nJSON: {json}", ex);
            }

            throw;
        }
    }

    protected T DeserializeJsonAsync<T>(JsonElement jsonElement)
    {
        try
        {
            T? jsonObject = jsonElement.Deserialize<T>(_serializerOptions);
            if (jsonObject is null)
                throw new JsonException($"Failed to deserialize JSON to type {typeof(T).FullName}");
            return jsonObject;
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Failed to deserialize JSON to type {typeof(T).FullName}\nJSON: {jsonElement.GetRawText()}", ex);
        }
    }

    internal class BucketIds
    {
        public ulong GuildId { get; internal set; }
        public ulong ChannelId { get; internal set; }
        public HttpMethod? HttpMethod { get; internal set; }

        internal BucketIds(ulong guildId = 0, ulong channelId = 0)
        {
            GuildId = guildId;
            ChannelId = channelId;
        }

        internal object?[] ToArray() =>
            [HttpMethod, GuildId, ChannelId];

        internal Dictionary<string, string> ToMajorParametersDictionary()
        {
            Dictionary<string, string> dict = new();
            if (GuildId != 0)
                dict["GuildId"] = GuildId.ToString();
            if (ChannelId != 0)
                dict["ChannelId"] = ChannelId.ToString();
            return dict;
        }

        internal static int? GetIndex(string name) =>
            name switch
            {
                "httpMethod" => 0,
                "guildId" => 1,
                "channelId" => 2,
                _ => null
            };
    }

    private static string GetEndpoint(Expression<Func<string>> endpointExpr) => endpointExpr.Compile()();

    private static string GetEndpoint<T1, T2>(Expression<Func<T1, T2, string>> endpointExpr, T1 arg1, T2 arg2) => endpointExpr.Compile()(arg1, arg2);

    private static BucketId GetBucketId(HttpMethod httpMethod, BucketIds ids, Expression<Func<string>> endpointExpr, string? callingMethod)
    {
        Preconditions.NotNull(callingMethod, nameof(callingMethod));
        ids.HttpMethod = httpMethod;
        return _bucketIdGenerators.GetOrAdd(callingMethod, _ => CreateBucketId(endpointExpr))(ids);
    }

    private static BucketId GetBucketId<TArg1, TArg2>(HttpMethod httpMethod, BucketIds ids, Expression<Func<TArg1, TArg2, string>> endpointExpr,
        TArg1 arg1, TArg2 arg2, string? callingMethod)
    {
        Preconditions.NotNull(callingMethod, nameof(callingMethod));
        ids.HttpMethod = httpMethod;
        return _bucketIdGenerators.GetOrAdd(callingMethod, x => CreateBucketId(endpointExpr, arg1, arg2))(ids);
    }

    private static Func<BucketIds, BucketId> CreateBucketId<TArg1, TArg2>(Expression<Func<TArg1, TArg2, string>> endpoint, TArg1 arg1, TArg2 arg2) =>
        CreateBucketId(() => endpoint.Compile().Invoke(arg1, arg2));

    private static Func<BucketIds, BucketId> CreateBucketId(Expression<Func<string>> endpoint)
    {
        try
        {
            //Is this a constant string?
            if (endpoint.Body.NodeType == ExpressionType.Constant)
                return x => BucketId.Create(x.HttpMethod, (endpoint.Body as ConstantExpression)?.Value?.ToString(), x.ToMajorParametersDictionary());

            StringBuilder builder = new();

            MethodCallExpression methodCall = (MethodCallExpression) endpoint.Body;
            Expression[] methodArgs = methodCall.Arguments.ToArray();
            string format = methodArgs[0].NodeType == ExpressionType.Constant
                ? ((ConstantExpression) methodArgs[0]).Value!.ToString()!
                : endpoint.Compile()();

            //Unpack the array, if one exists (happens with 4+ parameters)
            if (methodArgs.Length > 1 && methodArgs[1].NodeType == ExpressionType.NewArrayInit)
            {
                NewArrayExpression arrayExpr = (NewArrayExpression) methodArgs[1];
                Expression[] elements = arrayExpr.Expressions.ToArray();
                Array.Resize(ref methodArgs, elements.Length + 1);
                Array.Copy(elements, 0, methodArgs, 1, elements.Length);
            }

            int endIndex = format.IndexOf('?'); //Don't include params
            if (endIndex == -1)
                endIndex = format.Length;

            int lastIndex = 0;
            while (true)
            {
                int leftIndex = format.IndexOf("{", lastIndex, StringComparison.Ordinal);
                if (leftIndex == -1 || leftIndex > endIndex)
                {
                    builder.Append(format, lastIndex, endIndex - lastIndex);
                    break;
                }

                builder.Append(format, lastIndex, leftIndex - lastIndex);
                int rightIndex = format.IndexOf("}", leftIndex, StringComparison.Ordinal);

                int argId = int.Parse(format.Substring(leftIndex + 1, rightIndex - leftIndex - 1), NumberStyles.None, CultureInfo.InvariantCulture);
                string fieldName = GetFieldName(methodArgs[argId + 1]);

                int? mappedId = BucketIds.GetIndex(fieldName);

                if (!mappedId.HasValue
                    && rightIndex != endIndex
                    && format.Length > rightIndex + 1
                    && format[rightIndex + 1] == '/') //Ignore the next slash
                    rightIndex++;

                if (mappedId.HasValue)
                    builder.Append($"{{{mappedId.Value}}}");

                lastIndex = rightIndex + 1;
            }

            if (builder[^1] == '/')
                builder.Remove(builder.Length - 1, 1);

            format = builder.ToString();

            return x => BucketId.Create(x.HttpMethod, string.Format(format, x.ToArray()), x.ToMajorParametersDictionary());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate the bucket id for this operation.", ex);
        }
    }

    private static string GetFieldName(Expression expr)
    {
        if (expr.NodeType == ExpressionType.Convert)
            expr = ((UnaryExpression) expr).Operand;

        if (expr.NodeType != ExpressionType.MemberAccess)
            throw new InvalidOperationException("Unsupported expression");

        return ((MemberExpression) expr).Member.Name;
    }

    #endregion
}
