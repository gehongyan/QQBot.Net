using System.Text.Json.Serialization;

namespace QQBot.API;

internal class TextProperties
{
    [JsonPropertyName("font_bold")]
    public bool? Bold { get; init; }

    [JsonPropertyName("italic")]
    public bool? Italic { get; init; }

    [JsonPropertyName("underline")]
    public bool? Underline { get; init; }
}
