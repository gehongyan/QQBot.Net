---
uid: Guides.QuickReference.Guild.Role.CreateRole
title: 创建频道身份组
---

# 创建频道身份组

预声明变量

```csharp
IGuild guild = null;
```

### [创建频道身份组]

POST `/guilds/{guild_id}/roles`

```csharp
// API 请求
IRole role = await guild.CreateRoleAsync(props =>
{
    // 身份组属性配置（名称、颜色、是否在成员列表单独展示等）
});
```

[创建频道身份组]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/post_guild_role.html
