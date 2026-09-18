---
uid: Guides.QuickReference.Guild.ApiPermission.GetPermissions
title: 获取机器人在频道可用权限列表
---

# 获取机器人在频道可用权限列表

预声明变量

```csharp
IGuild guild = null;
```

### [获取机器人在频道可用权限列表]

GET `/guilds/{guild_id}/api_permission`

```csharp
// API 请求
IReadOnlyCollection<ApplicationPermission> permissions =
    await guild.GetApplicationPermissionsAsync();
```

[获取机器人在频道可用权限列表]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/api_permissions/get_guild_api_permission.html
