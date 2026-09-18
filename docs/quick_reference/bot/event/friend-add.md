---
uid: Guides.QuickReference.Bot.Event.FriendAdd
title: 用户添加好友
---

# 用户添加好友

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [用户添加好友]

当用户添加机器人为好友时引发。

```csharp
_client.UserAdded += (channel, source, callbackData) => Task.CompletedTask;
// channel 是添加当前用户的用户频道（SocketUserChannel）
// source 是添加来源场景（UserChannelSource）
// callbackData 是分享链接中携带的开发者自定义回调数据（可为 null）
```

[用户添加好友]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/friend_add.html
