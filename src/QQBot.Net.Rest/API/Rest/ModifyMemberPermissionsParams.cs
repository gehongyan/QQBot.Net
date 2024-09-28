using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyMemberPermissionsParams
{
    [JsonPropertyName("add")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public uint? Add { get; set; }

    [JsonPropertyName("remove")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public uint? Remove { get; set; }
}
