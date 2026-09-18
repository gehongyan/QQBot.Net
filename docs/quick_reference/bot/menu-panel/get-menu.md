---
uid: Guides.QuickReference.Bot.MenuPanel.GetMenu
title: 查询全局自定义菜单
---

# 查询全局自定义菜单

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;
```

### [查询全局自定义菜单]

GET `/v2/menu`

```csharp
// API 请求，获取当前机器人的全局自定义菜单
BotMenu menu = await _socketClient.GetMenuAsync();
// menu.Items 为菜单项集合，若未设置过则为空集合
```

[查询全局自定义菜单]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_menu.get.html
