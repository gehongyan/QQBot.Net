---
uid: Guides.QuickReference.Message.Interaction.Text
title: 文本交互
---

# 文本交互

QQBot.Net 通过键盘（<xref:QQBot.IKeyboard>）构造消息中的按钮/文本交互组件。构造完成的键盘
可传入各频道 `SendMessageAsync` 的 `keyboard` 参数。提供两种构造方式：自定义键盘
（<xref:QQBot.KeyboardContentBuilder>）与模板键盘（<xref:QQBot.KeyboardTemplateBuilder>）。

### [自定义键盘]

<xref:QQBot.KeyboardContentBuilder> 以行（<xref:QQBot.KeyboardButtonRowBuilder>）组织按钮，
最多 5 行。提供多种添加方式：

```csharp
KeyboardContentBuilder builder = new KeyboardContentBuilder()
    // 以参数直接添加按钮（内部创建 KeyboardButtonBuilder）
    .AddButton(id: "1", label: "回调按钮", style: ButtonStyle.Primary, action: ButtonAction.Callback)
    .AddButton(id: "2", label: "链接按钮", action: ButtonAction.Link, data: "https://bot.q.qq.com")
    // 以委托配置按钮构建器
    .AddButton(b => b.WithLabel("指令按钮").WithData("/help"))
    // 以按钮构建器实例添加
    .AddButton(new KeyboardButtonBuilder(id: "3", label: "第三个"))
    // 显式添加一行
    .AddRow(row => row.AddButton(new KeyboardButtonBuilder().WithLabel("新行按钮")));

IKeyboard keyboard = builder.Build();
```

> 当某行按钮数达到 <xref:QQBot.KeyboardButtonRowBuilder.MaxChildCount> 时，`AddButton` 会尝试添加到下一行。

### [模板键盘]

```csharp
IKeyboard templateKeyboard = new KeyboardTemplateBuilder("template_id").Build();
```

[自定义键盘]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/trans/text-chain.html
[模板键盘]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/trans/text-chain.html
