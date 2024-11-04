using QQBot.Net.Queue;
using QQBot.Net.Queue.SynchronousImmediate;
using QQBot.Net.WebSockets;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个用于 <see cref="QQBot.WebSocket.QQBotSocketClient"/> 的配置类。
/// </summary>
/// <remarks>
///     此配置基于 <see cref="QQBot.Rest.QQBotRestConfig"/>，在与 REST 有关的配置的基础上，定义了有关网关的配置。
/// </remarks>
/// <example>
///     以下代码启用了消息缓存，并配置客户端在频道可用时始终下载所有可用数据，这包括了频道列表、角色列表、频道用户列表。
///     <code language="cs">
///     var config = new QQBotSocketConfig
///     {
///         StartupCacheFetchData = StartupCacheFetchData.All,
///         MessageCacheSize = 100
///     };
///     var client = new QQBotSocketClient(config);
///     </code>
/// </example>
public class QQBotSocketConfig : QQBotRestConfig
{
    /// <summary>
    ///     获取网关使用的数据格式。
    /// </summary>
    public const string GatewayEncoding = "json";

    /// <summary>
    ///     获取或设置要连接的网关地址。如果为 <c>null</c>，则客户端将会通过 API 请求获取网关地址。
    /// </summary>
    public string? GatewayHost { get; set; }

    /// <summary>
    ///     获取或设置连接到网关时的超时时间间隔（毫秒）。
    /// </summary>
    public int ConnectionTimeout { get; set; } = 6000;

    /// <summary>
    ///     获取或设置此分片的 ID。必须小于 <see cref="TotalShards"/>。
    /// </summary>
    public int? ShardId { get; set; }

    /// <summary>
    ///     获取或设置此应用程序的总分片数。
    /// </summary>
    /// <remarks>
    ///     如果在分片客户端中将此属性设置为 <see langword="null"/>，则 Bot 将从 API 获取推荐的分片数量并使用。
    /// </remarks>
    public int? TotalShards { get; set; }

    /// <summary>
    ///     获取网关发送心跳包的时间间隔（毫秒）。
    /// </summary>
    internal const int HeartbeatIntervalMilliseconds = 42500;

    /// <summary>
    ///     获取语音客户端 RTP 连接中发送 RTCP 数据报的时间间隔（毫秒）。
    /// </summary>
    public const int RtcpIntervalMilliseconds = 5000;

    /// <summary>
    ///     获取或设置阻塞网关线程的事件处理程序的超时时间间隔（毫秒），超过此时间间隔的阻塞网关线程的事件处理程序会被日志记录警告。将此属性设置为 <c>null</c> 将禁用此检查。
    /// </summary>
    public int? HandlerTimeout { get; set; } = 3000;

    /// <summary>
    ///     Gets or sets the maximum identify concurrency.
    /// </summary>
    /// <remarks>
    ///     This information is provided by Discord.
    ///     It is only used when using a <see cref="QQBotShardedClient"/> and auto-sharding is disabled.
    /// </remarks>
    public int IdentifyMaxConcurrency { get; set; } = 1;

    /// <summary>
    ///     获取或设置被视为加入少量频道的阈值数量。
    /// </summary>
    /// <seealso cref="QQBot.WebSocket.StartupCacheFetchMode.Auto"/>
    public uint SmallNumberOfGuildsThreshold { get; set; } = 5;

    /// <summary>
    ///     获取或设置被视为加入大量频道的阈值数量。
    /// </summary>
    /// <seealso cref="QQBot.WebSocket.StartupCacheFetchMode.Auto"/>
    public uint LargeNumberOfGuildsThreshold { get; set; } = 50;

    /// <summary>
    ///     获取或设置应在缓存中保留的每个子频道的消息数量。将此属性设置为零将完全禁用消息缓存。
    /// </summary>
    public int MessageCacheSize { get; set; } = 10;

    /// <summary>
    ///     获取或设置用于创建 WebSocket 客户端的委托。
    /// </summary>
    public WebSocketProvider WebSocketProvider { get; set; }

    /// <summary>
    ///     获取或设置在启动时缓存获取模式。
    /// </summary>
    /// <remarks>
    ///     此属性用于指定客户端在启动时如何缓存基础数据，并影响 <see cref="QQBot.WebSocket.QQBotSocketClient.Ready"/> 事件的引发时机。 <br />
    ///     缓存基础数据包括频道基本信息、子频道、角色、子频道权限重写、当前用户在频道内的昵称。
    /// </remarks>
    public StartupCacheFetchMode StartupCacheFetchMode { get; set; } = StartupCacheFetchMode.Auto;

    /// <summary>
    ///     获取或设置在启动时缓存获取的数据。
    /// </summary>
    /// <remarks>
    ///     此属性用于指定客户端在启动时应该获取的数据。默认情况下，客户端将获取公域机器人可以获取的所有数据，即
    ///     <see cref="QQBot.WebSocket.StartupCacheFetchData.AllPublicDomain"/>，如私域机器人需在启动时获取所有可用数据，请将此属性设置为
    ///     <see cref="QQBot.WebSocket.StartupCacheFetchData.All"/>。
    /// </remarks>
    public StartupCacheFetchData StartupCacheFetchData { get; set; } = StartupCacheFetchData.AllPublicDomain;

    /// <summary>
    ///     获取或设置用于创建消息队列的委托。
    /// </summary>
    public MessageQueueProvider MessageQueueProvider { get; set; }

    /// <summary>
    ///     获取或设置网关意图以限制从 QQ Bot 网关下发的事件。默认值为 <see cref="GatewayIntents.AllPublicDomain"/>。
    /// </summary>
    /// <remarks>
    ///     更多信息，请参见 QQ Bot API 官方文档上的有关网关意图的说明：
    ///     <see href="https://bot.q.qq.com/wiki/develop/api/gateway/intents.html">v1</see>
    ///     <see href="https://bot.q.qq.com/wiki/develop/api-v2/dev-prepare/interface-framework/event-emit.html#事件订阅Intents">v2</see>。
    /// </remarks>
    public GatewayIntents GatewayIntents { get; set; } = GatewayIntents.AllPublicDomain;

    /// <summary>
    ///     获取或设置是否记录与频道意图和事件相关的警告。
    /// </summary>
    public bool LogGatewayIntentWarnings { get; set; } = true;

    /// <summary>
    ///     获取或设置是否应记录未知的网关事件消息。
    /// </summary>
    public bool SuppressUnknownDispatchWarnings { get; set; } = true;

    /// <summary>
    ///     初始化一个 <see cref="QQBotSocketConfig"/> 类的新实例。
    /// </summary>
    public QQBotSocketConfig()
    {
        WebSocketProvider = DefaultWebSocketProvider.Instance;
        MessageQueueProvider = SynchronousImmediateMessageQueueProvider.Instance;
    }

    internal QQBotSocketConfig Clone() => (QQBotSocketConfig)MemberwiseClone();
}
