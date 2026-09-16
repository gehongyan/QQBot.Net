using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Panel
{
    [JsonPropertyName("items")]
    public PanelItem[]? Items { get; init; }

    [JsonPropertyName("remark")]
    public string? Remark { get; init; }

    [JsonPropertyName("version")]
    public int Version { get; init; }
}
