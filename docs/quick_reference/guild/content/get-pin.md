---
uid: Guides.QuickReference.Guild.Content.GetPin
title: 获取精华消息
---

# 获取精华消息

预声明变量

```csharp
ITextChannel textChannel = null;
```

### [获取精华消息]

GET `/channels/{channel_id}/pins`

```csharp
// API 请求，获取此子频道所有精华消息的 ID
IReadOnlyCollection<ulong> pinnedMessageIds = await textChannel.GetPinnedMessagesAsync();
```

[获取精华消息]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/pins/get_pins_message.html
