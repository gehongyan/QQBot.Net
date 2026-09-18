---
uid: Guides.QuickReference.Bot.GenerateShareLink
title: 生成分享链接
---

# 生成分享链接

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;
```

### [生成分享链接]

POST `/v2/generate_url_link`

生成引导用户添加机器人的分享链接。可选的回调数据会在用户通过该链接添加机器人后，
经 <xref:QQBot.WebSocket.BaseSocketClient.UserAdded> 事件的第三个参数回传。

```csharp
string callbackData = null; // 可选的开发者自定义回调数据

// API 请求
Uri profileUrl = await _socketClient.GenerateProfileUrlAsync(callbackData);
```

[生成分享链接]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_generate_url_link.post.html
