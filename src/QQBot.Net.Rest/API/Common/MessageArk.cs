using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageArk
{
    [JsonPropertyName("template_id")]
    public required int TemplateId { get; set; }

    [JsonPropertyName("kv")]
    public required MessageArkKeyValue[] KeyValues { get; set; }
}

internal class MessageArkKeyValue
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("obj")]
    public MessageArkObject[]? Obj { get; set; }
}

internal class MessageArkObject
{
    [JsonPropertyName("obj_kv")]
    public required MessageArkObjectKeyValue[] ObjectKeyValues { get; set; }
}

internal class MessageArkObjectKeyValue
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("value")]
    public required string Value { get; set; }
}
