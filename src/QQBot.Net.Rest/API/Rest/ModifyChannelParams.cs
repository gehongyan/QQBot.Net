using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyChannelParams
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("parent_id")]
    public ulong? CategoryId { get; set; }

    [JsonPropertyName("private_type")]
    public PrivateType? PrivateType { get; set; }

    [JsonPropertyName("speak_permission")]
    public SpeakPermission SpeakPermission { get; set; }
}
