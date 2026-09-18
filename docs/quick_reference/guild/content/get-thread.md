---
uid: Guides.QuickReference.Guild.Content.GetThread
title: 获取帖子详情
---

# 获取帖子详情

预声明变量

```csharp
IForumChannel forumChannel = null;
string threadId = null;
```

### [获取帖子详情]

GET `/channels/{channel_id}/threads/{thread_id}`

```csharp
// API 请求
IThread thread = await forumChannel.GetThreadAsync(threadId);
```

[获取帖子详情]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/forum/get_thread.html
