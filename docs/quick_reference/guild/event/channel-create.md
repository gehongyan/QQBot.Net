---
uid: Guides.QuickReference.Guild.Event.ChannelCreate
title: 子频道创建
---

# 子频道创建

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [子频道创建]

当频道内创建了新的子频道时引发。

```csharp
_client.ChannelCreated += (channel) => Task.CompletedTask;
// channel 是新创建的子频道（SocketGuildChannel）
```

[子频道创建]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/channel_create.html
