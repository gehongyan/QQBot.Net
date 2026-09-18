---
uid: Guides.QuickReference.Bot.Event.C2CMsgReject
title: 单聊消息接收关闭
---

# 单聊消息接收关闭

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [单聊消息接收关闭]

当用户关闭接收机器人的单聊主动消息时引发。

```csharp
_client.UserActiveMessageRejected += (channel) => Task.CompletedTask;
// channel 是拒绝当前用户主动消息的用户频道（SocketUserChannel）
```

[单聊消息接收关闭]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/c2c_msg_reject.html
