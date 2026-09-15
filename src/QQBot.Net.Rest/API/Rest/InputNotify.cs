using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class InputNotify
{
    [JsonPropertyName("input_type")]
    public required InputNotifyType InputType { get; init; }

    [JsonPropertyName("input_second")]
    public required int InputSecond { get; init; }
}
