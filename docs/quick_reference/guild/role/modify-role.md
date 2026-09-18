---
uid: Guides.QuickReference.Guild.Role.ModifyRole
title: 修改频道身份组
---

# 修改频道身份组

预声明变量

```csharp
IRole role = null;
```

### [修改频道身份组]

PATCH `/guilds/{guild_id}/roles/{role_id}`

```csharp
// API 请求
await role.ModifyAsync(props =>
{
    // 要修改的身份组属性
});
```

[修改频道身份组]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/patch_guild_role.html
