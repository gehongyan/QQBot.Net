---
uid: Guides.QuickReference.Bot.Event.C2CMsgReceive
title: 单聊消息接收开启
---

# 单聊消息接收开启

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [单聊消息接收开启]

当用户开启接收机器人的单聊主动消息时引发。

```csharp
_client.UserActiveMessageAllowed += (channel) => Task.CompletedTask;
// channel 是接受当前用户主动消息的用户频道（SocketUserChannel）
```

[单聊消息接收开启]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/c2c_msg_receive.html
