using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyRolePermissionsParams
{
    [JsonPropertyName("add")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public ulong? Add { get; init; }

    [JsonPropertyName("remove")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public ulong? Remove { get; init; }
}
