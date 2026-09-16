using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyMenuParams
{
    [JsonPropertyName("menu")]
    public required Menu Menu { get; init; }
}
