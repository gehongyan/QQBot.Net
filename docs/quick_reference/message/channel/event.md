---
uid: Guides.QuickReference.Message.Channel.Event
title: 频道消息事件
---

# 频道消息事件

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [接收子频道消息]

当文字子频道中产生消息时引发。文字子频道的频道类型为 <xref:QQBot.WebSocket.SocketTextChannel>。

```csharp
_client.MessageReceived += async message =>
{
    if (message.Channel is not SocketTextChannel textChannel) return;
    // message 是收到的子频道消息
    await textChannel.SendMessageAsync("收到", passiveSource: message);
};
```

### [表情表态事件]

当子频道消息被添加或移除表情表态时引发。

```csharp
_client.ReactionAdded += (reaction) => Task.CompletedTask;
_client.ReactionRemoved += (reaction) => Task.CompletedTask;
```

[接收子频道消息]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/event.html
[表情表态事件]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/event.html
