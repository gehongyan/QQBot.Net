---
uid: Guides.QuickReference.Guild.GetChannels
title: 获取子频道列表
---

# 获取子频道列表

预声明变量

```csharp
SocketGuild socketGuild = null;
RestGuild restGuild = null;
IGuild guild = null;
```

### [获取子频道列表]

GET `/guilds/{guild_id}/channels`

```csharp
// 从缓存获取（WebSocket 客户端）——同步访问
IReadOnlyCollection<SocketGuildChannel> cachedChannels = socketGuild.Channels;

// API 请求获取 Rest 实体（Rest 客户端）
IReadOnlyCollection<RestGuildChannel> restChannels = await restGuild.GetChannelsAsync();

// 在 IGuild 接口上调用，CacheMode 控制是否允许发起 API 请求
IReadOnlyCollection<IGuildChannel> channels = await guild.GetChannelsAsync(CacheMode.AllowDownload);

// 仅获取文字子频道
IReadOnlyCollection<ITextChannel> textChannels = await guild.GetTextChannelsAsync();
```

[获取子频道列表]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/guilds_guild_id_channels.get.html
