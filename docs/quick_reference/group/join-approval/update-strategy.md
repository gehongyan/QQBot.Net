---
uid: Guides.QuickReference.Group.JoinApproval.UpdateStrategy
title: 修改入群自动审批策略
---

# 修改入群自动审批策略

预声明变量

```csharp
IGroupJoinApprovalStrategy strategy = null;
```

### [修改入群自动审批策略]

PATCH `/v2/groups/join_approval_strategy/{strategy_id}`

```csharp
// API 请求
await strategy.ModifyAsync(props =>
{
    // 要修改的策略属性（关联群的增删、过期时间、备注等）
});
```

[修改入群自动审批策略]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_join_approval_strategy_strategy_id.patch.html
