---
uid: Guides.QuickReference.Bot.MenuPanel.UpdatePanelTarget
title: 修改指令面板关联对象
---

# 修改指令面板关联对象

预声明变量

```csharp
ICommandPanel panel = null;
```

### [修改指令面板关联对象]

PUT `/v2/panels/{panel_id}/target`

调整面板生效的用户或群。仅 C2C 与 Group 场景的面板支持按指定对象生效。

```csharp
IEnumerable<Guid> targetIds = null; // 要增加或移除的生效对象标识符集合

// API 请求，增加生效对象
await panel.AddTargetsAsync(targetIds);
// API 请求，移除生效对象
await panel.RemoveTargetsAsync(targetIds);
```

[修改指令面板关联对象]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_panels_panel_id_target.put.html
