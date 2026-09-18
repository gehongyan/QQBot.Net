---
uid: Guides.QuickReference.Group.Event.JoinRequest
title: 用户申请加群事件
---

# 用户申请加群事件

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [用户申请加群事件]

当用户申请加入群聊时引发。

```csharp
_client.GroupJoinRequested += async (channel, request, autoApprovedStrategyId) =>
{
    // channel 是收到入群申请的群组
    // request 是入群申请，可直接调用其实例方法进行审批
    // autoApprovedStrategyId 是自动审批通过此申请的策略标识符，仅在已被自动审批时有值

    // 通过入群申请
    await request.ApproveAsync();
    // 拒绝入群申请
    await request.DeclineAsync();
};
```

[用户申请加群事件]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_join_request.html
