---
uid: Guides.QuickReference.Bot.GetBot
title: 获取机器人详情
---

# 获取机器人详情

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;
```

### [获取机器人详情]

GET `/users/@me`

当前登录的机器人用户在登录后由客户端缓存，通过 <xref:QQBot.IQQBotClient.CurrentUser> 同步访问，类型为 <xref:QQBot.ISelfUser>。

```csharp
// WebSocket 客户端：缓存的当前用户（RestSelfUser 派生的 SocketSelfUser）
ISelfUser socketSelf = _socketClient.CurrentUser;

// Rest 客户端：登录后同样缓存于 CurrentUser
ISelfUser restSelf = _restClient.CurrentUser;
```

[获取机器人详情]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/users_me.get.html
