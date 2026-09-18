---
uid: Guides.QuickReference.Message.C2C.SendMessage
title: 发送单聊消息
---

# 发送单聊消息

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;
readonly QQBotRestClient _restClient = null;

IUserChannel userChannel = null;
Stream stream = null;
ReadOnlyMemory<byte> memory = default;
```

### [发送单聊消息]

POST `/v2/users/{openid}/messages`

```csharp
string content = null;                  // 要发送的文本消息内容
IMarkdown markdown = null;              // Markdown 消息内容，构造见 Markdown 消息
FileAttachment? attachment = null;     // 文件附件，构造见下
Embed embed = null;                    // 嵌入式消息内容，构造见 Embed 消息
Ark ark = null;                        // 模板消息内容，构造见 Ark 消息
IKeyboard keyboard = null;             // 按钮，构造见文本交互
MessageReference messageReference = null; // 消息引用，用于回复消息
IUserMessage passiveSource = null;     // 被动消息来源，作为对某条用户消息的被动回复

// API 请求，发送单聊消息
IUserMessage message = await userChannel.SendMessageAsync(content, markdown, attachment,
    embed, ark, keyboard, messageReference, passiveSource);
```

各消息体的构造方式参见：[Markdown 消息](xref:Guides.QuickReference.Message.Type.Markdown)、
[Ark 消息](xref:Guides.QuickReference.Message.Type.Ark)、[Embed 消息](xref:Guides.QuickReference.Message.Type.Embed)、
[文本交互](xref:Guides.QuickReference.Message.Interaction.Text)、[富媒体上传](xref:Guides.QuickReference.Message.Type.RichMedia)。

### [构造文件附件]

<xref:QQBot.FileAttachment> 支持 5 种构造方式（均可指定文件名与 <xref:QQBot.AttachmentType>）：

```csharp
FileAttachment fromPath = new FileAttachment("path/to/image.png", type: AttachmentType.Image);
FileAttachment fromStream = new FileAttachment(stream, "image.png", AttachmentType.Image);
FileAttachment fromUri = new FileAttachment(new Uri("https://example.com/i.png"), "i.png", AttachmentType.Image);
FileAttachment fromMemory = new FileAttachment(memory, "image.png", AttachmentType.Image);
// 由富媒体上传结果（MediaFileInfo）构造，单聊/群聊分别传入
// FileAttachment fromMedia = new FileAttachment(userMediaFileInfo, groupMediaFileInfo, "image.png");

// FileAttachment 实现 IDisposable，使用后应释放（using）
using FileAttachment attachmentToSend = fromPath;
await userChannel.SendMessageAsync(attachment: attachmentToSend);
```

### [引用回复]

<xref:QQBot.MessageReference> 用于回复指定消息：

```csharp
MessageReference reference = new MessageReference("message_id");
// 可指定当被引用消息不存在时是否报错（默认视为 true）
MessageReference reference2 = new MessageReference("message_id", failIfNotExists: false);

await userChannel.SendMessageAsync("回复内容", messageReference: reference);
```

[发送单聊消息]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_users_user_openid_messages.post.html
[构造文件附件]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_users_user_openid_messages.post.html
[引用回复]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_users_user_openid_messages.post.html
