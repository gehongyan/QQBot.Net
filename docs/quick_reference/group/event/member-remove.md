---
uid: Guides.QuickReference.Group.Event.MemberRemove
title: 群成员退出
---

# 群成员退出

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [群成员退出]

当有成员退出群聊时引发。

```csharp
_client.GroupMemberLeft += async (channel, member, operatorUser) =>
{
    // channel 是成员退出的群组（SocketGroupChannel）
    // member 是退出的成员，为可延迟加载对象 Cacheable<SocketUser, string>
    // operatorUser 是操作者用户（可延迟加载，可为 null）
    string memberId = member.Id; // 始终可用的用户 ID
    // QQ 群用户目前无法通过 API 获取，GetOrDownloadAsync 在缓存未命中时返回 null
    SocketUser user = await member.GetOrDownloadAsync();
};
```

[群成员退出]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_member_remove.html
