using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetGuildRoleMembersResponse
{
    [JsonPropertyName("members")]
    public required Member[] Members { get; set; }

    [JsonPropertyName("next")]
    public required string Next { get; set; }
}
