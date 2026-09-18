---
uid: Guides.QuickReference.Guild.Member.Event.AudioLiveMember
title: 音视频/直播子频道成员进出事件
---

# 音视频/直播子频道成员进出事件

预声明变量

```csharp
readonly QQBotSocketClient _client = null;
```

### [成员进入音视频/直播子频道]

```csharp
_client.UserConnected += (member, channel) => Task.CompletedTask;
// member 是进入的成员（可延迟加载）
// channel 是被进入的音视频/直播子频道
```

### [成员退出音视频/直播子频道]

```csharp
_client.UserDisconnected += (member, channel) => Task.CompletedTask;
// member 是退出的成员（可延迟加载）
// channel 是被退出的音视频/直播子频道
```

[成员进入音视频/直播子频道]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/audio_or_live_channel_member.html
[成员退出音视频/直播子频道]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/audio_or_live_channel_member.html
