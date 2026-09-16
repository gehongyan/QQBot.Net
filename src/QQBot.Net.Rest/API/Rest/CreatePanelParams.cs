using System.Text.Json.Serialization;
using QQBot.API;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class CreatePanelParams
{
    [JsonPropertyName("scope")]
    [JsonConverter(typeof(CommandPanelScopeJsonConverter))]
    public CommandPanelScope Scope { get; init; }

    [JsonPropertyName("target_type")]
    [JsonConverter(typeof(CommandPanelTargetTypeJsonConverter))]
    public CommandPanelTargetType TargetType { get; init; }

    [JsonPropertyName("user_openids")]
    public string[]? UserOpenids { get; init; }

    [JsonPropertyName("group_openids")]
    public string[]? GroupOpenids { get; init; }

    [JsonPropertyName("panel")]
    public required Panel Panel { get; init; }
}
