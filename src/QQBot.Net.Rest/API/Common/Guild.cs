using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Guild
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icon")]
    public required string Icon { get; set; }

    [JsonPropertyName("owner_id")]
    public required ulong OwnerId { get; set; }

    [JsonPropertyName("owner")]
    public required bool Owner { get; set; }

    [JsonPropertyName("member_count")]
    public required int MemberCount { get; set; }

    [JsonPropertyName("max_members")]
    public required int MaxMembers { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("joined_at")]
    public required DateTimeOffset JoinedAt { get; set; }
}
