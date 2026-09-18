---
uid: Guides.QuickReference.Guild.Member.Event.GuildMember
title: 频道成员事件
---

# 频道成员事件

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [频道成员加入]

```csharp
_client.UserJoined += (member, operatorMember) => Task.CompletedTask;
// member 是加入的频道成员
// operatorMember 是操作者成员的可延迟加载对象
```

### [频道成员更新]

```csharp
_client.GuildMemberUpdated += async (before, after, operatorMember) =>
{
    // before 是更新前的成员，为可延迟加载对象 Cacheable<SocketGuildMember, ulong>
    // after 是更新后的成员（SocketGuildMember）
    // operatorMember 是操作者成员，为可延迟加载对象
    SocketGuildMember previous = await before.GetOrDownloadAsync();
    SocketGuildMember op = await operatorMember.GetOrDownloadAsync();
};
```

### [频道成员退出]

```csharp
_client.UserLeft += (guild, user, operatorMember) => Task.CompletedTask;
// guild 是成员退出的频道
// user 是退出的成员
// operatorMember 是操作者成员的可延迟加载对象
```

[频道成员加入]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/guild_member.html
[频道成员更新]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/guild_member.html
[频道成员退出]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/guild_member.html
