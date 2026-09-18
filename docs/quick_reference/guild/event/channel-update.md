---
uid: Guides.QuickReference.Guild.Event.ChannelUpdate
title: 子频道更新
---

# 子频道更新

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [子频道更新]

当子频道的信息被更新时引发。

```csharp
_client.ChannelUpdated += async (before, after, operatorMember) =>
{
    // before 是更新前的子频道（SocketGuildChannel）
    // after 是更新后的子频道（SocketGuildChannel）
    // operatorMember 是操作者成员，为可延迟加载对象 Cacheable<SocketGuildMember, ulong>
    SocketGuildMember op = await operatorMember.GetOrDownloadAsync(); // 缓存未命中时通过 API 下载
};
```

[子频道更新]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/channel_update.html
