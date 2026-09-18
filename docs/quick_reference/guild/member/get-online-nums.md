---
uid: Guides.QuickReference.Guild.Member.GetOnlineNums
title: 获取子频道在线成员数
---

# 获取子频道在线成员数

预声明变量

```csharp
IVoiceChannel voiceChannel = null;
ILiveStreamChannel liveStreamChannel = null;
```

### [获取子频道在线成员数]

GET `/channels/{channel_id}/online_nums`

仅适用于音视频子频道与直播子频道。

```csharp
// API 请求
int voiceOnline = await voiceChannel.CountOnlineUsersAsync();
int liveOnline = await liveStreamChannel.CountOnlineUsersAsync();
```

[获取子频道在线成员数]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/role/get_online_nums.html
