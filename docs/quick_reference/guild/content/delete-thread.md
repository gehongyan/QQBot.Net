---
uid: Guides.QuickReference.Guild.Content.DeleteThread
title: 删除帖子
---

# 删除帖子

预声明变量

```csharp
IThread thread = null;
```

### [删除帖子]

DELETE `/channels/{channel_id}/threads/{thread_id}`

```csharp
// API 请求
await thread.DeleteAsync();
```

[删除帖子]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/forum/delete_thread.html
