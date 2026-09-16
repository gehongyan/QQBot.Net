using System.Text.Json.Serialization;
using QQBot.API;

namespace QQBot.API.Rest;

internal class ModifyPanelParams
{
    [JsonPropertyName("panel")]
    public required Panel Panel { get; init; }
}
