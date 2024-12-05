using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetApplicationGuildPermissionResponse
{
    [JsonPropertyName("apis")]
    public required IReadOnlyCollection<ApiPermission> ApiPermissions { get; set; }
}
