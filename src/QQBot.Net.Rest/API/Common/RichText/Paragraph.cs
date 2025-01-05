using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Paragraph
{
    [JsonPropertyName("elems")]
    public required Element[] Elements { get; init; }

    [JsonPropertyName("props")]
    public required ParagraphProperties Properties { get; init; }
}
