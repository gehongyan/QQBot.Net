---
uid: Guides.QuickReference.Group.JoinApproval.ExecuteStrategy
title: 执行入群自动审批策略
---

# 执行入群自动审批策略

预声明变量

```csharp
IGroupJoinApprovalStrategy strategy = null;
```

### [执行入群自动审批策略]

POST `/v2/groups/join_approval_strategy/{strategy_id}/execute`

对策略关联的全部群发起全量扫描，命中白名单号码的入群申请将被自动审批通过。此操作异步执行，约 10 分钟完成。

```csharp
// API 请求
await strategy.ExecuteAsync();
```

[执行入群自动审批策略]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_join_approval_strategy_strategy_id_execute.post.html
