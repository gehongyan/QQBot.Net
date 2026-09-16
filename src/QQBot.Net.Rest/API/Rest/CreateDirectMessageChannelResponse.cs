using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class CreateDirectMessageChannelResponse
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; init; }

    [JsonPropertyName("create_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Seconds)]
    public DateTimeOffset? CreateTime { get; init; }
}
