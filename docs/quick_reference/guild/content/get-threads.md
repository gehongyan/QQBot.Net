---
uid: Guides.QuickReference.Guild.Content.GetThreads
title: 获取帖子列表
---

# 获取帖子列表

预声明变量

```csharp
IForumChannel forumChannel = null;
```

### [获取帖子列表]

GET `/channels/{channel_id}/threads`

```csharp
// API 请求
IReadOnlyCollection<IThread> threads = await forumChannel.GetThreadsAsync();
```

[获取帖子列表]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/forum/get_threads_list.html
