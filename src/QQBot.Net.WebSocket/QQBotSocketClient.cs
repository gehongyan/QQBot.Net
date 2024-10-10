using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using QQBot.API;
using QQBot.API.Gateway;
using QQBot.API.Rest;
using QQBot.Logging;
using QQBot.Net;
using QQBot.Net.Converters;
using QQBot.Net.Queue;
using QQBot.Net.WebSockets;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的 QQ Bot 客户端。
/// </summary>
public partial class QQBotSocketClient : BaseSocketClient, IQQBotClient
{
    #region QQBotSocketClient

    private readonly JsonSerializerOptions _serializerOptions;

    private readonly QQBotShardedClient? _shardedClient;
    private readonly QQBotSocketClient? _parentClient;
    private readonly ConcurrentQueue<long> _heartbeatTimes;
    private readonly Logger _gatewayLogger;
    private readonly SemaphoreSlim _stateLock;
    private readonly MessageIdCache _messageIdCache;

    private Guid? _sessionId;
    private int? _lastSeq;
    private int _heartbeatInterval;
    private Task? _heartbeatTask;
    private Task? _guildDownloadTask;
    private long _lastMessageTime;
    private readonly GatewayIntents _gatewayIntents;
    private SocketSelfUser? _previousSessionUser;

    private bool _isDisposed;

    /// <inheritdoc />
    public override QQBotSocketRestClient Rest { get; }

    /// <summary>
    ///     获取此客户端的分片 ID。
    /// </summary>
    public int ShardId { get; }

    internal virtual ConnectionManager Connection { get; }

    /// <inheritdoc />
    public override ConnectionState ConnectionState => Connection.State;

    /// <inheritdoc />
    public override int Latency { get; protected set; }

    #endregion

    // From QQBotSocketConfig
    internal int TotalShards { get; private set; }
    internal int MessageCacheSize { get; private set; }
    internal ClientState State { get; private set; }
    internal WebSocketProvider WebSocketProvider { get; private set; }
    internal BaseMessageQueue MessageQueue { get; private set; }
    internal uint SmallNumberOfGuildsThreshold { get; private set; }
    internal uint LargeNumberOfGuildsThreshold { get; private set; }
    internal StartupCacheFetchMode StartupCacheFetchMode { get; private set; }
    internal bool AlwaysDownloadUsers { get; private set; }
    internal int? HandlerTimeout { get; private set; }
    internal bool LogGatewayIntentWarnings { get; private set; }
    internal bool SuppressUnknownDispatchWarnings { get; private set; }
    internal new QQBotSocketApiClient ApiClient => base.ApiClient;

    // /// <inheritdoc />
    // public override IReadOnlyCollection<SocketGuild> Guilds => State.Guilds;

    /// <summary>
    ///     初始化一个 <see cref="QQBotSocketClient" /> 类的新实例。
    /// </summary>
    public QQBotSocketClient() : this(new QQBotSocketConfig())
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="QQBotSocketClient" /> 类的新实例。
    /// </summary>
    /// <param name="config"> 用于配置此客户端的配置对象。 </param>
    public QQBotSocketClient(QQBotSocketConfig config)
        : this(config, CreateApiClient(config), null, null)
    {
    }

    internal QQBotSocketClient(QQBotSocketConfig config, QQBotShardedClient? shardedClient, QQBotSocketClient? parentClient)
        : this(config, CreateApiClient(config), shardedClient, parentClient)
    {
    }

