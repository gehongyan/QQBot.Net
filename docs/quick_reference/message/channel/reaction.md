---
uid: Guides.QuickReference.Message.Channel.Reaction
title: 表情表态
---

# 表情表态

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;

IUserMessage userMessage = null;
```

有关如何构造表情（<xref:QQBot.IEmote>），请参考表情相关类型（如 `Emote` / `Emoji`）。

### [添加表情表态]

PUT `/channels/{channel_id}/messages/{message_id}/reactions/{type}/{id}`

```csharp
IEmote emote = null; // 要添加的表情

// API 请求
await userMessage.AddReactionAsync(emote);
```

### [删除表情表态]

DELETE `/channels/{channel_id}/messages/{message_id}/reactions/{type}/{id}`

```csharp
IEmote emote = null; // 要删除的表情

// API 请求
await userMessage.RemoveReactionAsync(emote);
```

### [获取表情表态用户列表]

GET `/channels/{channel_id}/messages/{message_id}/reactions/{type}/{id}`

```csharp
IEmote emote = null; // 要获取表态用户的表情

// API 请求，以分页形式返回
IAsyncEnumerable<IReadOnlyCollection<IGuildUser>> users = userMessage.GetReactionUsersAsync(emote);
```

[添加表情表态]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/trans/emoji.html
[删除表情表态]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/trans/emoji.html
[获取表情表态用户列表]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/trans/emoji.html
