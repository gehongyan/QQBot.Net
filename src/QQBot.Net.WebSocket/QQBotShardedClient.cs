using QQBot.API;
using QQBot.Rest;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QQBot.WebSocket;

/// <summary>
///     Represents a sharded QQBot client.
/// </summary>
public partial class QQBotShardedClient : BaseSocketClient, IQQBotClient
{
    #region QQBotShardedClient

    private readonly QQBotSocketConfig _baseConfig;
    private readonly Dictionary<int, int> _shardIdsToIndex;
    private int[]? _shardIds;
    private QQBotSocketClient[]? _shards;
    private int _totalShards;
    private SemaphoreSlim[]? _identifySemaphores;
    private readonly object _semaphoreResetLock;
    private Task? _semaphoreResetTask;

    private bool _isDisposed;

    [MemberNotNullWhen(false, nameof(_totalShards), nameof(_shardIds), nameof(_shards), nameof(_identifySemaphores))]
    private bool AutomaticShards { get; }

    /// <inheritdoc />
    public override int Latency { get => GetLatency(); protected set { } }

    internal new QQBotSocketApiClient ApiClient
    {
        get
        {
            base.ApiClient.CurrentUserId ??= CurrentUser?.Id;
            return base.ApiClient;
        }
    }

    // /// <inheritdoc />
    // public override IReadOnlyCollection<SocketGuild> Guilds => GetGuilds().ToReadOnlyCollection(GetGuildCount);

    /// <summary>
    ///     Gets the shards that this client is connected to.
    /// </summary>
    public IReadOnlyCollection<QQBotSocketClient> Shards => _shards ?? [];

    /// <summary>
    ///     Provides access to a REST-only client with a shared state from this client.
    /// </summary>
    public override QQBotSocketRestClient Rest =>
        _shards?.FirstOrDefault()?.Rest ?? throw new InvalidOperationException("No shards are available.");

    /// <inheritdoc />
    public override SocketSelfUser? CurrentUser
    {
        get => _shards?.FirstOrDefault(x => x.CurrentUser != null)?.CurrentUser;
        protected set => throw new InvalidOperationException();
    }

    /// <summary> Creates a new REST/WebSocket QQBot client. </summary>
    public QQBotShardedClient() : this(null, new QQBotSocketConfig()) { }

    /// <summary> Creates a new REST/WebSocket QQBot client. </summary>
    public QQBotShardedClient(QQBotSocketConfig config)
        : this(null, config, CreateApiClient(config))
    {
    }

    /// <summary> Creates a new REST/WebSocket QQBot client. </summary>
    public QQBotShardedClient(int[] ids) : this(ids, new QQBotSocketConfig()) { }

    /// <summary> Creates a new REST/WebSocket QQBot client. </summary>
    public QQBotShardedClient(int[]? ids, QQBotSocketConfig config)
        : this(ids, config, CreateApiClient(config))
    {
    }

    private QQBotShardedClient(int[]? ids, QQBotSocketConfig config, QQBotSocketApiClient client)
        : base(config, client)
    {
        if (config.ShardId.HasValue)
            throw new ArgumentException($"{nameof(config.ShardId)} must not be set.");
        if (ids != null && !config.TotalShards.HasValue)
            throw new ArgumentException($"Custom ids are not supported when {nameof(config.TotalShards)} is not specified.");

        _semaphoreResetLock = new object();
        _shardIdsToIndex = new Dictionary<int, int>();
        config.DisplayInitialLog = false;
        _baseConfig = config;

        if (config.TotalShards == null)
            AutomaticShards = true;
        else
        {
            _totalShards = config.TotalShards.Value;
            _shardIds = ids ?? Enumerable.Range(0, _totalShards).ToArray();
            _shards = new QQBotSocketClient[_shardIds.Length];
            _identifySemaphores = new SemaphoreSlim[config.IdentifyMaxConcurrency];
            for (int i = 0; i < config.IdentifyMaxConcurrency; i++)
                _identifySemaphores[i] = new SemaphoreSlim(1, 1);
            for (int i = 0; i < _shardIds.Length; i++)
            {
                _shardIdsToIndex.Add(_shardIds[i], i);
                QQBotSocketConfig newConfig = config.Clone();
                newConfig.ShardId = _shardIds[i];
                _shards[i] = new QQBotSocketClient(newConfig, this, i != 0 ? _shards[0] : null);
                RegisterEvents(_shards[i], i == 0);
            }
        }
    }

