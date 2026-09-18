---
uid: Guides.QuickReference.Group.Event.MemberAdd
title: 群成员加入
---

# 群成员加入

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [群成员加入]

当有成员加入群聊时引发。

```csharp
_client.GroupMemberJoined += async (channel, member, operatorUser) =>
{
    // channel 是成员加入的群组（SocketGroupChannel）
    // member 是加入的成员，为可延迟加载对象 Cacheable<SocketUser, string>
    // operatorUser 是操作者用户（可延迟加载，可为 null）
    string memberId = member.Id; // 始终可用的用户 ID
    SocketUser user = member.Value; // 缓存中的实体，可能为 null
    // 注意：QQ 群用户目前无法通过 API 获取，GetOrDownloadAsync 在缓存未命中时返回 null
    SocketUser downloaded = await member.GetOrDownloadAsync();
};
```

[群成员加入]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/event/group_member_add.html
