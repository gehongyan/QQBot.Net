---
uid: Guides.QuickReference.Guild.Content.DeleteSchedule
title: 删除日程
---

# 删除日程

预声明变量

```csharp
IGuildSchedule schedule = null;
```

### [删除日程]

DELETE `/channels/{channel_id}/schedules/{schedule_id}`

```csharp
// API 请求
await schedule.DeleteAsync();
```

[删除日程]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/schedule/delete_schedule.html
