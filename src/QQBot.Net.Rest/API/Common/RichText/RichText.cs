using System.Text.Json.Serialization;

namespace QQBot.API;

internal class RichText
{
    [JsonPropertyName("paragraphs")]
    public required Paragraph[] Paragraphs { get; set; }
}
