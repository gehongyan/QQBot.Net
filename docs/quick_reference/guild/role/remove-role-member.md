---
uid: Guides.QuickReference.Guild.Role.RemoveRoleMember
title: 删除频道身份组成员
---

# 删除频道身份组成员

预声明变量

```csharp
IGuildMember member = null;
IRole role = null;
```

### [删除频道身份组成员]

DELETE `/guilds/{guild_id}/members/{user_id}/roles/{role_id}`

将成员从指定身份组移除。

```csharp
// API 请求
await member.RemoveRoleAsync(role);
// 或按身份组 ID
await member.RemoveRoleAsync(roleId: 1u);
```

[删除频道身份组成员]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/delete_guild_member_role.html
