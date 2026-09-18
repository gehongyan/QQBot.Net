---
uid: Guides.QuickReference.Guild.GetGuild
title: 获取频道详情
---

# 获取频道详情

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;

ulong guildId = default;
```

### [获取频道详情]

GET `/guilds/{guild_id}`

```csharp
// 从缓存获取（WebSocket 客户端）——同步访问，不发起 API 请求
SocketGuild cachedGuild = _socketClient.GetGuild(guildId);
// 遍历缓存中的全部频道
IReadOnlyCollection<SocketGuild> cachedGuilds = _socketClient.Guilds;

// API 请求获取 Rest 实体（Rest 客户端）
RestGuild restGuild = await _restClient.GetGuildAsync(guildId);
```

> WebSocket 客户端的频道信息在启动缓存阶段填充；缓存的填充范围由
> <xref:QQBot.WebSocket.QQBotSocketConfig.StartupCacheFetchData> 控制。

[获取频道详情]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/guilds_guild_id.get.html
