using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Channel
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required ChannelType Type { get; set; }

    [JsonPropertyName("sub_type")]
    public ChannelSubType? SubType { get; set; }

    [JsonPropertyName("position")]
    public required int Position { get; set; }

    [JsonPropertyName("parent_id")]
    public ulong? ParentId { get; set; }

    [JsonPropertyName("owner_id")]
    public required ulong OwnerId { get; set; }

    [JsonPropertyName("private_type")]
    public PrivateType? PrivateType { get; set; }

    [JsonPropertyName("speak_permission")]
    public SpeakPermission? SpeakPermission { get; set; }

    [JsonPropertyName("application_id")]
    [JsonConverter(typeof(ChannelApplicationJsonConverter))]
    public ChannelApplication? ApplicationId { get; set; }

    [JsonPropertyName("permissions")]
    public string? Permissions { get; set; }
}
