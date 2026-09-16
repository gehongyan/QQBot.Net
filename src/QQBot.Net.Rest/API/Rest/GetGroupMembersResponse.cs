using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetGroupMembersResponse
{
    [JsonPropertyName("members")]
    public required GroupMember[] Members { get; init; }

    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; init; }
}
