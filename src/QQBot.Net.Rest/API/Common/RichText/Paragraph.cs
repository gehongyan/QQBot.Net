using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Paragraph
{
    [JsonPropertyName("elems")]
    public required Element[] Elements { get; set; }

    [JsonPropertyName("props")]
    public required ParagraphProperties Properties { get; set; }
}
