using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Role
{
    [JsonPropertyName("id")]
    public required uint Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("color")]
    [JsonConverter(typeof(HexAlphaColorConverter))]
    public required AlphaColor Color { get; set; }

    [JsonPropertyName("hoist")]
    [JsonConverter(typeof(NumberBooleanConverter))]
    public required bool Hoist { get; set; }

    [JsonPropertyName("number")]
    public required int Number { get; set; }

    [JsonPropertyName("member_limit")]
    public required int MemberLimit { get; set; }
}
