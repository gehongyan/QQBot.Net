using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetPanelListResponse
{
    [JsonPropertyName("records")]
    public required PanelRecord[] Records { get; init; }

    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; init; }

    [JsonPropertyName("is_end")]
    public bool IsEnd { get; init; }
}
