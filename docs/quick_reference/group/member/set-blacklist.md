---
uid: Guides.QuickReference.Group.Member.SetBlacklist
title: 群黑名单操作
---

# 群黑名单操作

预声明变量

```csharp
IGroupChannel groupChannel = null;
IEnumerable<Guid> memberIds = null;
```

### [群黑名单操作]

POST `/v2/groups/{group_openid}/member_blacklist`

单次最多操作 20 个成员。加入黑名单仅当目标成员不在群中时才可操作。

```csharp
// 加入黑名单（按成员标识符集合 / 按成员实体集合）
IReadOnlyCollection<Guid> failedAdd = await groupChannel.AddToBlacklistAsync(memberIds);
IEnumerable<IGroupMember> members = null;
IReadOnlyCollection<Guid> failedAdd2 = await groupChannel.AddToBlacklistAsync(members);

// 移出黑名单（按成员标识符集合 / 按成员实体集合）
IReadOnlyCollection<Guid> failedRemove = await groupChannel.RemoveFromBlacklistAsync(memberIds);
IReadOnlyCollection<Guid> failedRemove2 = await groupChannel.RemoveFromBlacklistAsync(members);
// 返回集合为操作失败的成员标识符；为空表示全部成功
```

[群黑名单操作]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_member_blacklist.post.html
