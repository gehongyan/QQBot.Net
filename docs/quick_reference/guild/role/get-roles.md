---
uid: Guides.QuickReference.Guild.Role.GetRoles
title: 获取频道身份组列表
---

# 获取频道身份组列表

预声明变量

```csharp
IGuild guild = null;
```

### [获取频道身份组列表]

GET `/guilds/{guild_id}/roles`

```csharp
// API 请求
IReadOnlyCollection<IRole> roles = await guild.GetRolesAsync();
```

[获取频道身份组列表]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/get_guild_roles.html
