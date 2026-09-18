---
uid: Guides.QuickReference.Guild.Role.GetChannelRolePermissions
title: 获取子频道身份组权限
---

# 获取子频道身份组权限

预声明变量

```csharp
INestedChannel channel = null;
IRole role = null;
```

### [获取子频道身份组权限]

GET `/channels/{channel_id}/roles/{role_id}/permissions`

```csharp
// API 请求
ChannelPermissions permissions = await channel.GetPermissionsAsync(role);
```

[获取子频道身份组权限]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/channel_permissions/get_channel_roles_permissions.html
