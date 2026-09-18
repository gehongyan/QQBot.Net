---
uid: Guides.QuickReference.Bot.MenuPanel.UpdatePanel
title: 修改指令面板
---

# 修改指令面板

预声明变量

```csharp
ICommandPanel panel = null;
```

### [修改指令面板]

PUT `/v2/panels/{panel_id}`

```csharp
IEnumerable<CommandPanelItem> items = null; // 新的面板元素列表

// API 请求，通过面板实体修改
await panel.ModifyAsync(items);
```

[修改指令面板]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_panels_panel_id.put.html
