---
uid: Guides.QuickReference.Guild.Content.GetSchedules
title: 获取频道日程列表
---

# 获取频道日程列表

预声明变量

```csharp
IScheduleChannel scheduleChannel = null;
```

### [获取频道日程列表]

GET `/channels/{channel_id}/schedules`

```csharp
DateTimeOffset? since = null; // 不为 null 时获取结束时间在此之后的日程；为 null 时获取当天所有日程

// API 请求
IReadOnlyCollection<IGuildSchedule> schedules = await scheduleChannel.GetSchedulesAsync(since);
```

[获取频道日程列表]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/schedule/get_schedules.html
