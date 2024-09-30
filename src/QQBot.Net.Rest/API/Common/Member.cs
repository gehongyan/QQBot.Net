using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Member
{
    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("nick")]
    public required string Nickname { get; set; }

    [JsonPropertyName("roles")]
    public required uint[] Roles { get; set; }

    [JsonPropertyName("joined_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public required DateTimeOffset JoinedAt { get; set; }
}
