using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyPanelResponse
{
    [JsonPropertyName("version")]
    public int Version { get; init; }
}
