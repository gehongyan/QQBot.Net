---
uid: Guides.QuickReference.Message.C2C.RecallMessage
title: 撤回单聊消息
---

# 撤回单聊消息

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;

IUserMessage userMessage = null;
```

### [撤回单聊消息]

DELETE `/v2/users/{openid}/messages/{message_id}`

```csharp
// API 请求，撤回消息
await userMessage.DeleteAsync();
```

[撤回单聊消息]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_users_user_openid_messages_message_id.delete.html
