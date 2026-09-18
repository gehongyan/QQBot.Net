---
uid: Guides.QuickReference.Bot.Event.FriendDelete
title: 用户删除好友
---

# 用户删除好友

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [用户删除好友]

当用户将机器人从好友中移除时引发。

```csharp
_client.UserRemoved += (channel) => Task.CompletedTask;
// channel 是移除当前用户的用户频道（SocketUserChannel）
```

[用户删除好友]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/friend_del.html
