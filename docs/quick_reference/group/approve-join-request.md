---
uid: Guides.QuickReference.Group.ApproveJoinRequest
title: 入群申请审批
---

# 入群申请审批

预声明变量

```csharp
IGroupChannel groupChannel = null;
GroupJoinRequest request = null;
Guid memberId = default;
```

### [入群申请审批]

POST `/v2/groups/{group_openid}/approval_join_request/{member_openid}`

```csharp
// 通过入群申请（两种方式）
await groupChannel.ApproveJoinRequestAsync(request);                 // 传入申请实体
await groupChannel.ApproveJoinRequestAsync(memberId, joinRequestId: null); // 按申请人 ID
await request.ApproveAsync();                                        // 富实体实例方法

// 拒绝入群申请（可附理由、可选择同时加入黑名单；两种方式）
await groupChannel.DeclineJoinRequestAsync(request, reason: "不符合条件", addToBlacklist: false);
await groupChannel.DeclineJoinRequestAsync(memberId, joinRequestId: null,
    reason: "不符合条件", addToBlacklist: false);
await request.DeclineAsync();                                        // 富实体实例方法
```

[入群申请审批]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_approval_join_request_member_openid.post.html
