using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetChannelMessageReactionUsersResponse
{
    [JsonPropertyName("users")]
    public required User[] Users { get; init; }

    [JsonPropertyName("cookie")]
    public string? Cookie { get; init; }

    [JsonPropertyName("is_end")]
    public required bool IsEnd { get; init; }
}
