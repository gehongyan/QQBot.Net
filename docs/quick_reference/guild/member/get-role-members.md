---
uid: Guides.QuickReference.Guild.Member.GetRoleMembers
title: 获取频道身份组成员列表
---

# 获取频道身份组成员列表

预声明变量

```csharp
IRole role = null;
```

### [获取频道身份组成员列表]

GET `/guilds/{guild_id}/roles/{role_id}/members`

```csharp
// API 请求，以分页形式获取拥有该身份组的所有成员
IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> members = role.GetUsersAsync();
// 可通过 CacheMode 控制是否允许发起 API 请求（默认 AllowDownload）
IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> cacheOnly = role.GetUsersAsync(CacheMode.CacheOnly);
```

[获取频道身份组成员列表]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/member/get_role_members.html
