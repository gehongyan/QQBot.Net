---
uid: Guides.QuickReference.Group.JoinApproval.GetStrategies
title: 查询入群自动审批策略列表
---

# 查询入群自动审批策略列表

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
```

### [查询入群自动审批策略列表]

GET `/v2/groups/join_approval_strategy`

```csharp
// API 请求，以分页形式获取入群自动审批策略
IAsyncEnumerable<IReadOnlyCollection<IGroupJoinApprovalStrategy>> strategies =
    _socketClient.GetJoinApprovalStrategiesAsync();
```

[查询入群自动审批策略列表]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_join_approval_strategy.get.html
