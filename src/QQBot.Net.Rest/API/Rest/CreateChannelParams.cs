using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class CreateChannelParams
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public ChannelType? Type { get; init; }

    [JsonPropertyName("sub_type")]
    public ChannelSubType? SubType { get; init; }

    [JsonPropertyName("position")]
    public int? Position { get; init; }

    [JsonPropertyName("parent_id")]
    public ulong? CategoryId { get; init; }

    [JsonPropertyName("private_type")]
    public PrivateType? PrivateType { get; init; }

    [JsonPropertyName("private_user_ids")]
    public ulong[]? PrivateUserIds { get; init; }

    [JsonPropertyName("speak_permission")]
    public SpeakPermission? SpeakPermission { get; init; }

    [JsonPropertyName("application_id")]
    [JsonConverter(typeof(ChannelApplicationJsonConverter))]
    public ChannelApplication? ApplicationId { get; init; }
}
