using System.Text.Json.Serialization;

namespace QQBot.API;

internal class TextProperties
{
    [JsonPropertyName("font_bold")]
    public bool? Bold { get; set; }

    [JsonPropertyName("italic")]
    public bool? Italic { get; set; }

    [JsonPropertyName("underline")]
    public bool? Underline { get; set; }
}
