---
uid: Guides.QuickReference.Bot.GetGuilds
title: 获取机器人频道列表
---

# 获取机器人频道列表

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;
```

### [获取机器人频道列表]

GET `/users/@me/guilds`

```csharp
// 从缓存获取（WebSocket 客户端）——同步访问，不发起 API 请求
IReadOnlyCollection<SocketGuild> cachedGuilds = _socketClient.Guilds;

// API 请求获取 Rest 实体（Rest 客户端）
IReadOnlyCollection<RestGuild> restGuilds = await _restClient.GetGuildsAsync();

// 在 IQQBotClient 接口上调用，CacheMode 控制是否允许发起 API 请求
IReadOnlyCollection<IGuild> guilds = await _socketClient.GetGuildsAsync(CacheMode.AllowDownload);
```

[获取机器人频道列表]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/users_me_guilds.get.html
