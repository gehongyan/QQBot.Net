using System.Text.Json.Serialization;

namespace QQBot.API;

internal class TextElement
{
    [JsonPropertyName("text")]
    public required string Text { get; set; }

    [JsonPropertyName("props")]
    public TextProperties? Properties { get; set; }
}
