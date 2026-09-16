using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MenuSwitch
{
    [JsonPropertyName("switch_id")]
    public string? SwitchId { get; init; }

    [JsonPropertyName("default")]
    public bool Default { get; init; }
}
