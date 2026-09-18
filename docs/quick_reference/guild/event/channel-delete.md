---
uid: Guides.QuickReference.Guild.Event.ChannelDelete
title: 子频道删除
---

# 子频道删除

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [子频道删除]

当子频道被删除时引发。

```csharp
_client.ChannelDestroyed += async (channel, operatorMember) =>
{
    // channel 是被删除的子频道（SocketGuildChannel）
    // operatorMember 是操作者成员，为可延迟加载对象 Cacheable<SocketGuildMember, ulong>
    SocketGuildMember op = await operatorMember.GetOrDownloadAsync();
};
```

[子频道删除]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/channel_delete.html
