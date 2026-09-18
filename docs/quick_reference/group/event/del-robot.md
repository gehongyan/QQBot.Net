---
uid: Guides.QuickReference.Group.Event.DelRobot
title: 机器人退出群聊
---

# 机器人退出群聊

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [机器人退出群聊]

当机器人被移出群聊时引发。

```csharp
_client.LeftGroup += async (channel, user) =>
{
    // channel 是移出机器人的群组（SocketGroupChannel）
    // user 是操作者用户，为可延迟加载对象 Cacheable<SocketUser, string>
    string userId = user.Id;
    SocketUser resolved = await user.GetOrDownloadAsync(); // 群用户无 API 获取，缓存未命中返回 null
};
```

[机器人退出群聊]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_del_robot.html
