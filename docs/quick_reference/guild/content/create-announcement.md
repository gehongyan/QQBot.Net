---
uid: Guides.QuickReference.Guild.Content.CreateAnnouncement
title: 创建频道公告
---

# 创建频道公告

预声明变量

```csharp
IGuild guild = null;
IUserMessage message = null;
```

### [创建频道公告]

POST `/guilds/{guild_id}/announces`

将一条消息设置为频道公告。

```csharp
// API 请求，通过消息实体发布
await guild.PublishAnnouncementAsync(message);

// 或通过子频道 ID 与消息 ID 发布
await guild.PublishAnnouncementAsync(channelId: 0ul, messageId: "message_id");
```

[创建频道公告]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/announces/post_guild_announces.html
