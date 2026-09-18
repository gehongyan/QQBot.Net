---
uid: Guides.QuickReference.Guild.Speak.MuteAll
title: 频道全员禁言
---

# 频道全员禁言

预声明变量

```csharp
IGuild guild = null;
```

### [频道全员禁言]

PATCH `/guilds/{guild_id}/mute`

```csharp
TimeSpan duration = default;   // 禁言时长
DateTimeOffset until = default; // 或禁言至某时刻

// API 请求，按时长禁言全员
await guild.MuteEveryoneAsync(duration);
// 或按截止时间
await guild.MuteEveryoneAsync(until);
// 解除全员禁言
await guild.UnmuteEveryoneAsync();
```

[频道全员禁言]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/speak/patch_guild_mute.html
