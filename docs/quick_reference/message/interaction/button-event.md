---
uid: Guides.QuickReference.Message.Interaction.Button.Event
title: 互动事件
---

# 互动事件

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [互动事件]

当用户点击消息按钮等互动组件时引发。事件参数为 <xref:QQBot.WebSocket.SocketInteraction>。

```csharp
_client.InteractionCreated += async interaction =>
{
    // interaction 是收到的互动
    // 可调用 RespondAsync 回应此互动
    await interaction.RespondAsync("已收到你的操作");
};
```

[互动事件]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/interaction_create.html
