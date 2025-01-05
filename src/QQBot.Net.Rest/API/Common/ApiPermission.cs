using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class ApiPermission
{
    [JsonPropertyName("path")]
    public required string Path { get; init; }

    [JsonPropertyName("method")]
    public required string Method { get; init; }

    [JsonPropertyName("desc")]
    public required string Description { get; init; }

    [JsonPropertyName("auth_status")]
    [JsonConverter(typeof(NumberBooleanConverter))]
    public required bool AuthStatus { get; init; }
}
