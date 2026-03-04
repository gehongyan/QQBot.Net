using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class UserBotAddingEvent : UserBotEvent
{
    [JsonPropertyName("group_scene")]
    public required UserChannelSource Scheme { get; set; }

    [JsonPropertyName("scene_param")]
    public string? SchemeParam { get; set; }
}
