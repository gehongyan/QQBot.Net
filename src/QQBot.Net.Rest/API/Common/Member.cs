using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Member
{
    [JsonPropertyName("user")]
    public User? User { get; init; }

    [JsonPropertyName("nick")]
    public string? Nickname { get; init; }

    [JsonPropertyName("roles")]
    public uint[]? Roles { get; init; }

    [JsonPropertyName("joined_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public required DateTimeOffset JoinedAt { get; init; }
}
