using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Role
{
    [JsonPropertyName("id")]
    public required uint Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("color")]
    [JsonConverter(typeof(HexAlphaColorConverter))]
    public required AlphaColor Color { get; init; }

    [JsonPropertyName("hoist")]
    [JsonConverter(typeof(NumberBooleanConverter))]
    public required bool Hoist { get; init; }

    [JsonPropertyName("number")]
    public required int Number { get; init; }

    [JsonPropertyName("member_limit")]
    public required int MemberLimit { get; init; }
}
