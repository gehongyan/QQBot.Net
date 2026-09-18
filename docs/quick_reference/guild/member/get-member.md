---
uid: Guides.QuickReference.Guild.Member.GetMember
title: 获取频道成员详情
---

# 获取频道成员详情

预声明变量

```csharp
SocketGuild socketGuild = null;
RestGuild restGuild = null;
IGuild guild = null;
IGuildChannel guildChannel = null;

ulong userId = default;
```

### [获取频道成员详情]

GET `/guilds/{guild_id}/members/{user_id}`

```csharp
// API 请求获取 Rest 实体（Rest 客户端）
RestGuildMember restMember = await restGuild.GetUserAsync(userId);

// 在 IGuild 接口上调用，CacheMode 控制是否允许发起 API 请求
// AllowDownload（默认）：缓存未命中时请求；CacheOnly：仅查缓存，未命中返回 null
IGuildMember member = await guild.GetUserAsync(userId, CacheMode.AllowDownload);
IGuildMember cachedOnly = await guild.GetUserAsync(userId, CacheMode.CacheOnly);

// 在子频道接口上按用户获取该频道成员
IGuildUser channelUser = await guildChannel.GetUserAsync(userId, CacheMode.AllowDownload);
```

[获取频道成员详情]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/member/get_member.html
