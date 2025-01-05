using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageMarkdown
{
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("custom_template_id")]
    public string? CustomTemplateId { get; init; }

    [JsonPropertyName("params")]
    public MessageMarkdownParam[]? Params { get; init; }
}

internal class MessageMarkdownParam
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("values")]
    public required string[] Values { get; init; }
}
