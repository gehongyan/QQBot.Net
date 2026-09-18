---
uid: Guides.QuickReference.Guild.Content.CreateThread
title: 发表帖子
---

# 发表帖子

预声明变量

```csharp
IForumChannel forumChannel = null;
```

### [发表帖子]

PUT `/channels/{channel_id}/threads`

```csharp
string title = null;                 // 帖子标题
ThreadTextType textType = default;   // 帖子内容的文本类型
string content = null;               // 帖子内容

// API 请求，按文本类型与内容发表
await forumChannel.CreateThreadAsync(title, textType, content);

// 或使用富文本构建器发表
RichTextBuilder richContent = new RichTextBuilder();
await forumChannel.CreateThreadAsync(title, richContent);
```

[发表帖子]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/forum/put_thread.html
