---
uid: Guides.QuickReference.Guild.Speak.GetMessageSetting
title: 获取频道消息频率的设置详情
---

# 获取频道消息频率的设置详情

预声明变量

```csharp
IGuild guild = null;
```

### [获取频道消息频率的设置详情]

GET `/guilds/{guild_id}/message/setting`

```csharp
// API 请求
MessageSetting setting = await guild.GetMessageSettingAsync();
```

[获取频道消息频率的设置详情]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/speak/setting/message_setting.html
