---
uid: Guides.QuickReference.Guild.Content.GetSchedule
title: 获取日程详情
---

# 获取日程详情

预声明变量

```csharp
IScheduleChannel scheduleChannel = null;
ulong scheduleId = default;
```

### [获取日程详情]

GET `/channels/{channel_id}/schedules/{schedule_id}`

```csharp
// API 请求
IGuildSchedule schedule = await scheduleChannel.GetScheduleAsync(scheduleId);
```

[获取日程详情]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/schedule/get_schedule.html
