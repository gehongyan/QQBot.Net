---
uid: Guides.QuickReference.Guild.ModifyChannel
title: 修改子频道
---

# 修改子频道

预声明变量

```csharp
ITextChannel textChannel = null;
```

### [修改子频道]

PATCH `/channels/{channel_id}`

```csharp
// API 请求，修改文字子频道的属性
await textChannel.ModifyAsync(props =>
{
    // 要修改的属性配置
});
```

[修改子频道]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/channels_channel_id.patch.html
