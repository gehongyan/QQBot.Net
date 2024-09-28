using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetChannelMessageReactionUsersParams
{
    [JsonPropertyName("cookie")]
    public string? Cookie { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }
}
