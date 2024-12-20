using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using QQBot.API;
using QQBot.API.Gateway;
using QQBot.Rest;

namespace QQBot.WebSocket;

public partial class QQBotSocketClient
{
    #region Gateway

    private async Task HandleGatewayDispatchAsync(int? sequence, string? type, object? payload)
    {
        if (type is null)
        {
            await _gatewayLogger.WarningAsync("Received Dispatch with no type").ConfigureAwait(false);
            return;
        }
        if (payload is null)
        {
            await _gatewayLogger.WarningAsync($"Received Dispatch with no payload ({type})").ConfigureAwait(false);
            return;
        }
        await MessageQueue.EnqueueAsync(sequence ?? _lastSeq ?? 0, type, payload).ConfigureAwait(false);
    }

    private async Task HandleGatewayHeartbeatAsync()
    {
        await _gatewayLogger.DebugAsync("Received Heartbeat").ConfigureAwait(false);
        await ApiClient.SendHeartbeatAsync(_lastSeq).ConfigureAwait(false);
    }

    private async Task HandleGatewayReconnectAsync()
    {
        await _gatewayLogger.DebugAsync("Received Reconnect").ConfigureAwait(false);
        Connection.Error(new GatewayReconnectException("Server requested a reconnect"));
    }

    private async Task HandleGatewayInvalidSessionAsync()
    {
        await _gatewayLogger.DebugAsync("Received InvalidSession").ConfigureAwait(false);
        await _gatewayLogger.WarningAsync("Failed to resume previous session").ConfigureAwait(false);

        _sessionId = null;
        _lastSeq = 0;
        _messageIdCache.Clear();

        if (_shardedClient != null)
        {
            await _shardedClient.AcquireIdentifyLockAsync(ShardId, Connection.CancellationToken).ConfigureAwait(false);
            try
            {
                await ApiClient.SendIdentifyAsync(ShardId, TotalShards, _gatewayIntents).ConfigureAwait(false);
            }
            finally
            {
                _shardedClient.ReleaseIdentifyLock();
            }
        }
        else
            await ApiClient.SendIdentifyAsync(ShardId, TotalShards, _gatewayIntents).ConfigureAwait(false);
    }

    private async Task HandleGatewayHelloAsync(object? payload)
    {
        if (DeserializePayload<GatewayHelloPayload>(payload) is not { } gatewayHelloPayload) return;
        await _gatewayLogger.DebugAsync("Received Hello").ConfigureAwait(false);
        try
        {
            _heartbeatInterval = gatewayHelloPayload.HeartbeatInterval;
        }
        catch (Exception ex)
        {
            Connection.CriticalError(new Exception("Processing Hello failed", ex));
            return;
        }
    }

    private async Task HandleGatewayHeartbeatAckAsync()
    {
        await _gatewayLogger.DebugAsync("Received HeartbeatAck").ConfigureAwait(false);

        if (_heartbeatTimes.TryDequeue(out long time))
        {
            int latency = (int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - time);
            int before = Latency;
            Latency = latency;

            await TimedInvokeAsync(_latencyUpdatedEvent, nameof(LatencyUpdated), before, latency).ConfigureAwait(false);
        }
    }

    private async Task HandleGatewayHttpCallbackAckAsync()
    {
        await _gatewayLogger.DebugAsync("Received HttpCallbackAck").ConfigureAwait(false);
    }

    internal async Task FetchRequiredDataAsync()
    {
        // Get current user
        try
        {
            SelfUser selfUser = await ApiClient.GetSelfUserAsync().ConfigureAwait(false);
            SocketSelfUser currentUser = SocketSelfUser.Create(this, State, selfUser);
            Rest.CreateRestSelfUser(selfUser);
            ApiClient.CurrentUserId = currentUser.Id;
            Rest.CurrentUser = RestSelfUser.Create(this, selfUser);
            CurrentUser = currentUser;
            _previousSessionUser = CurrentUser;
        }
        catch (Exception ex)
        {
            Connection.CriticalError(new Exception("Processing SelfUser failed", ex));
            return;
        }

        // Download guild data
        try
        {
            RequestOptions requestOptions = new();
            List<Guild> models = [..await SocketClientHelper.GetGuildsAsync(this, null, requestOptions).FlattenAsync().ConfigureAwait(false)];
            StartupCacheFetchMode = BaseConfig.StartupCacheFetchMode;
            if (StartupCacheFetchMode is StartupCacheFetchMode.Auto)
            {
                if (models.Count >= LargeNumberOfGuildsThreshold)
                    StartupCacheFetchMode = StartupCacheFetchMode.Lazy;
                else if (models.Count >= SmallNumberOfGuildsThreshold)
                    StartupCacheFetchMode = StartupCacheFetchMode.Asynchronous;
                else
                    StartupCacheFetchMode = StartupCacheFetchMode.Synchronous;
            }

            ClientState state = new(models.Count);
            foreach (Guild guild in models)
            {
                SocketGuild socketGuild = AddGuild(guild, state);
                if (StartupCacheFetchMode is StartupCacheFetchMode.Lazy)
                {
                    if (socketGuild.IsAvailable)
                        await GuildAvailableAsync(socketGuild).ConfigureAwait(false);
                    else
                        await GuildUnavailableAsync(socketGuild).ConfigureAwait(false);
                }
            }

            State = state;

            if (StartupCacheFetchMode is StartupCacheFetchMode.Synchronous
                && state.Guilds.Count > LargeNumberOfGuildsThreshold)
            {
                await _gatewayLogger
                    .WarningAsync($"The client is in synchronous startup mode and has joined {state.Guilds.Count} guilds. "
                        + "This may cause the client to take a long time to start up with blocking the gateway, "
                        + "which may result in a timeout or socket disconnection. "
                        + "Consider using asynchronous mode or lazy mode.").ConfigureAwait(false);
            }

            _guildDownloadTask = StartupCacheFetchMode is not StartupCacheFetchMode.Lazy
                ? DownloadGuildDataAsync(state.Guilds, Connection.CancellationToken)
                : Task.CompletedTask;
            _ = Connection.CompleteAsync();

            if (StartupCacheFetchMode is StartupCacheFetchMode.Synchronous)
                await _guildDownloadTask.ConfigureAwait(false);

            await TimedInvokeAsync(_readyEvent, nameof(Ready)).ConfigureAwait(false);
            await _gatewayLogger.InfoAsync("Ready").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Connection.CriticalError(new Exception("Processing Guilds failed", ex));
            return;
        }
    }

