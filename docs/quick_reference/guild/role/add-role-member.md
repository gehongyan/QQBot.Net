---
uid: Guides.QuickReference.Guild.Role.AddRoleMember
title: 创建频道身份组成员
---

# 创建频道身份组成员

预声明变量

```csharp
IGuildMember member = null;
IRole role = null;
```

### [创建频道身份组成员]

PUT `/guilds/{guild_id}/members/{user_id}/roles/{role_id}`

将成员添加到指定身份组。

```csharp
// API 请求
await member.AddRoleAsync(role);
// 或按身份组 ID
await member.AddRoleAsync(roleId: 1u);
```

[创建频道身份组成员]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/put_guild_member_role.html
