---
uid: Guides.QuickReference.Startup.Rest
title: Rest 客户端
---

# Rest 客户端

预声明变量

```csharp
readonly QQBotRestClient _restClient;
```

```csharp
// 使用默认配置创建 Rest 客户端
_restClient = new QQBotRestClient();
// 使用自定义配置创建 Rest 客户端
_restClient = new QQBotRestClient(new QQBotRestConfig
{
    // 访问环境，正式环境或沙箱环境
    AccessEnvironment = AccessEnvironment.Production,
    // 默认的请求失败重试模式
    DefaultRetryMode = RetryMode.AlwaysRetry,
    // 日志记录的最低级别
    LogLevel = LogSeverity.Info,
    // 用于创建 REST 客户端的提供程序
    RestClientProvider = DefaultRestClientProvider.Instance
});

// 机器人的 AppID
int appId = default;
// 机器人的 AppSecret
string appSecret = null;

// 登录，QQ 机器人使用 AppID 与 AppSecret 进行鉴权
await _restClient.LoginAsync(appId, TokenType.AppSecret, appSecret);
// 登出
await _restClient.LogoutAsync();
```
