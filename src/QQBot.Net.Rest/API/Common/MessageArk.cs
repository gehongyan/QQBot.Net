using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageArk
{
    [JsonPropertyName("template_id")]
    public required int TemplateId { get; init; }

    [JsonPropertyName("kv")]
    public required MessageArkKeyValue[] KeyValues { get; init; }
}

internal class MessageArkKeyValue
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("value")]
    public string? Value { get; init; }

    [JsonPropertyName("obj")]
    public MessageArkObject[]? Obj { get; init; }
}

internal class MessageArkObject
{
    [JsonPropertyName("obj_kv")]
    public required MessageArkObjectKeyValue[] ObjectKeyValues { get; init; }
}

internal class MessageArkObjectKeyValue
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }
}
