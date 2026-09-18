---
uid: Guides.QuickReference.Guild.Content.ModifySchedule
title: 修改日程
---

# 修改日程

预声明变量

```csharp
IGuildSchedule schedule = null;
```

### [修改日程]

PATCH `/channels/{channel_id}/schedules/{schedule_id}`

```csharp
// API 请求
await schedule.ModifyAsync(props =>
{
    // 要修改的日程属性（名称、时间、描述、提醒类型等）
});
```

[修改日程]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/schedule/patch_schedule.html
