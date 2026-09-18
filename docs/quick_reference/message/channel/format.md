---
uid: Guides.QuickReference.Message.Channel.Format
title: 内嵌格式
---

# 内嵌格式

QQBot.Net 提供 <xref:QQBot.Format> 与 <xref:QQBot.MentionUtils> 两个工具类，用于构造与处理消息中的
内嵌格式（Markdown 文本样式、提及、图片、命令、表情等）。

### [文本样式]

```csharp
string text = "示例文本";

Format.Bold(text);          // **示例文本**
Format.Italics(text);       // *示例文本*
Format.BoldItalics(text);   // ***示例文本***
Format.Strikethrough(text); // ~~示例文本~~
Format.H1("标题");           // # 标题
Format.H2("小标题");         // ## 标题
Format.BlockQuote("引用");   // > 引用（多行会逐行加 >）
Format.HorizontalRule();    // 分割线 ***
Format.NewLine(2);          // 多行换行
// 各样式方法的 sanitize 参数（默认 true）会先转义与该样式冲突的字符
```

### [链接与图片]

```csharp
Format.Url("https://bot.q.qq.com");                 // <url>
Format.Url("https://bot.q.qq.com", "文本");          // [文本](url)
Format.Url(new Uri("https://bot.q.qq.com"), "文本"); // Uri 重载

Format.Image("https://example.com/i.png");                       // ![](url)
Format.Image("https://example.com/i.png", "替代文本");            // ![替代文本](url)
Format.Image(new Uri("https://example.com/i.png"), "替代文本");   // Uri 重载
// 也可从图片类型的 FileAttachment 构造：Format.Image(attachment)
```

### [列表]

```csharp
Format.OrderedList(new[] { "第一项", "第二项" });   // 1. / 2.
Format.UnorderedList(new[] { "甲", "乙" });         // - 甲 / - 乙
// 两者均可传 indentLevel 控制缩进级别
```

### [命令与表情]

```csharp
Format.Command("/help");                       // 点击后直接发送的命令
Format.Command("/help", "帮助", reference: true); // 带显示文本与引用
Format.Emote(Emotes.System.Angry);             // 系统表情
Format.Emote(4);                               // 按表情 ID
```

### [转义与反转义]

```csharp
Format.Sanitize(text);     // 在敏感字符前插入零宽空格，使内嵌格式失效（可传自定义敏感字符）
Format.Escape(text);       // 将 & < > 转义为 &amp; &lt; &gt;（保留显示形式）
Format.Unescape(text);     // Escape 的逆操作，使内嵌格式生效
```

### [提及]

```csharp
IUser user = null;
ITextChannel channel = null;

MentionUtils.MentionUser(user);       // <qqbot-at-user id=".." />
MentionUtils.MentionUser("user_id");  // 按用户 ID
MentionUtils.MentionChannel(channel); // <#id>
MentionUtils.MentionChannel(123ul);   // 按子频道 ID
MentionUtils.MentionEveryone;         // <qqbot-at-everyone />
```

### [解析提及]

```csharp
ulong userId = MentionUtils.ParseUser("<qqbot-at-user id=\"123\" />");
bool okUser = MentionUtils.TryParseUser("<@!123>", out ulong parsedUserId);
ulong channelId = MentionUtils.ParseChannel("<#123>");
bool okChannel = MentionUtils.TryParseChannel("<#123>", out ulong parsedChannelId);
```

[文本样式]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/format.html
[链接与图片]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/format.html
[列表]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/format.html
[命令与表情]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/format.html
[转义与反转义]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/format.html
[提及]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/format.html
[解析提及]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/channel/message/format.html
