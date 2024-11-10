namespace QQBot.WebSocket;

public partial class QQBotShardedClient
{
    #region General
    /// <summary> Fired when a shard is connected to the QQBot gateway. </summary>
    public event Func<QQBotSocketClient, Task> ShardConnected
    {
        add { _shardConnectedEvent.Add(value); }
        remove { _shardConnectedEvent.Remove(value); }
    }
    private readonly AsyncEvent<Func<QQBotSocketClient, Task>> _shardConnectedEvent = new AsyncEvent<Func<QQBotSocketClient, Task>>();
    /// <summary> Fired when a shard is disconnected from the QQBot gateway. </summary>
    public event Func<Exception, QQBotSocketClient, Task> ShardDisconnected
    {
        add { _shardDisconnectedEvent.Add(value); }
        remove { _shardDisconnectedEvent.Remove(value); }
    }
    private readonly AsyncEvent<Func<Exception, QQBotSocketClient, Task>> _shardDisconnectedEvent = new AsyncEvent<Func<Exception, QQBotSocketClient, Task>>();
    /// <summary> Fired when a guild data for a shard has finished downloading. </summary>
    public event Func<QQBotSocketClient, Task> ShardReady
    {
        add { _shardReadyEvent.Add(value); }
        remove { _shardReadyEvent.Remove(value); }
    }
    private readonly AsyncEvent<Func<QQBotSocketClient, Task>> _shardReadyEvent = new AsyncEvent<Func<QQBotSocketClient, Task>>();
    /// <summary> Fired when a shard receives a heartbeat from the QQBot gateway. </summary>
    public event Func<int, int, QQBotSocketClient, Task> ShardLatencyUpdated
    {
        add { _shardLatencyUpdatedEvent.Add(value); }
        remove { _shardLatencyUpdatedEvent.Remove(value); }
    }
    private readonly AsyncEvent<Func<int, int, QQBotSocketClient, Task>> _shardLatencyUpdatedEvent = new AsyncEvent<Func<int, int, QQBotSocketClient, Task>>();
    #endregion
}
