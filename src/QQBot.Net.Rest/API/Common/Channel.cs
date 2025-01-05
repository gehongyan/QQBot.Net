using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Channel
{
    [JsonPropertyName("id")]
    public required ulong Id { get; init; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("type")]
    public required ChannelType Type { get; init; }

    [JsonPropertyName("sub_type")]
    public ChannelSubType? SubType { get; init; }

    [JsonPropertyName("position")]
    public int? Position { get; init; }

    [JsonPropertyName("parent_id")]
    public ulong? ParentId { get; init; }

    [JsonPropertyName("owner_id")]
    public required ulong OwnerId { get; init; }

    [JsonPropertyName("private_type")]
    public PrivateType? PrivateType { get; init; }

    [JsonPropertyName("speak_permission")]
    public SpeakPermission? SpeakPermission { get; init; }

    [JsonPropertyName("application_id")]
    [JsonConverter(typeof(ChannelApplicationJsonConverter))]
    public ChannelApplication? ApplicationId { get; init; }

    [JsonPropertyName("permissions")]
    public string? Permissions { get; init; }
}
