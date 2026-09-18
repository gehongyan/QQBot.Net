---
uid: Guides.QuickReference.Message.Group.RecallMessage
title: 撤回群聊消息
---

# 撤回群聊消息

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;

IGroupChannel groupChannel = null;
IUserMessage userMessage = null;
```

### [撤回群聊消息]

DELETE `/v2/groups/{group_openid}/messages/{message_id}`

```csharp
string messageId = null; // 要撤回的消息 ID

// API 请求，通过消息实体撤回
await userMessage.DeleteAsync();

// 或通过群组子频道撤回
await groupChannel.DeleteMessageAsync(messageId);
await groupChannel.DeleteMessageAsync(userMessage);
```

[撤回群聊消息]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_messages_message_id.delete.html
