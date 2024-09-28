using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetGuildRolesResponse
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("roles")]
    public required Role[] Roles { get; set; }

    [JsonPropertyName("role_num_limit")]
    public required int RoleNumLimit { get; set; }
}
