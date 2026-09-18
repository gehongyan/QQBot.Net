---
uid: Guides.QuickReference.Bot.MenuPanel.GetPanels
title: 查询指令面板列表
---

# 查询指令面板列表

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
```

### [查询指令面板列表]

GET `/v2/panels`

```csharp
CommandPanelScope scope = default; // 要筛选的生效场景

// API 请求，以分页形式返回指定场景下当前生效的指令面板
IAsyncEnumerable<IReadOnlyCollection<ICommandPanel>> panels =
    _socketClient.GetCommandPanelsAsync(scope);
```

[查询指令面板列表]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_panels.get.html
