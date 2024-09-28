using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ChannelPermissions
{
    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("user_id")]
    public ulong? UserId { get; set; }

    [JsonPropertyName("role_id")]
    public uint? RoleId { get; set; }

    [JsonPropertyName("permissions")]
    public required string Permissions { get; set; }
}
