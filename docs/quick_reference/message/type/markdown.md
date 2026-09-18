---
uid: Guides.QuickReference.Message.Type.Markdown
title: Markdown 消息
---

# Markdown 消息

QQBot.Net 通过 <xref:QQBot.IMarkdown> 表示 Markdown 消息内容，构造完成后可传入各频道
`SendMessageAsync` 的 `markdown` 参数。推荐使用原生 Markdown（<xref:QQBot.MarkdownText>）。

### [原生 Markdown]

<xref:QQBot.MarkdownTextBuilder> 支持多种构造方式：

```csharp
// 直接以文本构造
MarkdownText a = new MarkdownTextBuilder("# 标题\n正文 **加粗**").Build();

// 无参构造后设置 Text
MarkdownTextBuilder builder = new MarkdownTextBuilder();
builder.Text = "*斜体* 与 [链接](https://bot.q.qq.com)";
MarkdownText b = builder.Build();

// 以 StringBuilder 构造
StringBuilder sb = new StringBuilder().AppendLine("第一行").AppendLine("第二行");
MarkdownText c = new MarkdownTextBuilder(sb).Build();
```

对于在 <xref:QQBot.IUserChannel> 与 <xref:QQBot.IGroupChannel> 发送的 Markdown，可通过
`ForceVerifyImageResource` 控制图片资源校验行为（文字子频道与频道私信忽略此选项）。

### [模板 Markdown（已弃用）]

<xref:QQBot.MarkdownTemplateBuilder> 用于构造模板 Markdown，但 QQ 平台的 Markdown 模板已弃用，
新代码请改用原生 Markdown。其参数支持设置与追加：

```csharp
#pragma warning disable CS0618 // 模板 Markdown 已弃用
MarkdownTemplateBuilder templateBuilder = new MarkdownTemplateBuilder("template_id");
// 设置参数（覆盖）
templateBuilder.AddParameter("key", "value1", "value2");
templateBuilder.AddParameter("key2", new List<string> { "v" });
// 追加参数（在已有值末尾追加）
templateBuilder.AppendParameter("key", "value3");
MarkdownTemplate template = templateBuilder.Build();
#pragma warning restore CS0618
```

[原生 Markdown]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/type/markdown.html
[模板 Markdown（已弃用）]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/type/markdown.html
