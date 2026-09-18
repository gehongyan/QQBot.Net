---
uid: Guides.QuickReference.Message.C2C.Event.MessageCreate
title: 单聊消息事件
---

# 单聊消息事件

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [单聊消息事件]

当用户在单聊中向机器人发送消息时引发。可通过消息的 <xref:QQBot.WebSocket.SocketMessage.Channel> 判断来源，
单聊消息的频道类型为 <xref:QQBot.WebSocket.SocketUserChannel>。

```csharp
_client.MessageReceived += async message =>
{
    if (message.Channel is not SocketUserChannel userChannel) return;
    // message 是收到的消息
    // 可通过 message.Channel 回复
    await userChannel.SendMessageAsync("收到", passiveSource: message);
};
```

[单聊消息事件]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/c2c_message_create.html
