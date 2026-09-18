---
uid: Guides.QuickReference.Guild.Content.DeleteAnnouncement
title: 删除频道公告
---

# 删除频道公告

预声明变量

```csharp
IGuild guild = null;
IUserMessage message = null;
```

### [删除频道公告]

DELETE `/guilds/{guild_id}/announces/{message_id}`

```csharp
// API 请求，通过消息实体撤销公告
await guild.RevokeAnnouncementAsync(message);

// 或通过消息 ID 撤销
await guild.RevokeAnnouncementAsync(messageId: "message_id");
```

[删除频道公告]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/content/announces/delete_guild_announces.html
