---
uid: Guides.QuickReference.Group.GetBotState
title: 获取机器人群内状态
---

# 获取机器人群内状态

预声明变量

```csharp
IGroupChannel groupChannel = null;
```

### [获取机器人群内状态]

GET `/v2/groups/{group_openid}/bot_state`

```csharp
// API 请求
GroupBotState state = await groupChannel.GetBotStateAsync();
```

[获取机器人群内状态]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_bot_state.get.html
