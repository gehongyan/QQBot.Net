---
uid: Guides.QuickReference.Message.Group.Event.AtMessageCreate
title: 群@机器人消息事件
---

# 群@机器人消息事件

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [群@机器人消息事件]

当群聊中有消息 @ 了机器人时引发。这是公域机器人默认可接收的群聊消息事件，频道类型为
<xref:QQBot.WebSocket.SocketGroupChannel>。

```csharp
_client.MessageReceived += async message =>
{
    if (message.Channel is not SocketGroupChannel groupChannel) return;
    // message 是 @ 机器人的群聊消息
    await groupChannel.SendMessageAsync("收到", passiveSource: message);
};
```

[群@机器人消息事件]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_at_message_create.html
