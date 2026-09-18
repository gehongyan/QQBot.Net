---
uid: Guides.QuickReference.Group.GetInfo
title: 获取群基本信息
---

# 获取群基本信息

预声明变量

```csharp
IGroupChannel groupChannel = null;
```

### [获取群基本信息]

GET `/v2/groups/{group_openid}/info`

```csharp
// API 请求
GroupInfo info = await groupChannel.GetInfoAsync();
```

[获取群基本信息]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_info.get.html
