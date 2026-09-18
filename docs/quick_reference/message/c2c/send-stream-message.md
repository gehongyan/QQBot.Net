---
uid: Guides.QuickReference.Message.C2C.SendStreamMessage
title: 流式发送单聊消息
---

# 流式发送单聊消息

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;

IUserChannel userChannel = null;
```

流式消息仅适用于 QQ 单聊。推荐使用由 QQBot.Net 管理的流式消息会话，会话会自动维护分片序号和流式消息标识符。

### [流式发送单聊消息]

POST `/v2/users/{openid}/messages`（流式）

```csharp
string initialContent = null;   // 首个内容分片
IUserMessage passiveSource = null; // 可选的被动回复消息来源

// 开始一个由 QQBot.Net 管理的流式消息会话
IUserMessageStream stream = await userChannel.StartStreamMessageAsync(initialContent,
    StreamMessageContentType.Text, passiveSource);

// 追加内容分片
await stream.AppendAsync("继续输出的内容");
// 显式结束流式消息
await stream.CompleteAsync("最后一段内容");
```

如需对分片序号、输入模式和完成状态进行完全控制，可使用底层分片方法：

```csharp
StreamMessageChunk chunk = default; // 要发送的流式消息分片

// 发送单个流式消息分片
StreamMessageChunkResult result = await userChannel.SendStreamMessageChunkAsync(chunk);
```

[流式发送单聊消息]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_users_user_openid_stream_messages.post.html
