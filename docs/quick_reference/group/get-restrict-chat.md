---
uid: Guides.QuickReference.Group.GetRestrictChat
title: 查询群禁言状态
---

# 查询群禁言状态

预声明变量

```csharp
IGroupChannel groupChannel = null;
```

### [查询群禁言状态]

GET `/v2/groups/{group_openid}/restrict_chat_setting`

返回全员禁言规则以及当前处于禁言状态的成员列表。机器人需拥有群管理员身份。

```csharp
// API 请求
GroupMuteSetting setting = await groupChannel.GetMuteSettingAsync();
```

[查询群禁言状态]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_restrict_chat_setting.get.html