    private static QQBotSocketApiClient CreateApiClient(QQBotSocketConfig config) =>
        new(config.RestClientProvider, config.WebSocketProvider,
            config.AccessEnvironment, QQBotConfig.UserAgent,
            config.GatewayHost, defaultRatelimitCallback: config.DefaultRatelimitCallback);

    internal Task AcquireIdentifyLockAsync(int shardId, CancellationToken token)
    {
        if (_identifySemaphores == null)
            throw new InvalidOperationException("Shards are not initialized.");
        int semaphoreIdx = shardId % _baseConfig.IdentifyMaxConcurrency;
        return _identifySemaphores[semaphoreIdx].WaitAsync(token);
    }

    internal void ReleaseIdentifyLock()
    {
        lock (_semaphoreResetLock)
        {
            _semaphoreResetTask ??= ResetSemaphoresAsync();
        }
    }

    private async Task ResetSemaphoresAsync()
    {
        await Task.Delay(5000).ConfigureAwait(false);
        lock (_semaphoreResetLock)
        {
            if (_identifySemaphores == null)
                throw new InvalidOperationException("Shards are not initialized.");
            foreach (SemaphoreSlim semaphore in _identifySemaphores)
            {
                if (semaphore.CurrentCount == 0)
                    semaphore.Release();
            }
            _semaphoreResetTask = null;
        }
    }

    internal override async Task OnLoginAsync(int appId, TokenType tokenType, string token)
    {
        BotGateway botGateway = await GetBotGatewayAsync().ConfigureAwait(false);
        if (AutomaticShards)
        {
            _shardIds = Enumerable.Range(0, botGateway.Shards).ToArray();
            _totalShards = _shardIds.Length;
            _shards = new QQBotSocketClient[_shardIds.Length];
            int maxConcurrency = botGateway.SessionStartLimit.MaxConcurrency;
            _baseConfig.IdentifyMaxConcurrency = maxConcurrency;
            _identifySemaphores = new SemaphoreSlim[maxConcurrency];
            for (int i = 0; i < maxConcurrency; i++)
                _identifySemaphores[i] = new SemaphoreSlim(1, 1);
            for (int i = 0; i < _shardIds.Length; i++)
            {
                _shardIdsToIndex.Add(_shardIds[i], i);
                QQBotSocketConfig newConfig = _baseConfig.Clone();
                newConfig.ShardId = _shardIds[i];
                newConfig.TotalShards = _totalShards;
                _shards[i] = new QQBotSocketClient(newConfig, this, i != 0 ? _shards[0] : null);
                RegisterEvents(_shards[i], i == 0);
            }
        }

        //Assume thread safe: already in a connection lock
        foreach (QQBotSocketClient client in _shards)
        {
            // Set the gateway URL to the one returned by QQBot, if a custom one isn't set.
            client.ApiClient.GatewayUrl = botGateway.Url;
            await client.LoginAsync(appId, tokenType, token);
        }
    }

    internal override async Task OnLogoutAsync()
    {
        //Assume thread safe: already in a connection lock
        if (_shards != null)
        {
            foreach (QQBotSocketClient client in _shards)
            {
                // Reset the gateway URL set for the shard.
                client.ApiClient.GatewayUrl = null;
                await client.LogoutAsync();
            }
        }

        if (AutomaticShards)
        {
            _shardIds = [];
            _shardIdsToIndex.Clear();
            _totalShards = 0;
            _shards = null;
        }
    }

    /// <inheritdoc />
    public override Task StartAsync() => _shards is not null
        ? Task.WhenAll(_shards.Select(x => x.StartAsync()))
        : Task.CompletedTask;

    /// <inheritdoc />
    public override Task StopAsync() => _shards is not null
        ? Task.WhenAll(_shards.Select(x => x.StopAsync()))
        : Task.CompletedTask;

    /// <summary>
    ///     Gets the shard for the provided ID.
    /// </summary>
    /// <param name="id"> The ID of the shard to get. </param>
    /// <returns> The shard with the provided ID, or <see langword="null"/> if none is found. </returns>
    public QQBotSocketClient? GetShard(int id) =>
        _shardIdsToIndex.TryGetValue(id, out int index) && index < _shards?.Length ? _shards[index] : null;

    private int GetShardIdFor(ulong guildId)
        => (int)((guildId >> 22) % (uint)_totalShards);

