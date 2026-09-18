---
uid: Guides.QuickReference.Bot.MenuPanel.GetPanel
title: 查询指令面板详情
---

# 查询指令面板详情

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
```

### [查询指令面板详情]

GET `/v2/panels/{panel_id}`

```csharp
string panelId = null; // 要获取的面板标识符

// API 请求
ICommandPanel panel = await _socketClient.GetCommandPanelAsync(panelId);
```

[查询指令面板详情]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_panels_panel_id.get.html
