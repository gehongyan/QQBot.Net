---
uid: Guides.QuickReference.Guild.Event.GuildCreate
title: 频道创建
---

# 频道创建

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [频道创建]

当机器人加入频道（QQ 频道服务器）时引发。

```csharp
_client.JoinedGuild += async (guild, operatorMember) =>
{
    // guild 是机器人加入的频道
    // operatorMember 是操作者成员，为可延迟加载对象 Cacheable<SocketGuildMember, ulong>
    ulong operatorId = operatorMember.Id;              // 始终可用的标识符
    SocketGuildMember op = await operatorMember.GetOrDownloadAsync(); // 缓存未命中时通过 API 下载
};
```

[频道创建]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/guild_create.html
