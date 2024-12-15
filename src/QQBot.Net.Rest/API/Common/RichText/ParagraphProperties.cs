using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ParagraphProperties
{
    [JsonPropertyName("alignment")]
    public TextAlignment? Alignment { get; set; }
}
