---
uid: Guides.QuickReference.Guild.Content.AudioControl
title: 音频控制
---

# 音频控制

预声明变量

```csharp
IVoiceChannel voiceChannel = null;
```

### [音频控制]

PUT `/channels/{channel_id}/audio`

```csharp
string url = null;         // 要播放的音频 URL
string displayText = null; // 状态文本

// 开始播放
await voiceChannel.PlayAsync(url, displayText);
// 暂停
await voiceChannel.PauseAsync();
// 继续
await voiceChannel.ResumeAsync();
// 停止
await voiceChannel.StopAsync();
```

[音频控制]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/audio/audio_control.html
