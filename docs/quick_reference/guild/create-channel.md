---
uid: Guides.QuickReference.Guild.CreateChannel
title: 创建子频道
---

# 创建子频道

预声明变量

```csharp
IGuild guild = null;
```

### [创建子频道]

POST `/guilds/{guild_id}/channels`

```csharp
string name = null; // 子频道名称

// API 请求，创建文字子频道
ITextChannel textChannel = await guild.CreateTextChannelAsync(name, func: props =>
{
    // 子频道额外属性配置
});
```

[创建子频道]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/guilds_guild_id_channels.post.html