    /// <summary>
    ///     Gets the shard ID for the provided guild.
    /// </summary>
    /// <param name="guild"> The guild to get the shard ID for. </param>
    /// <returns> The shard ID for the provided guild. </returns>
    public int GetShardIdFor(IGuild guild)
        => GetShardIdFor(guild?.Id ?? 0);

    private QQBotSocketClient? GetShardFor(ulong guildId)
        => GetShard(GetShardIdFor(guildId));

    /// <summary>
    ///     Gets the shard for the provided guild.
    /// </summary>
    /// <param name="guild"> The guild to get the shard ID for. </param>
    /// <returns> The shard for the provided guild. </returns>
    public QQBotSocketClient? GetShardFor(IGuild guild)
        => GetShardFor(guild?.Id ?? 0);

    /// <inheritdoc />
    public override SocketGuild? GetGuild(ulong id)
        => GetShardFor(id)?.GetGuild(id);

    // /// <inheritdoc />
    // public override SocketChannel GetChannel(ulong id)
    // {
    //     for (int i = 0; i < _shards.Length; i++)
    //     {
    //         var channel = _shards[i].GetChannel(id);
    //         if (channel != null)
    //             return channel;
    //     }
    //
    //     return null;
    // }

    // private IEnumerable<SocketGuild> GetGuilds()
    // {
    //     for (int i = 0; i < _shards.Length; i++)
    //     {
    //         foreach (var guild in _shards[i].Guilds)
    //             yield return guild;
    //     }
    // }
    //
    // private int GetGuildCount()
    // {
    //     return _shards?.Sum(x => x.Guilds.Count) ?? 0;
    // }

    // /// <inheritdoc />
    // public override SocketUser GetUser(ulong id)
    // {
    //     for (int i = 0; i < _shards.Length; i++)
    //     {
    //         var user = _shards[i].GetUser(id);
    //         if (user != null)
    //             return user;
    //     }
    //
    //     return null;
    // }
    //
    // /// <inheritdoc />
    // public override SocketUser GetUser(string username, string discriminator = null)
    // {
    //     for (int i = 0; i < _shards.Length; i++)
    //     {
    //         var user = _shards[i].GetUser(username, discriminator);
    //         if (user != null)
    //             return user;
    //     }
    //
    //     return null;
    // }
    //
    // /// <inheritdoc />
    // public override ValueTask<IReadOnlyCollection<RestVoiceRegion>> GetVoiceRegionsAsync(RequestOptions options = null)
    //     => _shards[0].GetVoiceRegionsAsync();
    //
    // /// <inheritdoc />
    // public override ValueTask<RestVoiceRegion> GetVoiceRegionAsync(string id, RequestOptions options = null)
    //     => _shards[0].GetVoiceRegionAsync(id, options);
    //
    // /// <inheritdoc />
    // /// <exception cref="ArgumentNullException"><paramref name="guilds"/> is <see langword="null"/></exception>
    // public override async Task DownloadUsersAsync(IEnumerable<IGuild> guilds)
    // {
    //     if (guilds == null)
    //         throw new ArgumentNullException(nameof(guilds));
    //     for (int i = 0; i < _shards.Length; i++)
    //     {
    //         int id = _shardIds[i];
    //         var arr = guilds.Where(x => GetShardIdFor(x) == id).ToArray();
    //         if (arr.Length > 0)
    //             await _shards[i].DownloadUsersAsync(arr).ConfigureAwait(false);
    //     }
    // }

    private int GetLatency()
    {
        if (_shards == null) return -1;
        int total = _shards.Sum(x => x.Latency);
        return (int)Math.Round(total / (double)_shards.Length);
    }

    private void RegisterEvents(QQBotSocketClient client, bool isPrimary)
    {
        client.Log += msg => _logEvent.InvokeAsync(msg);
        client.LoggedOut += () =>
        {
            LoginState state = LoginState;
            if (state is LoginState.LoggedIn or LoginState.LoggingIn)
            {
                //Should only happen if token is changed
                _ = LogoutAsync(); //Signal the logout, fire and forget
            }

            return Task.CompletedTask;
        };

        client.SentRequest += (method, endpoint, millis) => _sentRequest.InvokeAsync(method, endpoint, millis);

        client.Connected += () => _shardConnectedEvent.InvokeAsync(client);
        client.Disconnected += (exception) => _shardDisconnectedEvent.InvokeAsync(exception, client);
        client.Ready += () => _shardReadyEvent.InvokeAsync(client);
        client.LatencyUpdated += (oldLatency, newLatency) => _shardLatencyUpdatedEvent.InvokeAsync(oldLatency, newLatency, client);
    }

