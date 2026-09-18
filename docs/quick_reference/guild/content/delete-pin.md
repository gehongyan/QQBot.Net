---
uid: Guides.QuickReference.Guild.Content.DeletePin
title: 删除精华消息
---

# 删除精华消息

预声明变量

```csharp
ITextChannel textChannel = null;
IUserMessage message = null;
```

### [删除精华消息]

DELETE `/channels/{channel_id}/pins/{message_id}`

```csharp
// API 请求，通过消息实体取消精华
await textChannel.UnpinMessageAsync(message);
// 或通过消息 ID 取消
await textChannel.UnpinMessageAsync(messageId: "message_id");
```

[删除精华消息]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/pins/delete_pins_message.html
