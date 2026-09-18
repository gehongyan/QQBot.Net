---
uid: Guides.QuickReference.Guild.DeleteChannel
title: 删除子频道
---

# 删除子频道

预声明变量

```csharp
IGuildChannel channel = null;
```

### [删除子频道]

DELETE `/channels/{channel_id}`

```csharp
// API 请求
await channel.DeleteAsync();
```

[删除子频道]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/channels_channel_id.delete.html
