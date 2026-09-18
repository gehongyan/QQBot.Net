---
uid: Guides.QuickReference.Guild.Event.GuildDelete
title: 频道解散
---

# 频道解散

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [频道解散]

当机器人被移出频道、或频道被解散时引发。

```csharp
_client.LeftGuild += async (guild, operatorMember) =>
{
    // guild 是机器人离开的频道
    // operatorMember 是操作者成员，为可延迟加载对象 Cacheable<SocketGuildMember, ulong>
    ulong operatorId = operatorMember.Id;
    SocketGuildMember op = await operatorMember.GetOrDownloadAsync();
};
```

[频道解散]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/guild_delete.html
