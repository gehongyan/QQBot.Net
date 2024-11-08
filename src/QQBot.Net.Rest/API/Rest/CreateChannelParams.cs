using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class CreateChannelParams
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public ChannelType? Type { get; set; }

    [JsonPropertyName("sub_type")]
    public ChannelSubType? SubType { get; set; }

    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("parent_id")]
    public ulong? CategoryId { get; set; }

    [JsonPropertyName("private_type")]
    public PrivateType? PrivateType { get; set; }

    [JsonPropertyName("private_user_ids")]
    public ulong[]? PrivateUserIds { get; set; }

    [JsonPropertyName("speak_permission")]
    public SpeakPermission? SpeakPermission { get; set; }

    [JsonPropertyName("application_id")]
    [JsonConverter(typeof(ChannelApplicationJsonConverter))]
    public ChannelApplication? ApplicationId { get; set; }
}
