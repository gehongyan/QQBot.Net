using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetGroupBlacklistResponse
{
    [JsonPropertyName("users")]
    public required GroupBlacklistUser[] Users { get; init; }

    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; init; }
}
