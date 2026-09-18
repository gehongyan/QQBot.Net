---
uid: Guides.QuickReference.Guild.Role.ModifyChannelUserPermissions
title: 修改子频道用户权限
---

# 修改子频道用户权限

预声明变量

```csharp
INestedChannel channel = null;
IGuildMember user = null;
```

### [修改子频道用户权限]

PUT `/channels/{channel_id}/members/{user_id}/permissions`

```csharp
OverwritePermissions permissions = default; // 要设置的权限覆盖

// API 请求
await channel.ModifyPermissionsAsync(user, permissions);
```

[修改子频道用户权限]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/channel_permissions/put_channel_permissions.html
