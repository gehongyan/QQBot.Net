---
uid: Guides.QuickReference.Group.Event.MsgReject
title: 群聊消息接收关闭
---

# 群聊消息接收关闭

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [群聊消息接收关闭]

当群聊关闭接收机器人的主动消息时引发。

```csharp
_client.GroupActiveMessageRejected += async (channel, user) =>
{
    // channel 是拒绝主动消息的群组（SocketGroupChannel）
    // user 是操作者用户，为可延迟加载对象 Cacheable<SocketUser, string>
    string userId = user.Id;
    SocketUser resolved = await user.GetOrDownloadAsync(); // 群用户无 API 获取，缓存未命中返回 null
};
```

[群聊消息接收关闭]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_msg_reject.html
