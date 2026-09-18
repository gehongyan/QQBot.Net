---
uid: Guides.QuickReference.Group.Member.GetMember
title: 获取群成员信息
---

# 获取群成员信息

预声明变量

```csharp
IGroupChannel groupChannel = null;
Guid memberId = default;
```

### [获取群成员信息]

GET `/v2/groups/{group_openid}/members/{member_openid}`

```csharp
// API 请求
IGroupMember member = await groupChannel.GetMemberAsync(memberId);
```

[获取群成员信息]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_members_member_openid.get.html
