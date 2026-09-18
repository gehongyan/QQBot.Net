---
uid: Guides.QuickReference.Message.Channel.RecallMessage
title: 撤回子频道消息
---

# 撤回子频道消息

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;

IUserMessage userMessage = null;
```

### [撤回子频道消息]

DELETE `/channels/{channel_id}/messages/{message_id}`

```csharp
// API 请求，撤回消息
await userMessage.DeleteAsync();
```

[撤回子频道消息]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/recall.html
