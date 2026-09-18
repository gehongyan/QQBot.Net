---
uid: Guides.QuickReference.Message.Type.Embed
title: Embed 消息
---

# Embed 消息

Embed 是结构化的嵌入式消息。QQBot.Net 通过 <xref:QQBot.Embed> 与 <xref:QQBot.EmbedBuilder> 构造，
构造完成的 <xref:QQBot.Embed> 可传入各频道 `SendMessageAsync` 的 `embed` 参数。

### [构造 Embed 消息]

```csharp
Embed embed = new EmbedBuilder()
    .WithTitle("标题")                       // 标题
    .WithPrompt("弹窗内容")                    // 弹窗提示内容
    .WithThumbnailUrl("https://example.com/thumb.png") // 缩略图 URL
    // 添加字段：仅指定名称
    .AddField("字段一")
    // 添加字段：使用委托配置字段构建器
    .AddField(field => field.WithName("字段二"))
    // 添加字段：直接传入字段构建器
    .AddField(new EmbedFieldBuilder().WithName("字段三"))
    .Build();
```

### [字段构建器]

<xref:QQBot.EmbedFieldBuilder> 用于构造单个字段，通过 `WithName(string)` 设置字段名称。

```csharp
EmbedFieldBuilder field = new EmbedFieldBuilder().WithName("字段名");
EmbedField built = field.Build();
```

[构造 Embed 消息]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/type/embed.html
[字段构建器]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/type/embed.html
