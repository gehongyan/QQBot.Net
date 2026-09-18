---
uid: Guides.QuickReference.Bot.MenuPanel.DeletePanel
title: 删除指令面板
---

# 删除指令面板

预声明变量

```csharp
ICommandPanel panel = null;
```

### [删除指令面板]

DELETE `/v2/panels/{panel_id}`

```csharp
// API 请求，通过面板实体删除
await panel.DeleteAsync();
```

[删除指令面板]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_panels_panel_id.delete.html
