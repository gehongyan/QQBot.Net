---
uid: Guides.QuickReference.Guild.Speak.MuteMember
title: 频道指定成员禁言
---

# 频道指定成员禁言

预声明变量

```csharp
IGuild guild = null;
IGuildMember member = null;
ulong userId = default;
```

### [频道指定成员禁言]

PATCH `/guilds/{guild_id}/members/{user_id}/mute`

```csharp
TimeSpan duration = default; // 禁言时长

// API 请求，按成员实体禁言
await guild.MuteMemberAsync(member, duration);
// 或按成员 ID 禁言
await guild.MuteMemberAsync(userId, duration);
```

[频道指定成员禁言]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/speak/patch_guild_member_mute.html
