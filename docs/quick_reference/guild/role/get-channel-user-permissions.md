---
uid: Guides.QuickReference.Guild.Role.GetChannelUserPermissions
title: 获取子频道用户权限
---

# 获取子频道用户权限

预声明变量

```csharp
INestedChannel channel = null;
IGuildMember user = null;
```

### [获取子频道用户权限]

GET `/channels/{channel_id}/members/{user_id}/permissions`

```csharp
// API 请求
ChannelPermissions permissions = await channel.GetPermissionsAsync(user);
```

[获取子频道用户权限]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role-group/channel_permissions/get_channel_permissions.html