    private async Task DownloadGuildDataAsync(IEnumerable<SocketGuild> socketGuilds, CancellationToken cancellationToken)
    {
        try
        {
            await _gatewayLogger.DebugAsync("GuildDownloader Started").ConfigureAwait(false);

            foreach (SocketGuild socketGuild in socketGuilds)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                if (!socketGuild.IsAvailable)
                    await socketGuild.UpdateAsync(new RequestOptions { CancellationToken = cancellationToken }).ConfigureAwait(false);

                if (socketGuild.IsAvailable)
                    await GuildAvailableAsync(socketGuild).ConfigureAwait(false);
                else
                    await GuildUnavailableAsync(socketGuild).ConfigureAwait(false);
            }

            await _gatewayLogger.DebugAsync("GuildDownloader Stopped").ConfigureAwait(false);

            // Download user list if enabled
            if (BaseConfig.StartupCacheFetchData.HasFlag(StartupCacheFetchData.GuildUsers))
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        IEnumerable<SocketGuild> availableGuilds = Guilds
                            .Where(x => x.IsAvailable && x.HasAllMembers is not true);
                        await DownloadUsersAsync(availableGuilds, new RequestOptions
                        {
                            CancellationToken = cancellationToken
                        });
                    }
                    catch (Exception ex)
                    {
                        await _gatewayLogger.WarningAsync("Downloading users failed", ex).ConfigureAwait(false);
                    }
                }, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            await _gatewayLogger.DebugAsync("GuildDownloader Stopped").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _gatewayLogger.ErrorAsync("GuildDownloader Errored", ex).ConfigureAwait(false);
        }
    }

    private async Task HandleReadyAsync(object? payload)
    {
        if (DeserializePayload<ReadyEvent>(payload) is not { } data) return;
        _sessionId = data.SessionId;
        _heartbeatTask = RunHeartbeatAsync(_heartbeatInterval, Connection.CancellationToken);
        await FetchRequiredDataAsync();
    }

    private async Task HandleResumedAsync()
    {
        _ = Connection.CompleteAsync();
        _heartbeatTask = RunHeartbeatAsync(_heartbeatInterval, Connection.CancellationToken);
        //Notify the client that these guilds are available again
        foreach (SocketGuild guild in State.Guilds)
        {
            if (guild.IsAvailable)
                await GuildAvailableAsync(guild).ConfigureAwait(false);
        }
        // Restore the previous sessions current user
        CurrentUser = _previousSessionUser;
        await _gatewayLogger.InfoAsync("Resumed previous session").ConfigureAwait(false);
    }

    #endregion

    #region Messages

    private async Task HandleUserMessageCreatedAsync(object? payload, string dispatch)
    {
        if (DeserializePayload<MessageCreatedEvent>(payload) is not { } data) return;
        SocketUser author = State.GetOrAddGlobalUser(data.Author.Id,
            _ => SocketGlobalUser.Create(this, State, data.Author));
        SocketUserChannel channel = GetOrCreateUserChannel(State, data.Author.Id, author);
        SocketUserMessage message = SocketUserMessage.Create(this, State, channel, author, data, dispatch);
        SocketChannelHelper.AddMessage(channel, this, message);
        await TimedInvokeAsync(_messageReceivedEvent, nameof(MessageReceived), message).ConfigureAwait(false);
    }

    private async Task HandleGroupMessageCreatedAsync(object? payload, string dispatch)
    {
        if (DeserializePayload<MessageCreatedEvent>(payload) is not { } data) return;
        if (!data.GroupId.HasValue)
        {
            await LogGatewayErrorAsync(dispatch,
                "Received GroupMessageCreated with no GroupId.", payload).ConfigureAwait(false);
            return;
        }
        SocketUser author = State.GetOrAddGlobalUser(data.Author.Id,
            _ => SocketGlobalUser.Create(this, State, data.Author));
        SocketGroupChannel channel = GetOrCreateGroupChannel(State, data.GroupId.Value);
        SocketUserMessage message = SocketUserMessage.Create(this, State, channel, author, data, dispatch);
        SocketChannelHelper.AddMessage(channel, this, message);
        await TimedInvokeAsync(_messageReceivedEvent, nameof(MessageReceived), message).ConfigureAwait(false);
    }

    private async Task HandleDirectMessageCreatedAsync(object? payload, string dispatch)
    {
        if (DeserializePayload<ChannelMessage>(payload) is not { } data) return;
        SocketGuildUser author = State.GetOrAddGuildUser(data.Author.Id,
            _ => SocketGuildUser.Create(this, State, data.Author));
        SocketDMChannel channel = GetOrCreateDMChannel(State, data.GuildId, author);
        SocketUserMessage message = SocketUserMessage.Create(this, State, channel, author, data, dispatch);
        SocketChannelHelper.AddMessage(channel, this, message);
        await TimedInvokeAsync(_messageReceivedEvent, nameof(MessageReceived), message).ConfigureAwait(false);
    }

    private async Task HandleChannelMessageCreatedAsync(object? payload, string dispatch)
    {
        if (DeserializePayload<ChannelMessage>(payload) is not { } data) return;
        if (!_messageIdCache.TryAdd(data.Id)) return;
        if (GetGuild(data.GuildId) is not { } guild)
        {
            await UnknownGuildAsync(dispatch, data.GuildId, payload).ConfigureAwait(false);
            return;
        }
        if (guild.GetTextChannel(data.ChannelId) is not { } channel)
        {
            await UnknownChannelAsync(dispatch, data.ChannelId, payload).ConfigureAwait(false);
            return;
        }
        SocketGuildMember author = guild.GetUser(data.Author.Id) ?? guild.AddOrUpdateUser(data.Author, data.Member);
        SocketUserMessage message = SocketUserMessage.Create(this, State, channel, author, data, dispatch);
        SocketChannelHelper.AddMessage(channel, this, message);
        await TimedInvokeAsync(_messageReceivedEvent, nameof(MessageReceived), message).ConfigureAwait(false);
    }

    #endregion

    #region Raising Events

    private async Task GuildAvailableAsync(SocketGuild guild)
    {
        if (guild.IsConnected) return;
        guild.IsConnected = true;
        await TimedInvokeAsync(_guildAvailableEvent, nameof(GuildAvailable), guild).ConfigureAwait(false);
    }

    internal async Task GuildUnavailableAsync(SocketGuild guild)
    {
        if (!guild.IsConnected) return;
        guild.IsConnected = false;
        await TimedInvokeAsync(_guildUnavailableEvent, nameof(GuildUnavailable), guild).ConfigureAwait(false);
    }

    private async Task UnknownGuildAsync(string dispatch, ulong guildId, object? payload) =>
        await LogGatewayErrorAsync(dispatch, $"Unknown GuildId: {guildId}.", payload).ConfigureAwait(false);

    private async Task UnknownChannelAsync(string dispatch, ulong channelId, object? payload) =>
        await LogGatewayErrorAsync(dispatch, $"Unknown ChannelId: {channelId}.", payload).ConfigureAwait(false);

    private async Task LogGatewayErrorAsync(string dispatch, string message, object? payload) =>
        await _gatewayLogger.WarningAsync($"{message} Event: {dispatch}. Payload: {SerializePayload(payload)}").ConfigureAwait(false);

    #endregion

    #region Helpers

    [return: NotNullIfNotNull(nameof(payload))]
    private T? DeserializePayload<T>(object? payload)
    {
        if (payload is null) return default;
        if (payload is T targetTypedObject) return targetTypedObject;
        if (payload is JsonElement jsonElement)
        {
            if (jsonElement.Deserialize<T>(_serializerOptions) is not { } deserializedFromJsonElement)
                throw new InvalidOperationException($"Failed to deserialize payload to {typeof(T).Name}");
            return deserializedFromJsonElement;
        }
        if (JsonSerializer.Deserialize<T>(SerializePayload(payload), _serializerOptions) is not { } deserializedFromJson)
            throw new InvalidOperationException($"Failed to deserialize payload to {typeof(T).Name}");
        return deserializedFromJson;
    }

    [return: NotNullIfNotNull(nameof(payload))]
    private string? SerializePayload(object? payload) =>
        payload is not null ? JsonSerializer.Serialize(payload, _serializerOptions) : null;

    #endregion
}
