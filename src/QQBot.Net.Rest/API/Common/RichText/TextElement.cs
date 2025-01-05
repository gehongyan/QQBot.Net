using System.Text.Json.Serialization;

namespace QQBot.API;

internal class TextElement
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    [JsonPropertyName("props")]
    public TextProperties? Properties { get; init; }
}