    internal QQBotSocketClient(QQBotSocketConfig config, QQBotSocketApiClient client, QQBotShardedClient? shardedClient, QQBotSocketClient? parentClient)
        : base(config, client)
    {
        ShardId = config.ShardId ?? 0;
        TotalShards = config.TotalShards ?? 1;
        MessageCacheSize = config.MessageCacheSize;
        WebSocketProvider = config.WebSocketProvider;
        MessageQueue = config.MessageQueueProvider(ProcessGatewayEventAsync);
        SmallNumberOfGuildsThreshold = config.SmallNumberOfGuildsThreshold;
        LargeNumberOfGuildsThreshold = config.LargeNumberOfGuildsThreshold;
        // StartupCacheFetchMode will be set to the current config value whenever the socket client starts up
        StartupCacheFetchMode = config.StartupCacheFetchMode;
        AlwaysDownloadUsers = config.AlwaysDownloadUsers;
        HandlerTimeout = config.HandlerTimeout;
        State = new ClientState(0);
        Rest = new QQBotSocketRestClient(config, ApiClient);
        _heartbeatInterval = QQBotSocketConfig.HeartbeatIntervalMilliseconds;
        _heartbeatTimes = [];
        _gatewayIntents = config.GatewayIntents;
        LogGatewayIntentWarnings = config.LogGatewayIntentWarnings;
        SuppressUnknownDispatchWarnings = config.SuppressUnknownDispatchWarnings;

        _stateLock = new SemaphoreSlim(1, 1);
        _gatewayLogger = LogManager.CreateLogger("Gateway");
        _messageIdCache = new MessageIdCache();
        ConnectionManager connectionManager = new(_stateLock, _gatewayLogger, config.ConnectionTimeout,
            OnConnectingAsync, OnDisconnectingAsync, x => ApiClient.Disconnected += x);
        connectionManager.Connected += () => TimedInvokeAsync(_connectedEvent, nameof(Connected));
        connectionManager.Disconnected += (ex, _) => TimedInvokeAsync(_disconnectedEvent, nameof(Disconnected), ex);
        Connection = connectionManager;

        _shardedClient = shardedClient;
        _parentClient = parentClient;

        _serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        ApiClient.SentGatewayMessage += async socketFrameType =>
            await _gatewayLogger.DebugAsync($"Sent {socketFrameType}").ConfigureAwait(false);
        ApiClient.ReceivedGatewayEvent += ProcessMessageAsync;

        // LeftGuild += async g => await _gatewayLogger.InfoAsync($"Left {g.Name}").ConfigureAwait(false);
        // JoinedGuild += async g => await _gatewayLogger.InfoAsync($"Joined {g.Name}").ConfigureAwait(false);
        // GuildAvailable += async g => await _gatewayLogger.VerboseAsync($"Connected to {g.Name}").ConfigureAwait(false);
        // GuildUnavailable += async g => await _gatewayLogger.VerboseAsync($"Disconnected from {g.Name}").ConfigureAwait(false);
        LatencyUpdated += async (_, val) => await _gatewayLogger.DebugAsync($"Latency = {val} ms").ConfigureAwait(false);

        // GuildAvailable += g =>
        // {
        //     if (_guildDownloadTask?.IsCompleted is true
        //         && ConnectionState == ConnectionState.Connected)
        //     {
        //         if (AlwaysDownloadUsers && g.HasAllMembers is not true)
        //             _ = g.DownloadUsersAsync();
        //         if (AlwaysDownloadVoiceStates)
        //             _ = g.DownloadVoiceStatesAsync();
        //         if (AlwaysDownloadBoostSubscriptions)
        //             _ = g.DownloadBoostSubscriptionsAsync();
        //     }
        //
        //     return Task.CompletedTask;
        // };
    }

    private static QQBotSocketApiClient CreateApiClient(QQBotSocketConfig config) =>
        new(config.RestClientProvider, config.WebSocketProvider,
            config.AccessEnvironment, QQBotConfig.UserAgent,
            config.GatewayHost, defaultRatelimitCallback: config.DefaultRatelimitCallback);

