using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreatePanelResponse
{
    [JsonPropertyName("panel_id")]
    public required string PanelId { get; init; }
}
