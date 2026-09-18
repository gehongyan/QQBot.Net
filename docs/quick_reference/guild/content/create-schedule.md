---
uid: Guides.QuickReference.Guild.Content.CreateSchedule
title: 创建日程
---

# 创建日程

预声明变量

```csharp
IScheduleChannel scheduleChannel = null;
```

### [创建日程]

POST `/channels/{channel_id}/schedules`

```csharp
string name = null;                     // 日程名称
DateTimeOffset startTime = default;     // 日程开始时间
DateTimeOffset endTime = default;       // 日程结束时间
string description = null;              // 日程描述
IGuildChannel jumpChannel = null;       // 日程开始时要跳转到的子频道
RemindType remindType = RemindType.None; // 日程提醒类型

// API 请求
IGuildSchedule schedule = await scheduleChannel.CreateScheduleAsync(
    name, startTime, endTime, description, jumpChannel, remindType);
```

[创建日程]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/schedule/post_schedule.html
