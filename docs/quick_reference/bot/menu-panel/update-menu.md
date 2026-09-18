---
uid: Guides.QuickReference.Bot.MenuPanel.UpdateMenu
title: 修改全局自定义菜单
---

# 修改全局自定义菜单

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;
```

### [修改全局自定义菜单]

PUT `/v2/menu`

传入的菜单项将覆盖原有的完整菜单配置，最多 10 个。全局自定义菜单仅在 QQ 单聊（C2C）场景生效。

```csharp
IEnumerable<MenuItem> items = null; // 要设置的菜单项列表，最多 10 个

// API 请求，修改全局自定义菜单
BotMenu menu = await _socketClient.ModifyMenuAsync(items);
```

[修改全局自定义菜单]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_menu.put.html
