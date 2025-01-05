using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ChannelPermissions
{
    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("user_id")]
    public ulong? UserId { get; init; }

    [JsonPropertyName("role_id")]
    public uint? RoleId { get; init; }

    [JsonPropertyName("permissions")]
    public required string Permissions { get; init; }
}
