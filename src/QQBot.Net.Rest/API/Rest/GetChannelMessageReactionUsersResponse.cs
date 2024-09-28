using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetChannelMessageReactionUsersResponse
{
    [JsonPropertyName("users")]
    public required User[] Users { get; set; }

    [JsonPropertyName("cookie")]
    public required string Cookie { get; set; }

    [JsonPropertyName("ie_end")]
    public required bool IsEnd { get; set; }
}
