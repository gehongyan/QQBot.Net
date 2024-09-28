using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageMarkdown
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("custom_template_id")]
    public string? CustomTemplateId { get; set; }

    [JsonPropertyName("params")]
    public MessageMarkdownParam[]? Params { get; set; }
}

internal class MessageMarkdownParam
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("value")]
    public required string Value { get; set; }
}
