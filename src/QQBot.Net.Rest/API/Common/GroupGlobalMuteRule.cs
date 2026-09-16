using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class GroupGlobalMuteRule
{
    [JsonPropertyName("mode")]
    [JsonConverter(typeof(GroupGlobalMuteModeJsonConverter))]
    public GroupGlobalMuteMode Mode { get; init; }

    [JsonPropertyName("schedule_rules")]
    public GroupMuteScheduleRule[]? ScheduleRules { get; init; }

    [JsonPropertyName("recurring_rules")]
    public GroupMuteRecurringRule[]? RecurringRules { get; init; }
}
