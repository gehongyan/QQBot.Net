using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyRolePermissionsParams
{
    [JsonPropertyName("add")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public ulong? Add { get; set; }

    [JsonPropertyName("remove")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public ulong? Remove { get; set; }
}
