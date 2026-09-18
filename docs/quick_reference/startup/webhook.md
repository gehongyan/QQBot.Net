---
uid: Guides.QuickReference.Startup.Webhook
title: Webhook 服务端
---

# Webhook 服务端

QQ 机器人可通过 Webhook 方式接收网关事件。QQBot.Net 提供两种 Webhook 传输实现：
基于 `System.Net.HttpListener` 的独立服务端，以及基于 ASP.NET Core 的中间件。

## 使用 HttpListener

预声明变量

```csharp
readonly QQBotHttpListenerWebhookClient _webhookClient;
```

```csharp
_webhookClient = new QQBotHttpListenerWebhookClient(new QQBotHttpListenerWebhookConfig
{
    // 包含 QQBotSocketConfig 的全部配置项，此处略

    // 用于验证 Webhook 请求签名的机器人密钥
    Secret = null,
    // 监听传入 Webhook 请求的 URI 前缀
    UriPrefixes = ["http://+:5000/qqbot/"],
    // 监听器意外关闭后的自动重启间隔
    AutoRestartInterval = TimeSpan.FromSeconds(5),
    // 是否在启动时自动登录
    AutoLogin = true,
    // 是否在停止时自动登出
    AutoLogout = false
});

// 机器人的 AppID
int appId = default;
// 机器人的 AppSecret
string appSecret = null;

// 登录
await _webhookClient.LoginAsync(appId, TokenType.AppSecret, appSecret);
// 启动监听
await _webhookClient.StartAsync();
// 停止监听
await _webhookClient.StopAsync();
// 登出
await _webhookClient.LogoutAsync();
```

## 使用 ASP.NET Core

在 ASP.NET Core 应用中使用 `QQBotAspNetWebhookClient`，通过 `AddQQBotAspNetWebhookClient` 扩展方法
注册到依赖注入容器，再用 `MapQQBotWebhook` 将 Webhook 的 POST 端点映射到请求管线。
配置类为 `QQBotAspNetWebhookConfig`。该客户端实现了 `IHostedService`，其登录与启动由
泛型主机（Generic Host）生命周期驱动，因此**不应**手动调用 `StartAsync`/`StopAsync`（调用会抛出
`NotSupportedException`）。

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 注册 QQ Bot ASP.NET Webhook 客户端
builder.Services.AddQQBotAspNetWebhookClient(config =>
{
    // 包含 QQBotSocketConfig 的全部配置项，此处略

    // 机器人 AppID
    config.AppId = default;
    // 用于验证 Webhook 请求签名的机器人密钥（AppSecret）
    config.Secret = null;
    // 登录前是否验证机器人密钥格式
    config.ValidateToken = true;
    // Webhook 路由模式，默认为 "/qqbot"
    config.RoutePattern = "/qqbot";
    // 是否在主机启动时自动登录并开始接收事件
    config.AutoLogin = true;
    // 是否在主机停止时自动登出
    config.AutoLogout = false;
});

WebApplication app = builder.Build();

// 从容器解析客户端以订阅事件
QQBotAspNetWebhookClient client = app.Services.GetRequiredService<QQBotAspNetWebhookClient>();
client.Log += log =>
{
    Console.WriteLine(log.ToString());
    return Task.CompletedTask;
};
client.MessageReceived += message =>
{
    // 处理收到的消息
    return Task.CompletedTask;
};

// 映射 Webhook POST 端点（不传参时使用配置中的 RoutePattern）
app.MapQQBotWebhook();
// 或显式指定路由：app.MapQQBotWebhook("/qqbot/callback");

await app.RunAsync();
```

> [!NOTE]
> `AddQQBotAspNetWebhookClient` 也提供接收现成 `QQBotAspNetWebhookConfig` 实例的重载：
> `builder.Services.AddQQBotAspNetWebhookClient(config)`。
> 当 `AutoLogin` 为 `false` 时，客户端会在首次收到平台的 Webhook 回调验证请求时延迟登录并启动。
