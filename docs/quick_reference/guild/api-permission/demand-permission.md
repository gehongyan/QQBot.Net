---
uid: Guides.QuickReference.Guild.ApiPermission.DemandPermission
title: 发送机器人在频道接口权限的授权链接
---

# 发送机器人在频道接口权限的授权链接

预声明变量

```csharp
IGuild guild = null;
ITextChannel textChannel = null;
```

### [发送接口权限授权链接]

POST `/guilds/{guild_id}/api_permission/demand`

向指定子频道发送一条接口权限授权链接消息，引导频道管理员授权。

```csharp
string title = null;                    // 接口权限描述信息
ApplicationPermission permission = null; // 要请求的权限

// 在频道上按权限对象请求
await guild.RequestApplicationPermissionAsync(textChannel, title, permission);

// 或在文字子频道上直接请求
await textChannel.RequestApplicationPermissionAsync(title, permission);
```

[发送接口权限授权链接]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/api_permissions/post_api_permission_demand.html
