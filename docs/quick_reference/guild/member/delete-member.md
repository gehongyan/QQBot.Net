---
uid: Guides.QuickReference.Guild.Member.DeleteMember
title: 删除频道成员
---

# 删除频道成员

预声明变量

```csharp
IGuildMember member = null;
```

### [删除频道成员]

DELETE `/guilds/{guild_id}/members/{user_id}`

```csharp
bool addBlacklist = false; // 是否同时加入黑名单
int pruneDays = 0;         // 删除消息的天数

// API 请求
await member.KickAsync(addBlacklist, pruneDays);
```

[删除频道成员]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/member/delete_member.html
