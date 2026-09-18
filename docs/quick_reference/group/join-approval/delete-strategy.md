---
uid: Guides.QuickReference.Group.JoinApproval.DeleteStrategy
title: 删除入群自动审批策略
---

# 删除入群自动审批策略

预声明变量

```csharp
IGroupJoinApprovalStrategy strategy = null;
```

### [删除入群自动审批策略]

DELETE `/v2/groups/join_approval_strategy/{strategy_id}`

```csharp
// API 请求
await strategy.DeleteAsync();
```

[删除入群自动审批策略]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_join_approval_strategy_strategy_id.delete.html
