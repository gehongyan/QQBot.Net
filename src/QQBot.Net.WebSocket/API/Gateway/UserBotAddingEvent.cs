using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class UserBotAddingEvent : UserBotEvent
{
    [JsonPropertyName("scene")]
    public required UserChannelSource Scene { get; set; }

    [JsonPropertyName("scene_param")]
    public string? SceneParam { get; set; }
}
