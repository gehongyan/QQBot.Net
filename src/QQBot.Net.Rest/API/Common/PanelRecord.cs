using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class PanelRecord
{
    [JsonPropertyName("panel_id")]
    public required string PanelId { get; init; }

    [JsonPropertyName("scope")]
    [JsonConverter(typeof(CommandPanelScopeJsonConverter))]
    public CommandPanelScope Scope { get; init; }

    [JsonPropertyName("target_type")]
    [JsonConverter(typeof(CommandPanelTargetTypeJsonConverter))]
    public CommandPanelTargetType TargetType { get; init; }

    [JsonPropertyName("panel")]
    public Panel? Panel { get; init; }

    [JsonPropertyName("created_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset UpdatedAt { get; init; }

    [JsonPropertyName("version")]
    public int Version { get; init; }
}
