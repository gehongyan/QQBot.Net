using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreateGuildRoleResponse
{
    [JsonPropertyName("role_id")]
    public required uint RoleId { get; init; }

    [JsonPropertyName("role")]
    public required Role Role { get; init; }
}
