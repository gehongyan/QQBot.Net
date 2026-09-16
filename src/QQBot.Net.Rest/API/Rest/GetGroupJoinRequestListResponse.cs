using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetGroupJoinRequestListResponse
{
    [JsonPropertyName("list")]
    public required GroupJoinRequest[] List { get; init; }

    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; init; }
}
