---
uid: Guides.QuickReference.Guild.GetChannel
title: 获取子频道详情
---

# 获取子频道详情

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;

SocketGuild socketGuild = null;
RestGuild restGuild = null;
IGuild guild = null;

ulong channelId = default;
```

### [获取子频道详情]

GET `/channels/{channel_id}`

```csharp
// 从缓存获取（WebSocket 客户端）——同步访问，不发起 API 请求
SocketGuildChannel cachedChannel = socketGuild.GetChannel(channelId);
SocketTextChannel cachedTextChannel = socketGuild.GetTextChannel(channelId);
// 遍历缓存中的全部子频道
IReadOnlyCollection<SocketGuildChannel> cachedChannels = socketGuild.Channels;

// API 请求获取 Rest 实体（Rest 客户端）
RestGuildChannel restChannel = await restGuild.GetChannelAsync(channelId);

// 在 IGuild 接口上调用：CacheMode 控制是否允许发起 API 请求
// AllowDownload（默认）：缓存未命中时发起 API 请求；CacheOnly：仅查缓存，未命中返回 null
IGuildChannel channelAllowDownload = await guild.GetChannelAsync(channelId, CacheMode.AllowDownload);
IGuildChannel channelCacheOnly = await guild.GetChannelAsync(channelId, CacheMode.CacheOnly);

// 获取文字子频道
ITextChannel textChannel = await guild.GetTextChannelAsync(channelId);
```

[获取子频道详情]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/channels_channel_id.get.html
