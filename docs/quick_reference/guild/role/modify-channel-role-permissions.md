---
uid: Guides.QuickReference.Guild.Role.ModifyChannelRolePermissions
title: 修改子频道身份组权限
---

# 修改子频道身份组权限

预声明变量

```csharp
INestedChannel channel = null;
IRole role = null;
```

### [修改子频道身份组权限]

PUT `/channels/{channel_id}/roles/{role_id}/permissions`

```csharp
OverwritePermissions permissions = default; // 要设置的权限覆盖

// API 请求
await channel.ModifyPermissionsAsync(role, permissions);
```

[修改子频道身份组权限]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/channel_permissions/put_channel_roles_permissions.html
