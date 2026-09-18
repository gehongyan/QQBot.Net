---
uid: Guides.QuickReference.Group.JoinApproval.UpdateWhitelist
title: 修改入群自动审批策略的白名单号码
---

# 修改入群自动审批策略的白名单号码

预声明变量

```csharp
IGroupJoinApprovalStrategy strategy = null;
IEnumerable<string> qqNumbers = null;
```

### [修改入群自动审批策略的白名单号码]

POST `/v2/groups/join_approval_strategy/{strategy_id}/whitelist_users`

单次最多操作 10000 个号码，号码总数上限为 10 万。

```csharp
// 将号码加入白名单
await strategy.AddWhitelistAsync(qqNumbers);
// 将号码移出白名单
await strategy.RemoveWhitelistAsync(qqNumbers);
```

[修改入群自动审批策略的白名单号码]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_join_approval_strategy_strategy_id_whitelist_users.post.html
