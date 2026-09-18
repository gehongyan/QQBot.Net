---
uid: Guides.QuickReference.Group.Member.BatchRemove
title: 群成员批量移除
---

# 群成员批量移除

预声明变量

```csharp
IGroupChannel groupChannel = null;
IEnumerable<Guid> memberIds = null;
```

### [群成员批量移除]

POST `/v2/groups/{group_openid}/batch_remove_members`

单次最多移除 20 个成员。机器人需拥有群管理员身份。

```csharp
bool addToBlacklist = false; // 是否在移除的同时加入群黑名单

// API 请求，按成员标识符集合移除
GroupRemoveMembersResult result = await groupChannel.RemoveMembersAsync(memberIds, addToBlacklist);
// 或按成员实体集合移除
IEnumerable<IGroupMember> members = null;
GroupRemoveMembersResult result2 = await groupChannel.RemoveMembersAsync(members, addToBlacklist);
```

[群成员批量移除]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_batch_remove_members.post.html
