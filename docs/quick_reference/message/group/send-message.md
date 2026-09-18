---
uid: Guides.QuickReference.Message.Group.SendMessage
title: 发送群聊消息
---

# 发送群聊消息

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;

IGroupChannel groupChannel = null;
```

### [发送群聊消息]

POST `/v2/groups/{group_openid}/messages`

```csharp
string content = null;                  // 要发送的文本消息内容
IMarkdown markdown = null;              // Markdown 消息内容
FileAttachment? attachment = null;     // 文件附件
Embed embed = null;                    // 嵌入式消息内容
Ark ark = null;                        // 模板消息内容
IKeyboard keyboard = null;             // 按钮
MessageReference messageReference = null; // 消息引用，用于回复消息
IUserMessage passiveSource = null;     // 被动消息来源

// API 请求，发送群聊消息
IUserMessage message = await groupChannel.SendMessageAsync(content, markdown, attachment,
    embed, ark, keyboard, messageReference, passiveSource);
```

各消息体（Markdown / Ark / Embed / 按钮 / 文件附件 / 消息引用）的构造方式，
参见 [发送单聊消息](xref:Guides.QuickReference.Message.C2C.SendMessage) 中的详细说明，
以及 [Markdown 消息](xref:Guides.QuickReference.Message.Type.Markdown)、
[Ark 消息](xref:Guides.QuickReference.Message.Type.Ark)、[Embed 消息](xref:Guides.QuickReference.Message.Type.Embed)、
[文本交互](xref:Guides.QuickReference.Message.Interaction.Text)、[富媒体上传](xref:Guides.QuickReference.Message.Type.RichMedia)。

[发送群聊消息]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_messages.post.html
