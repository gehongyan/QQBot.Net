using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class CreateGuildRoleParams
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("color")]
    [JsonConverter(typeof(HexAlphaColorConverter))]
    public AlphaColor? Color { get; set; }

    [JsonPropertyName("hoist")]
    [NumberBooleanConverter(WriteType = NumberBooleanConverter.EnumWriteType.Number)]
    public bool? Hoist { get; set; }
}
