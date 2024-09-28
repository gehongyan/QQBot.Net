using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyGuildRoleResponse
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("role_id")]
    public required uint RoleId { get; set; }

    [JsonPropertyName("role")]
    public required Role Role { get; set; }
}
