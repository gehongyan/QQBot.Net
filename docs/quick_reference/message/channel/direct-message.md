---
uid: Guides.QuickReference.Message.Channel.DirectMessage
title: 频道私信
---

# 频道私信

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;

IDMChannel dmChannel = null;
```

### [发送频道私信]

POST `/dms/{guild_id}/messages`

```csharp
string content = null;                  // 要发送的文本消息内容
IMarkdown markdown = null;              // Markdown 消息内容
FileAttachment? attachment = null;     // 文件附件
Embed embed = null;                    // 嵌入式消息内容
Ark ark = null;                        // 模板消息内容
IKeyboard keyboard = null;             // 按钮
MessageReference messageReference = null; // 消息引用
IUserMessage passiveSource = null;     // 被动消息来源

// API 请求，发送频道私信
IUserMessage message = await dmChannel.SendMessageAsync(content, markdown, attachment,
    embed, ark, keyboard, messageReference, passiveSource);
```

各消息体的构造方式，参见 [发送单聊消息](xref:Guides.QuickReference.Message.C2C.SendMessage) 中的详细说明，
以及 [Markdown 消息](xref:Guides.QuickReference.Message.Type.Markdown)、
[Ark 消息](xref:Guides.QuickReference.Message.Type.Ark)、[Embed 消息](xref:Guides.QuickReference.Message.Type.Embed)、
[文本交互](xref:Guides.QuickReference.Message.Interaction.Text)。

[发送频道私信]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/dms.html
