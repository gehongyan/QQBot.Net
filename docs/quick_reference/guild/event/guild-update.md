---
uid: Guides.QuickReference.Guild.Event.GuildUpdate
title: 频道更新
---

# 频道更新

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [频道更新]

当频道的信息被更新时引发。

```csharp
_client.GuildUpdated += async (before, after, operatorMember) =>
{
    // before 是更新前的频道，after 是更新后的频道
    // operatorMember 是操作者成员，为可延迟加载对象 Cacheable<SocketGuildMember, ulong>
    if (operatorMember.Value is { } cached)
    {
        // 缓存中已有实体，直接使用
    }
    // 或尝试从缓存获取，未命中时通过 API 下载
    SocketGuildMember op = await operatorMember.GetOrDownloadAsync();
    ulong operatorId = operatorMember.Id; // 始终可用的标识符
};
```

[频道更新]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/guild_update.html
