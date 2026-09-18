---
uid: Guides.QuickReference.Guild.Content.AddPin
title: 添加精华消息
---

# 添加精华消息

预声明变量

```csharp
ITextChannel textChannel = null;
IUserMessage message = null;
```

### [添加精华消息]

PUT `/channels/{channel_id}/pins/{message_id}`

```csharp
// API 请求，通过消息实体添加
await textChannel.PinMessageAsync(message);
// 或通过消息 ID 添加
await textChannel.PinMessageAsync(messageId: "message_id");
```

[添加精华消息]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/pins/put_pins_message.html
