---
uid: Guides.QuickReference.Group.Member.GetBlacklist
title: 群黑名单查询
---

# 群黑名单查询

预声明变量

```csharp
IGroupChannel groupChannel = null;
```

### [群黑名单查询]

GET `/v2/groups/{group_openid}/member_blacklist`

```csharp
// API 请求，以分页形式获取群黑名单中的用户
IAsyncEnumerable<IReadOnlyCollection<GroupBlacklistUser>> blacklist =
    groupChannel.GetBlacklistAsync();
```

[群黑名单查询]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_member_blacklist.get.html
