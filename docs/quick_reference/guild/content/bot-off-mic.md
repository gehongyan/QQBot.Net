---
uid: Guides.QuickReference.Guild.Content.BotOffMic
title: 机器人下麦
---

# 机器人下麦

预声明变量

```csharp
IVoiceChannel voiceChannel = null;
```

### [机器人下麦]

DELETE `/channels/{channel_id}/mic`

将机器人语音断开连接（下麦）此语音子频道。

```csharp
// API 请求
await voiceChannel.LeaveAsync();
```

[机器人下麦]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/audio/delete_mic.html
