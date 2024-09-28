using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Member
{
    [JsonPropertyName("user")]
    public required User User { get; set; }

    [JsonPropertyName("nick")]
    public required string Nickname { get; set; }

    [JsonPropertyName("roles")]
    public required string[] Roles { get; set; }

    [JsonPropertyName("joined_at")]
    public required DateTimeOffset JoinedAt { get; set; }
}
