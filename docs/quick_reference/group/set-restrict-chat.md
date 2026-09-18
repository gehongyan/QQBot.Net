---
uid: Guides.QuickReference.Group.SetRestrictChat
title: 设置群成员禁言
---

# 设置群成员禁言

预声明变量

```csharp
IGroupChannel groupChannel = null;
Guid memberId = default;
```

### [设置群成员禁言]

POST `/v2/groups/{group_openid}/restrict_chat_setting`

机器人需拥有群管理员身份，最大禁言时长为 30 天，且仅可禁言普通成员。

```csharp
DateTimeOffset expiresAt = default; // 禁言到期时间
TimeSpan duration = default;         // 或自当前时间起的禁言时长

// API 请求，按到期时间禁言
await groupChannel.MuteMemberAsync(memberId, expiresAt);
// 或按时长禁言
await groupChannel.MuteMemberAsync(memberId, duration);
// 解除禁言
await groupChannel.UnmuteMemberAsync(memberId);
```

[设置群成员禁言]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_restrict_chat_setting.post.html
