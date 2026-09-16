using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyPanelTargetParams
{
    [JsonPropertyName("op")]
    public required string Op { get; init; }

    [JsonPropertyName("user_openids")]
    public string[]? UserOpenids { get; init; }

    [JsonPropertyName("group_openids")]
    public string[]? GroupOpenids { get; init; }
}
