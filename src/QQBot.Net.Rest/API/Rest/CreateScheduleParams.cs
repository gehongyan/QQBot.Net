using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreateScheduleParams
{
    [JsonPropertyName("schedule")]
    public required ScheduleParams Schedule { get; set; }
}
