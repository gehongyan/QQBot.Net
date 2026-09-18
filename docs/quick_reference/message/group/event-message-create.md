---
uid: Guides.QuickReference.Message.Group.Event.MessageCreate
title: 群消息（全量模式）事件
---

# 群消息（全量模式）事件

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [群消息（全量模式）事件]

当群聊中产生消息时引发。全量模式需要机器人具备相应的网关意图（在
<xref:QQBot.WebSocket.QQBotSocketConfig.GatewayIntents> 中配置）。群聊消息的频道类型为
<xref:QQBot.WebSocket.SocketGroupChannel>。

```csharp
_client.MessageReceived += async message =>
{
    if (message.Channel is not SocketGroupChannel groupChannel) return;
    // message 是收到的群聊消息
    await groupChannel.SendMessageAsync("收到", passiveSource: message);
};
```

[群消息（全量模式）事件]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_message_create.html
