using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyScheduleParams
{
    [JsonPropertyName("schedule")]
    public required ScheduleParams Schedule { get; init; }
}
