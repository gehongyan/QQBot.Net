---
uid: Guides.QuickReference.Message.Interaction.Button.Response
title: 互动事件响应
---

# 互动事件响应

预声明变量

```csharp
SocketInteraction interaction = null;
```

### [互动事件响应]

PUT `/interactions/{interaction_id}`

在互动事件处理程序中，通过 <xref:QQBot.WebSocket.SocketInteraction.RespondAsync*> 回应互动。
一个互动只能被回应一次。

```csharp
string content = null;                  // 回应的文本内容
IMarkdown markdown = null;              // 回应的 Markdown 内容
FileAttachment? attachment = null;     // 回应的文件附件
Embed embed = null;                    // 回应的嵌入式消息内容
Ark ark = null;                        // 回应的模板消息内容
IKeyboard keyboard = null;             // 回应的按钮
MessageReference messageReference = null; // 消息引用

// API 请求，回应互动
await interaction.RespondAsync(content, markdown, attachment, embed, ark, keyboard, messageReference);
```

> 若配置了 <xref:QQBot.WebSocket.QQBotSocketConfig.AutoAcknowledgeInteractions>（默认开启），
> 未被用户代码回应的互动会在 <xref:QQBot.WebSocket.QQBotSocketConfig.InteractionAutoAcknowledgeDelay> 后自动确认。

[互动事件响应]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/interactions_interaction_id.put.html
