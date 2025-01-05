using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyGuildRoleResponse
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("role_id")]
    public required uint RoleId { get; init; }

    [JsonPropertyName("role")]
    public required Role Role { get; init; }
}
