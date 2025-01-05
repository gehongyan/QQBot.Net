using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class CreateGuildRoleParams
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("color")]
    [JsonConverter(typeof(HexAlphaColorConverter))]
    public AlphaColor? Color { get; init; }

    [JsonPropertyName("hoist")]
    [NumberBooleanConverter(WriteType = NumberBooleanConverter.EnumWriteType.Number)]
    public bool? Hoist { get; init; }
}
