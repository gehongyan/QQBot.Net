---
uid: Guides.QuickReference.Message.Type.Ark
title: Ark 消息
---

# Ark 消息

Ark 是基于模板的结构化消息。QQBot.Net 通过 <xref:QQBot.Ark> 与 <xref:QQBot.ArkBuilder> 构造，
构造完成的 <xref:QQBot.Ark> 可传入各频道 `SendMessageAsync` 的 `ark` 参数。

### [构造 Ark 模板消息]

```csharp
int templateId = default; // 已在平台申请的 Ark 模板 ID

ArkBuilder builder = new ArkBuilder(templateId);

// 单值参数：等价于 ArkSingleParameterBuilder().WithValue(value)
builder.WithParameter("key", "value");

// 多字典参数（列表型）：每个字典是一组键值
builder.WithParameter("list", new Dictionary<string, string> { ["a"] = "1" },
    new Dictionary<string, string> { ["a"] = "2" });
builder.WithParameter("list", new List<IReadOnlyDictionary<string, string>>());

// 直接传入参数构建器
builder.WithParameter("key2", new ArkSingleParameterBuilder().WithValue("v"));
// 或使用委托配置指定类型的参数构建器
builder.WithParameter<ArkMultiDictionaryParameterBuilder>("key3", p => { /* 配置 */ });

Ark ark = builder.Build();
```

### [参数构建器类型]

- <xref:QQBot.ArkSingleParameterBuilder>：单值参数，`WithValue(string)`。
- <xref:QQBot.ArkMultiDictionaryParameterBuilder>：多字典（列表）参数，接收一组 `IReadOnlyDictionary<string,string>`。

[构造 Ark 模板消息]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/type/ark.html
[参数构建器类型]: https://bot.q.qq.com/wiki/develop/api-v2/server-inter/message/type/ark.html
