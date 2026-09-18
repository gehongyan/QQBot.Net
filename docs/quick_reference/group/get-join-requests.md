---
uid: Guides.QuickReference.Group.GetJoinRequests
title: 入群申请列表拉取
---

# 入群申请列表拉取

预声明变量

```csharp
IGroupChannel groupChannel = null;
```

### [入群申请列表拉取]

GET `/v2/groups/{group_openid}/join_request_list`

机器人需拥有群管理员身份。

```csharp
// API 请求，以分页形式获取入群申请
IAsyncEnumerable<IReadOnlyCollection<GroupJoinRequest>> requests =
    groupChannel.GetJoinRequestsAsync();
```

[入群申请列表拉取]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_join_request_list.get.html
