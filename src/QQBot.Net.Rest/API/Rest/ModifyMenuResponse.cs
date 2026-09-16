using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyMenuResponse
{
    [JsonPropertyName("version")]
    public int Version { get; init; }
}
