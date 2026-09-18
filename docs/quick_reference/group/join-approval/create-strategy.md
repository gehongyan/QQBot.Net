---
uid: Guides.QuickReference.Group.JoinApproval.CreateStrategy
title: 创建入群自动审批策略
---

# 创建入群自动审批策略

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
IEnumerable<Guid> groupIds = null;
```

### [创建入群自动审批策略]

POST `/v2/groups/join_approval_strategy`

```csharp
// API 请求
IGroupJoinApprovalStrategy strategy = await _socketClient.CreateJoinApprovalStrategyAsync(
    groupIds, func: props =>
    {
        // 策略属性配置（过期时间、备注等）
    });
```

[创建入群自动审批策略]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_join_approval_strategy.post.html
