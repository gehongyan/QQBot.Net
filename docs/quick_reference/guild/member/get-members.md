---
uid: Guides.QuickReference.Guild.Member.GetMembers
title: 获取频道成员列表
---

# 获取频道成员列表

预声明变量

```csharp
SocketGuild socketGuild = null;
IGuild guild = null;
```

### [获取频道成员列表]

GET `/guilds/{guild_id}/members`

```csharp
// 从缓存获取（WebSocket 客户端）——同步访问已缓存的成员
IReadOnlyCollection<SocketGuildMember> cachedMembers = socketGuild.Users;
SocketGuildMember cachedMember = socketGuild.GetUser(userId: 0ul);

// API 请求，以分页形式获取频道内所有成员
IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> members = guild.GetUsersAsync();
```

> 缓存成员的范围取决于启动缓存配置与运行时收到的成员事件；完整列表请以 `GetUsersAsync` 的分页结果为准。

[获取频道成员列表]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/member/get_members.html