    #endregion

    #region IQQBotClient

    /// <inheritdoc />
    ISelfUser? IQQBotClient.CurrentUser => CurrentUser;

    // /// <inheritdoc />
    // async Task<IApplication> IQQBotClient.GetApplicationInfoAsync(RequestOptions options)
    //     => await GetApplicationInfoAsync().ConfigureAwait(false);
    //
    // /// <inheritdoc />
    // Task<IChannel> IQQBotClient.GetChannelAsync(ulong id, CacheMode mode, RequestOptions options)
    //     => Task.FromResult<IChannel>(GetChannel(id));
    //
    // /// <inheritdoc />
    // Task<IReadOnlyCollection<IPrivateChannel>> IQQBotClient.GetPrivateChannelsAsync(CacheMode mode,
    //     RequestOptions options)
    //     => Task.FromResult<IReadOnlyCollection<IPrivateChannel>>(PrivateChannels);
    //
    // /// <inheritdoc />
    // async Task<IReadOnlyCollection<IConnection>> IQQBotClient.GetConnectionsAsync(RequestOptions options)
    //     => await GetConnectionsAsync().ConfigureAwait(false);
    //
    // /// <inheritdoc />
    // async Task<IInvite> IQQBotClient.GetInviteAsync(string inviteId, RequestOptions options)
    //     => await GetInviteAsync(inviteId, options).ConfigureAwait(false);
    //
    // /// <inheritdoc />
    // Task<IGuild> IQQBotClient.GetGuildAsync(ulong id, CacheMode mode, RequestOptions options)
    //     => Task.FromResult<IGuild>(GetGuild(id));
    //
    // /// <inheritdoc />
    // Task<IReadOnlyCollection<IGuild>> IQQBotClient.GetGuildsAsync(CacheMode mode, RequestOptions options)
    //     => Task.FromResult<IReadOnlyCollection<IGuild>>(Guilds);
    //
    // /// <inheritdoc />
    // async Task<IGuild> IQQBotClient.CreateGuildAsync(string name, IVoiceRegion region, Stream jpegIcon,
    //     RequestOptions options)
    //     => await CreateGuildAsync(name, region, jpegIcon).ConfigureAwait(false);
    //
    // /// <inheritdoc />
    // async Task<IUser> IQQBotClient.GetUserAsync(ulong id, CacheMode mode, RequestOptions options)
    // {
    //     var user = GetUser(id);
    //     if (user is not null || mode == CacheMode.CacheOnly)
    //         return user;
    //
    //     return await Rest.GetUserAsync(id, options).ConfigureAwait(false);
    // }
    //
    // /// <inheritdoc />
    // Task<IUser> IQQBotClient.GetUserAsync(string username, string discriminator, RequestOptions options)
    //     => Task.FromResult<IUser>(GetUser(username, discriminator));
    //
    // /// <inheritdoc />
    // async Task<IReadOnlyCollection<IVoiceRegion>> IQQBotClient.GetVoiceRegionsAsync(RequestOptions options)
    // {
    //     return await GetVoiceRegionsAsync().ConfigureAwait(false);
    // }
    //
    // /// <inheritdoc />
    // async Task<IVoiceRegion> IQQBotClient.GetVoiceRegionAsync(string id, RequestOptions options)
    // {
    //     return await GetVoiceRegionAsync(id).ConfigureAwait(false);
    // }
    //
    // /// <inheritdoc />
    // async Task<IApplicationCommand> IQQBotClient.CreateGlobalApplicationCommand(ApplicationCommandProperties properties,
    //     RequestOptions options)
    //     => await CreateGlobalApplicationCommandAsync(properties, options).ConfigureAwait(false);
    //
    // /// <inheritdoc />
    // async Task<IReadOnlyCollection<IApplicationCommand>> IQQBotClient.BulkOverwriteGlobalApplicationCommand(
    //     ApplicationCommandProperties[] properties, RequestOptions options)
    //     => await BulkOverwriteGlobalApplicationCommandsAsync(properties, options);

    #endregion

    #region Dispose

    internal override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                if (_shards != null)
                {
                    foreach (var client in _shards)
                        client?.Dispose();
                }
            }

            _isDisposed = true;
        }

        base.Dispose(disposing);
    }

    #endregion
}
