---
uid: Guides.QuickReference.Guild.Content.BotOnMic
title: 机器人上麦
---

# 机器人上麦

预声明变量

```csharp
IVoiceChannel voiceChannel = null;
```

### [机器人上麦]

PUT `/channels/{channel_id}/mic`

将机器人语音连接到（上麦）此语音子频道。

```csharp
// API 请求
await voiceChannel.JoinAsync();
```

[机器人上麦]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/audio/put_mic.html
