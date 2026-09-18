---
uid: Guides.QuickReference.Guild.Role.DeleteRole
title: 删除频道身份组
---

# 删除频道身份组

预声明变量

```csharp
IRole role = null;
```

### [删除频道身份组]

DELETE `/guilds/{guild_id}/roles/{role_id}`

```csharp
// API 请求
await role.DeleteAsync();
```

[删除频道身份组]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/delete_guild_role.html
