using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class InteractionEvent
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("type")]
    public int Type { get; init; }

    [JsonPropertyName("scene")]
    public string? Scene { get; init; }

    [JsonPropertyName("chat_type")]
    public int? ChatType { get; init; }

    [JsonPropertyName("timestamp")]
    public DateTimeOffset? Timestamp { get; init; }

    [JsonPropertyName("guild_id")]
    public ulong? GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; init; }

    [JsonPropertyName("user_openid")]
    [GuidJsonConverter]
    public Guid? UserOpenId { get; init; }

    [JsonPropertyName("group_openid")]
    [GuidJsonConverter]
    public Guid? GroupOpenId { get; init; }

    [JsonPropertyName("group_member_openid")]
    [GuidJsonConverter]
    public Guid? GroupMemberOpenId { get; init; }

    [JsonPropertyName("data")]
    public InteractionData? Data { get; init; }

    [JsonPropertyName("version")]
    public int? Version { get; init; }
}

internal class InteractionData
{
    [JsonPropertyName("type")]
    public int Type { get; init; }

    [JsonPropertyName("resolved")]
    public InteractionResolvedData? Resolved { get; init; }
}

internal class InteractionResolvedData
{
    [JsonPropertyName("button_data")]
    public string? ButtonData { get; init; }

    [JsonPropertyName("button_id")]
    public string? ButtonId { get; init; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; init; }

    [JsonPropertyName("feature_id")]
    public string? FeatureId { get; init; }

    [JsonPropertyName("message_id")]
    public string? MessageId { get; init; }
}
