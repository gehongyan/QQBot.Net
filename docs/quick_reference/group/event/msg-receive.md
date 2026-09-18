---
uid: Guides.QuickReference.Group.Event.MsgReceive
title: 群聊消息接收开启
---

# 群聊消息接收开启

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [群聊消息接收开启]

当群聊开启接收机器人的主动消息时引发。

```csharp
_client.GroupActiveMessageAllowed += async (channel, user) =>
{
    // channel 是接受主动消息的群组（SocketGroupChannel）
    // user 是操作者用户，为可延迟加载对象 Cacheable<SocketUser, string>
    string userId = user.Id;
    SocketUser resolved = await user.GetOrDownloadAsync(); // 群用户无 API 获取，缓存未命中返回 null
};
```

[群聊消息接收开启]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_msg_receive.html
