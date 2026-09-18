---
uid: Guides.QuickReference.Startup.WebSocket
title: WebSocket 客户端
---

# WebSocket 客户端

预声明变量

```csharp
readonly QQBotSocketClient _socketClient;
```

```csharp
// 使用默认配置创建 WebSocket 客户端
_socketClient = new QQBotSocketClient();
// 使用自定义配置创建 WebSocket 客户端
_socketClient = new QQBotSocketClient(new QQBotSocketConfig
{
    // 包含 QQBotRestConfig 的全部配置项，此处略

    // 网关意图，用于限制网关下发的事件
    GatewayIntents = GatewayIntents.AllPublicDomain,
    // 显式指定网关地址，为 null 时通过 API 请求获取
    GatewayHost = null,
    // 连接网关的超时时间（毫秒）
    ConnectionTimeout = 6000,
    // 分片 ID
    ShardId = null,
    // 总分片数
    TotalShards = null,
    // 处理程序警告耗时阈值（毫秒）
    HandlerTimeout = 3000,
    // 是否自动确认未被用户代码回应的互动事件
    AutoAcknowledgeInteractions = true,
    // 自动确认互动事件前等待用户代码回应的时间（毫秒）
    InteractionAutoAcknowledgeDelay = 2000,
    // 被视为加入少量频道的阈值数量
    SmallNumberOfGuildsThreshold = 5,
    // 被视为加入大量频道的阈值数量
    LargeNumberOfGuildsThreshold = 50,
    // 消息缓存数量，设置为零将禁用消息缓存
    MessageCacheSize = 10,
    // 启动时缓存获取模式
    StartupCacheFetchMode = StartupCacheFetchMode.Auto,
    // 启动时缓存获取的数据范围
    StartupCacheFetchData = StartupCacheFetchData.AllPublicDomain,
    // WebSocket 客户端提供程序
    WebSocketProvider = DefaultWebSocketProvider.Instance,
    // 消息队列提供程序
    MessageQueueProvider = SynchronousImmediateMessageQueueProvider.Instance,
    // 是否记录与网关意图和事件相关的警告
    LogGatewayIntentWarnings = true,
    // 是否记录未知的网关事件消息
    SuppressUnknownDispatchWarnings = true
});

// 机器人的 AppID
int appId = default;
// 机器人的 AppSecret
string appSecret = null;

// 登录
await _socketClient.LoginAsync(appId, TokenType.AppSecret, appSecret);
// 启动
await _socketClient.StartAsync();
// 停止
await _socketClient.StopAsync();
// 登出
await _socketClient.LogoutAsync();
```
