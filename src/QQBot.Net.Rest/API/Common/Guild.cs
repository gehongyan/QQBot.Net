using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Guild
{
    [JsonPropertyName("id")]
    public required ulong Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("icon")]
    public required string Icon { get; init; }

    [JsonPropertyName("owner_id")]
    public required ulong OwnerId { get; init; }

    [JsonPropertyName("owner")]
    public required bool Owner { get; init; }

    [JsonPropertyName("member_count")]
    public required int MemberCount { get; init; }

    [JsonPropertyName("max_members")]
    public required int MaxMembers { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("joined_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public required DateTimeOffset JoinedAt { get; init; }
}
