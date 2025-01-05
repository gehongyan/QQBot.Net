using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyChannelParams
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("position")]
    public int? Position { get; init; }

    [JsonPropertyName("parent_id")]
    public ulong? CategoryId { get; init; }

    [JsonPropertyName("private_type")]
    public PrivateType? PrivateType { get; init; }

    [JsonPropertyName("speak_permission")]
    public SpeakPermission? SpeakPermission { get; init; }
}