    internal override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                try
                {
                    StopAsync().GetAwaiter().GetResult();
                }
                catch (NotSupportedException)
                {
                    // ignored
                }
                ApiClient?.Dispose();
                _stateLock?.Dispose();
            }

            _isDisposed = true;
        }

        base.Dispose(disposing);
    }

    private async Task OnConnectingAsync()
    {
        bool locked = false;
        if (_shardedClient != null && _sessionId == null)
        {
            await _shardedClient.AcquireIdentifyLockAsync(ShardId, Connection.CancellationToken).ConfigureAwait(false);
            locked = true;
        }
        try
        {
            await _gatewayLogger.DebugAsync("Connecting ApiClient").ConfigureAwait(false);
            await ApiClient.ConnectAsync().ConfigureAwait(false);

            if (_sessionId.HasValue)
            {
                await _gatewayLogger.DebugAsync("Resuming").ConfigureAwait(false);
                await ApiClient.SendResumeAsync(_sessionId.Value, _lastSeq).ConfigureAwait(false);
            }
            else
            {
                await _gatewayLogger.DebugAsync("Identifying").ConfigureAwait(false);
                await ApiClient.SendIdentifyAsync(ShardId, TotalShards, _gatewayIntents).ConfigureAwait(false);
            }
        }
        finally
        {
            if (locked)
                _shardedClient?.ReleaseIdentifyLock();
        }

        await Connection.WaitAsync().ConfigureAwait(false);
    }

    private async Task OnDisconnectingAsync(Exception ex)
    {
        await _gatewayLogger.DebugAsync("Disconnecting ApiClient").ConfigureAwait(false);
        await ApiClient.DisconnectAsync(ex).ConfigureAwait(false);

        //Wait for tasks to complete
        await _gatewayLogger.DebugAsync("Waiting for heartbeater").ConfigureAwait(false);
        Task? heartbeatTask = _heartbeatTask;
        if (heartbeatTask != null)
            await heartbeatTask.ConfigureAwait(false);
        _heartbeatTask = null;
        while (_heartbeatTimes.TryDequeue(out _))
        {
            // flush the queue
        }

        ResetCounter();

        // //Raise virtual GUILD_UNAVAILABLEs
        // await _gatewayLogger.DebugAsync("Raising virtual GuildUnavailables").ConfigureAwait(false);
        // foreach (SocketGuild guild in State.Guilds)
        //     if (guild.IsAvailable)
        //         await GuildUnavailableAsync(guild).ConfigureAwait(false);
    }

    private protected void ResetCounter()
    {
        _lastMessageTime = 0;
    }

    /// <inheritdoc />
    public override SocketGuild? GetGuild(ulong id) => State.GetGuild(id);

    /// <summary>
    ///     获取一个用户单聊频道。
    /// </summary>
    /// <param name="id"> 参与到单聊频道中另一位用户的用户 ID。</param>
    /// <returns> 如果找到了指定用户的单聊频道，则返回该单聊频道；否则返回 <c>null</c>。</returns>
    public SocketUserChannel? GetUserChannel(Guid id) => State.GetUserChannel(id);

    internal SocketUserChannel AddUserChannel(ClientState state, Guid id, SocketUser recipient)
    {
        SocketUserChannel channel = SocketUserChannel.Create(this, state, id, recipient);
        state.AddUserChannel(channel);
        return channel;
    }

    internal SocketUserChannel GetOrCreateUserChannel(ClientState state, Guid id, SocketUser recipient) =>
        state.GetOrAddUserChannel(id, _ => SocketUserChannel.Create(this, state, id, recipient));

    internal SocketDMChannel GetOrCreateDMChannel(ClientState state, ulong id, SocketUser recipient) =>
        state.GetOrAddDMChannel(id, _ => SocketDMChannel.Create(this, state, id, recipient));

    internal SocketGroupChannel GetOrCreateGroupChannel(ClientState state, Guid id) =>
        state.GetOrAddGroupChannel(id, _ => SocketGroupChannel.Create(this, state, id));

    internal SocketGlobalUser GetOrCreateUser(ClientState state, User model) =>
        state.GetOrAddGlobalUser(model.Id, _ => SocketGlobalUser.Create(this, state, model));

    internal SocketGlobalUser GetOrCreateSelfUser(ClientState state, User model) =>
        state.GetOrAddGlobalUser(model.Id, _ =>
        {
            SocketGlobalUser user = SocketGlobalUser.Create(this, state, model);
            user.GlobalUser.AddRef();
            return user;
        });

    internal void RemoveUser(string id) => State.RemoveGlobalUser(id);

    // /// <inheritdoc />
    // public override SocketGuild? GetGuild(ulong id) => State.GetGuild(id);
    //
    // /// <inheritdoc />
    // public override SocketChannel? GetChannel(ulong id) => State.GetChannel(id);
    //
    // /// <inheritdoc />
    // public override SocketDMChannel? GetDMChannel(Guid chatCode) => State.GetDMChannel(chatCode);
    //
    // /// <inheritdoc />
    // public override SocketDMChannel? GetDMChannel(ulong userId) => State.GetDMChannel(userId);
    //
    // /// <summary>
    // ///     获取一个频道。
    // /// </summary>
    // /// <param name="id"> 频道的 ID。 </param>
    // /// <param name="options"> 发送请求时要使用的选项。 </param>
    // /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定 ID 的频道；若指定 ID 的频道不存在，则为 <c>null</c>。 </returns>
    // public async Task<IChannel> GetChannelAsync(ulong id, RequestOptions? options = null)
    // {
    //     if (GetChannel(id) is { } channel) return channel;
    //     return await ClientHelper.GetChannelAsync(this, id, options).ConfigureAwait(false);
    // }
    //
    // /// <summary>
    // ///     获取一个私聊频道。
    // /// </summary>
    // /// <param name="chatCode"> 私聊频道的聊天代码。 </param>
    // /// <param name="options"> 发送请求时要使用的选项。 </param>
    // /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定聊天代码的私聊频道；若指定聊天代码的私聊频道不存在，则为 <c>null</c>。 </returns>
    // public async Task<IDMChannel> GetDMChannelAsync(Guid chatCode, RequestOptions? options = null) =>
    //     await ClientHelper.GetDMChannelAsync(this, chatCode, options).ConfigureAwait(false);
    //
    // /// <summary>
    // ///     获取当前会话中已创建的所有私聊频道。
    // /// </summary>
    // /// <remarks>
    // ///     <note type="warning">
    // ///         此方法不会返回当前会话之外已创建的私聊频道。如果客户端刚刚启动，这可能会返回一个空集合。
    // ///     </note>
    // /// </remarks>
    // /// <param name="options"> 发送请求时要使用的选项。 </param>
    // /// <returns> 一个表示异步获取操作的任务。任务的结果是当前会话中已创建的所有私聊频道。 </returns>
    // public async Task<IReadOnlyCollection<IDMChannel>> GetDMChannelsAsync(RequestOptions? options = null) =>
    //     (await ClientHelper.GetDMChannelsAsync(this, options).ConfigureAwait(false)).ToImmutableArray();
    //
    // /// <summary>
    // ///     获取一个用户。
    // /// </summary>
    // /// <param name="id"> 用户的 ID。 </param>
    // /// <param name="options"> 发送请求时要使用的选项。 </param>
    // /// <returns> 一个表示异步获取操作的任务。任务的结果是具有指定 ID 的用户；若指定 ID 的用户不存在，则为 <c>null</c>。 </returns>
    // public async Task<IUser> GetUserAsync(ulong id, RequestOptions? options = null)
    // {
    //     if (GetUser(id) is { } user) return user;
    //     return await Rest.GetUserAsync(id, options).ConfigureAwait(false);
    // }
    //
    // /// <inheritdoc />
    // public override SocketUser? GetUser(ulong id) => State.GetUser(id);
    //
    // /// <inheritdoc />
    // public override SocketUser? GetUser(string username, string identifyNumber) =>
    //     State.Users.FirstOrDefault(x => x.IdentifyNumber == identifyNumber && x.Username == username);

    // internal SocketGlobalUser GetOrCreateUser(ClientState state, User model) =>
    //     state.GetOrAddUser(model.Id, _ => SocketGlobalUser.Create(this, state, model));
    //
    // internal SocketUser GetOrCreateTemporaryUser(ClientState state, User model) =>
    //     state.GetUser(model.Id) ?? (SocketUser)SocketUnknownUser.Create(this, state, model.Id);
    //
    // internal SocketGlobalUser GetOrCreateSelfUser(ClientState state, User model) =>
    //     state.GetOrAddUser(model.Id, _ =>
    //     {
    //         SocketGlobalUser user = SocketGlobalUser.Create(this, state, model);
    //         user.GlobalUser.AddRef();
    //         return user;
    //     });
    //
    // internal void RemoveUser(ulong id) => State.RemoveUser(id);

    // /// <inheritdoc />
    // public override async Task DownloadUsersAsync(IEnumerable<IGuild>? guilds = null, RequestOptions? options = null)
    // {
    //     if (ConnectionState != ConnectionState.Connected) return;
    //     IEnumerable<SocketGuild> socketGuilds = (guilds ?? Guilds.Where(x => x.IsAvailable))
    //         .Select(x => GetGuild(x.Id))
    //         .OfType<SocketGuild>();
    //     await ProcessUserDownloadsAsync(socketGuilds, options).ConfigureAwait(false);
    // }
    //
    // private async Task ProcessUserDownloadsAsync(IEnumerable<SocketGuild> guilds, RequestOptions? options)
    // {
    //     foreach (SocketGuild socketGuild in guilds)
    //     {
    //         if (options?.CancellationToken.IsCancellationRequested is true) return;
    //         IEnumerable<GuildMember> guildMembers = await ApiClient
    //             .GetGuildMembersAsync(socketGuild.Id, options: options)
    //             .FlattenAsync()
    //             .ConfigureAwait(false);
    //         socketGuild.Update(State, [..guildMembers]);
    //     }
    // }
    //
    // /// <inheritdoc />
    // public override async Task DownloadVoiceStatesAsync(IEnumerable<IGuild>? guilds = null,
    //     RequestOptions? options = null)
    // {
    //     if (ConnectionState != ConnectionState.Connected) return;
    //     IEnumerable<SocketGuild> socketGuilds = (guilds ?? Guilds.Where(x => x.IsAvailable))
    //         .Select(x => GetGuild(x.Id))
    //         .OfType<SocketGuild>();
    //     await ProcessVoiceStateDownloadsAsync(socketGuilds, options).ConfigureAwait(false);
    // }
    //
    // private async Task ProcessVoiceStateDownloadsAsync(IEnumerable<SocketGuild> guilds, RequestOptions? options)
    // {
    //     foreach (SocketGuild socketGuild in guilds)
    //     {
    //         socketGuild.ResetAllVoiceStateChannels();
    //         foreach (ulong channelId in socketGuild.VoiceChannels.Select(x => x.Id))
    //         {
    //             if (options?.CancellationToken.IsCancellationRequested is true) return;
    //             if (GetChannel(channelId) is not SocketVoiceChannel channel) continue;
    //             IReadOnlyCollection<User> users = await ApiClient
    //                 .GetConnectedUsersAsync(channelId, options)
    //                 .ConfigureAwait(false);
    //             foreach (User user in users)
    //                 socketGuild.AddOrUpdateVoiceStateForJoining(user.Id, channel);
    //         }
    //
    //         GetGuildMuteDeafListResponse model = await ApiClient
    //             .GetGuildMutedDeafenedUsersAsync(socketGuild.Id)
    //             .ConfigureAwait(false);
    //         foreach (ulong id in model.Muted.UserIds)
    //             socketGuild.AddOrUpdateVoiceState(id, true);
    //         foreach (ulong id in socketGuild.Users.Select(x => x.Id).Except(model.Deafened.UserIds))
    //             socketGuild.AddOrUpdateVoiceState(id, false);
    //         foreach (ulong id in model.Deafened.UserIds)
    //             socketGuild.AddOrUpdateVoiceState(id, isDeafened: true);
    //         foreach (ulong id in socketGuild.Users.Select(x => x.Id).Except(model.Muted.UserIds))
    //             socketGuild.AddOrUpdateVoiceState(id, isDeafened: false);
    //     }
    // }
    //
    // /// <inheritdoc />
    // public override async Task DownloadBoostSubscriptionsAsync(IEnumerable<IGuild>? guilds = null,
    //     RequestOptions? options = null)
    // {
    //     if (ConnectionState != ConnectionState.Connected) return;
    //     IEnumerable<SocketGuild> socketGuilds = (guilds ?? Guilds.Where(x => x.IsAvailable))
    //         .Select(x => GetGuild(x.Id))
    //         .OfType<SocketGuild>();
    //     await ProcessBoostSubscriptionsDownloadsAsync(socketGuilds, options).ConfigureAwait(false);
    // }
    //
    // private async Task ProcessBoostSubscriptionsDownloadsAsync(IEnumerable<SocketGuild> guilds, RequestOptions? options)
    // {
    //     foreach (SocketGuild socketGuild in guilds)
    //     {
    //         if (options?.CancellationToken.IsCancellationRequested is true) return;
    //         IEnumerable<BoostSubscription> subscriptions = await ApiClient
    //             .GetGuildBoostSubscriptionsAsync(socketGuild.Id, options: options)
    //             .FlattenAsync()
    //             .ConfigureAwait(false);
    //         socketGuild.Update(State, [..subscriptions]);
    //     }
    // }

    #region ProcessMessageAsync

    internal virtual async Task ProcessMessageAsync(GatewayOpCode opCode, int? sequence, string? type, object? payload)
    {
        if (sequence.HasValue)
        {
            int expectedSeq = (_lastSeq ?? 0) + 1;
            if (sequence.Value != expectedSeq)
                await _gatewayLogger.WarningAsync($"Missed a sequence number. Expected {expectedSeq}, got {sequence.Value}.");
            _lastSeq = sequence.Value;
        }

        _lastMessageTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        try
        {
            switch (opCode)
            {
                case GatewayOpCode.Dispatch:
                    await HandleGatewayDispatchAsync(sequence, type, payload).ConfigureAwait(false);
                    break;
                case GatewayOpCode.Heartbeat:
                    await HandleGatewayHeartbeatAsync().ConfigureAwait(false);
                    break;
                case GatewayOpCode.Reconnect:
                    await HandleGatewayReconnectAsync().ConfigureAwait(false);
                    break;
                case GatewayOpCode.InvalidSession:
                    await HandleGatewayInvalidSessionAsync().ConfigureAwait(false);
                    break;
                case GatewayOpCode.Hello:
                    await HandleGatewayHelloAsync(payload).ConfigureAwait(false);
                    break;
                case GatewayOpCode.HeartbeatAck:
                    await HandleGatewayHeartbeatAckAsync().ConfigureAwait(false);
                    break;
                case GatewayOpCode.HttpCallbackAck:
                    await HandleGatewayHttpCallbackAckAsync().ConfigureAwait(false);
                    break;
                default:
                {
                    await _gatewayLogger
                        .WarningAsync($"Unknown OpCode ({opCode}). Payload: {SerializePayload(payload)}")
                        .ConfigureAwait(false);
                }
                    break;
            }
        }
        catch (Exception ex)
        {
            await _gatewayLogger
                .ErrorAsync($"Error handling {opCode}. Payload: {SerializePayload(payload)}", ex)
                .ConfigureAwait(false);
        }
    }

    internal async Task ProcessGatewayEventAsync(int sequence, string type, object payload)
    {
        await _gatewayLogger.DebugAsync($"Received Dispatch ({type})").ConfigureAwait(false);
        switch (type)
        {
            #region Connection

            case "READY":
                await HandleReadyAsync(payload).ConfigureAwait(false);
                break;
            case "RESUMED":
                await HandleResumedAsync().ConfigureAwait(false);
                break;

            #endregion

            #region Messages

            case "C2C_MESSAGE_CREATE":
                await HandleUserMessageCreatedAsync(payload, type).ConfigureAwait(false);
                break;
            case "GROUP_AT_MESSAGE_CREATE":
                await HandleGroupMessageCreatedAsync(payload, type).ConfigureAwait(false);
                break;
            case "DIRECT_MESSAGE_CREATE":
                await HandleDirectMessageCreatedAsync(payload, type).ConfigureAwait(false);
                break;
            case "AT_MESSAGE_CREATE":
            case "MESSAGE_CREATE":
                await HandleChannelMessageCreatedAsync(payload, type).ConfigureAwait(false);
                break;

            #endregion



            default:
                if (!SuppressUnknownDispatchWarnings)
                    await _gatewayLogger.WarningAsync($"Unknown Dispatch ({type})").ConfigureAwait(false);
                break;
        }
    }

    #endregion

    /// <inheritdoc />
    public override async Task StartAsync()
    {
        await MessageQueue.StartAsync();
        await Connection.StartAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task StopAsync()
    {
        await Connection.StopAsync().ConfigureAwait(false);
        await MessageQueue.StopAsync();
    }

    private async Task RunHeartbeatAsync(int intervalMillis, CancellationToken cancellationToken)
    {
        int delayInterval = (int)(intervalMillis * QQBotConfig.HeartbeatIntervalFactor);

        try
        {
            await _gatewayLogger.DebugAsync("Heartbeat Started").ConfigureAwait(false);
            while (!cancellationToken.IsCancellationRequested)
            {
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                //Did server respond to our last heartbeat, or are we still receiving messages (long load?)
                if (_heartbeatTimes.IsEmpty && now - _lastMessageTime > intervalMillis + 1000.0 / 64
                    && ConnectionState == ConnectionState.Connected && (_guildDownloadTask?.IsCompleted ?? true))
                {
                    Connection.Error(new GatewayReconnectException("Server missed last heartbeat"));
                    return;
                }

                _heartbeatTimes.Enqueue(now);
                try
                {
                    await ApiClient.SendHeartbeatAsync(_lastSeq).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await _gatewayLogger.WarningAsync("Heartbeat Errored", ex).ConfigureAwait(false);
                }

                int delay = Math.Max(0, delayInterval - Latency);
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }

            await _gatewayLogger.DebugAsync("Heartbeat Stopped").ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            await _gatewayLogger.DebugAsync("Heartbeat Stopped").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _gatewayLogger.ErrorAsync("Heartbeat Errored", ex).ConfigureAwait(false);
        }
    }

    // internal SocketGuild AddGuild(ExtendedGuild model, ClientState state)
    // {
    //     SocketGuild guild = SocketGuild.Create(this, state, model);
    //     state.AddGuild(guild);
    //     return guild;
    // }

    internal SocketGuild AddGuild(Guild model, ClientState state)
    {
        SocketGuild guild = SocketGuild.Create(this, state, model);
        state.AddGuild(guild);
        return guild;
    }
    //
    // internal SocketGuild AddGuild(RichGuild model, ClientState state)
    // {
    //     SocketGuild guild = SocketGuild.Create(this, state, model);
    //     state.AddGuild(guild);
    //     return guild;
    // }
    //
    // internal SocketGuild? RemoveGuild(ulong id) => State.RemoveGuild(id);
    //
    // internal SocketDMChannel AddDMChannel(UserChat model, ClientState state)
    // {
    //     SocketDMChannel channel = SocketDMChannel.Create(this, state, model.Code, model.Recipient);
    //     state.AddDMChannel(channel);
    //     return channel;
    // }
    //
    // internal SocketDMChannel AddDMChannel(Guid chatCode, User model, ClientState state)
    // {
    //     SocketDMChannel channel = SocketDMChannel.Create(this, state, chatCode, model);
    //     state.AddDMChannel(channel);
    //     return channel;
    // }
    //
    // internal SocketDMChannel CreateDMChannel(Guid chatCode, User model, ClientState state) =>
    //     SocketDMChannel.Create(this, state, chatCode, model);
    //
    // internal SocketDMChannel CreateDMChannel(Guid chatCode, SocketUser user, ClientState state) =>
    //     new(this, chatCode, user);
    //
    // private async Task GuildAvailableAsync(SocketGuild guild)
    // {
    //     if (guild.IsConnected) return;
    //     guild.IsConnected = true;
    //     await TimedInvokeAsync(_guildAvailableEvent, nameof(GuildAvailable), guild).ConfigureAwait(false);
    // }
    //
    // internal async Task GuildUnavailableAsync(SocketGuild guild)
    // {
    //     if (!guild.IsConnected) return;
    //     guild.IsConnected = false;
    //     await TimedInvokeAsync(_guildUnavailableEvent, nameof(GuildUnavailable), guild).ConfigureAwait(false);
    // }

    internal async Task TimedInvokeAsync(AsyncEvent<Func<Task>> eventHandler, string name)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, eventHandler.InvokeAsync).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync().ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T>(AsyncEvent<Func<T, Task>> eventHandler, string name, T arg)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg).ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T1, T2>(AsyncEvent<Func<T1, T2, Task>> eventHandler, string name, T1 arg1,
        T2 arg2)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg1, arg2)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg1, arg2).ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T1, T2, T3>(AsyncEvent<Func<T1, T2, T3, Task>> eventHandler, string name,
        T1 arg1, T2 arg2, T3 arg3)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg1, arg2, arg3)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg1, arg2, arg3).ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T1, T2, T3, T4>(AsyncEvent<Func<T1, T2, T3, T4, Task>> eventHandler,
        string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg1, arg2, arg3, arg4)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg1, arg2, arg3, arg4).ConfigureAwait(false);
    }

    internal async Task TimedInvokeAsync<T1, T2, T3, T4, T5>(AsyncEvent<Func<T1, T2, T3, T4, T5, Task>> eventHandler,
        string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (!eventHandler.HasSubscribers) return;
        if (HandlerTimeout.HasValue)
            await TimeoutWrap(name, () => eventHandler.InvokeAsync(arg1, arg2, arg3, arg4, arg5)).ConfigureAwait(false);
        else
            await eventHandler.InvokeAsync(arg1, arg2, arg3, arg4, arg5).ConfigureAwait(false);
    }

    private async Task TimeoutWrap(string name, Func<Task> action)
    {
        try
        {
            if (!HandlerTimeout.HasValue)
            {
                await action().ConfigureAwait(false);
                return;
            }

            Task timeoutTask = Task.Delay(HandlerTimeout.Value);
            Task handlersTask = action();
            if (await Task.WhenAny(timeoutTask, handlersTask).ConfigureAwait(false) == timeoutTask)
            {
                await _gatewayLogger.WarningAsync($"A {name} handler is blocking the gateway task.")
                    .ConfigureAwait(false);
            }

            await handlersTask.ConfigureAwait(false); //Ensure the handler completes
        }
        catch (Exception ex)
        {
            await _gatewayLogger.WarningAsync($"A {name} handler has thrown an unhandled exception", ex)
                .ConfigureAwait(false);
        }
    }

    // #region IQQBotClient
    //
    // /// <inheritdoc />
    // Task<IReadOnlyCollection<IGuild>> IQQBotClient.GetGuildsAsync(CacheMode mode, RequestOptions? options) =>
    //     Task.FromResult<IReadOnlyCollection<IGuild>>(Guilds);
    //
    // /// <inheritdoc />
    // Task<IGuild?> IQQBotClient.GetGuildAsync(ulong id, CacheMode mode, RequestOptions? options) =>
    //     Task.FromResult<IGuild?>(GetGuild(id));
    //
    // /// <inheritdoc />
    // async Task<IUser?> IQQBotClient.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options)
    // {
    //     if (GetUser(id) is { } user)
    //         return user;
    //     if (mode == CacheMode.CacheOnly)
    //         return null;
    //     return await Rest.GetUserAsync(id, options).ConfigureAwait(false);
    // }
    //
    // /// <inheritdoc />
    // async Task<IChannel?> IQQBotClient.GetChannelAsync(ulong id, CacheMode mode, RequestOptions? options) =>
    //     mode == CacheMode.AllowDownload ? await GetChannelAsync(id, options).ConfigureAwait(false) : GetChannel(id);
    //
    // /// <inheritdoc />
    // async Task<IDMChannel?> IQQBotClient.GetDMChannelAsync(Guid chatCode, CacheMode mode, RequestOptions? options) =>
    //     mode == CacheMode.AllowDownload
    //         ? await GetDMChannelAsync(chatCode, options).ConfigureAwait(false)
    //         : GetDMChannel(chatCode);
    //
    // /// <inheritdoc />
    // async Task<IReadOnlyCollection<IDMChannel>> IQQBotClient.GetDMChannelsAsync(CacheMode mode, RequestOptions? options) =>
    //     mode == CacheMode.AllowDownload
    //         ? await GetDMChannelsAsync(options).ConfigureAwait(false)
    //         : DMChannels;
    //
    // #endregion
}
