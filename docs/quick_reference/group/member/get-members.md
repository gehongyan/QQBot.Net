---
uid: Guides.QuickReference.Group.Member.GetMembers
title: 获取群成员列表
---

# 获取群成员列表

预声明变量

```csharp
IGroupChannel groupChannel = null;
```

### [获取群成员列表]

GET `/v2/groups/{group_openid}/members`

```csharp
// API 请求，以分页形式获取群内所有成员
IAsyncEnumerable<IReadOnlyCollection<IGroupMember>> members = groupChannel.GetMembersAsync();
```

[获取群成员列表]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_members.get.html
