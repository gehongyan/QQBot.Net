---
uid: Guides.QuickReference.Group.Event.AddRobot
title: 机器人加入群聊
---

# 机器人加入群聊

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [机器人加入群聊]

当机器人被添加到群聊时引发。

```csharp
_client.JoinedGroup += async (channel, user) =>
{
    // channel 是添加机器人的群组（SocketGroupChannel）
    // user 是操作者用户，为可延迟加载对象 Cacheable<SocketUser, string>
    string userId = user.Id; // 始终可用的用户 ID
    // QQ 群用户目前无法通过 API 获取，GetOrDownloadAsync 在缓存未命中时返回 null
    SocketUser resolved = await user.GetOrDownloadAsync();
};
```

[机器人加入群聊]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_add_robot.html
