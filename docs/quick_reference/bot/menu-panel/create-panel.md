---
uid: Guides.QuickReference.Bot.MenuPanel.CreatePanel
title: 创建指令面板
---

# 创建指令面板

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
```

### [创建指令面板]

POST `/v2/panels`

一个机器人最多创建 20 个指令面板。

```csharp
CommandPanelScope scope = default;              // 面板的生效场景
IEnumerable<CommandPanelItem> items = null;     // 面板元素列表，最多 20 个

// 创建对指定场景下所有用户或群生效的面板
ICommandPanel panel = await _socketClient.CreateCommandPanelAsync(scope, items,
    func: props => { /* 面板额外配置 */ });

// 或创建仅对指定用户/群生效的面板（仅 C2C 或 Group 场景）
IEnumerable<Guid> targetIds = null; // 生效对象的标识符集合，单次最多 20 个
ICommandPanel targetedPanel = await _socketClient.CreateCommandPanelAsync(scope, targetIds, items);
```

[创建指令面板]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_panels.post.html
