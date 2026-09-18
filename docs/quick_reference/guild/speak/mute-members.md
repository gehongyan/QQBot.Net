---
uid: Guides.QuickReference.Guild.Speak.MuteMembers
title: 频道批量成员禁言
---

# 频道批量成员禁言

预声明变量

```csharp
IGuild guild = null;
IEnumerable<ulong> userIds = null;
IEnumerable<IGuildMember> members = null;
```

### [频道批量成员禁言]

PATCH `/guilds/{guild_id}/mute`

```csharp
TimeSpan duration = default;    // 禁言时长
DateTimeOffset until = default; // 或禁言至某时刻

// API 请求，4 种重载：成员实体/成员 ID × 时长/截止时间
await guild.MuteMembersAsync(members, duration);
await guild.MuteMembersAsync(userIds, duration);
await guild.MuteMembersAsync(members, until);
await guild.MuteMembersAsync(userIds, until);
```

[频道批量成员禁言]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/speak/patch_guild_mute_multi_member.html
