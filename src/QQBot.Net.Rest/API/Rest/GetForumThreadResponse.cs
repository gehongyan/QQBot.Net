using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetForumThreadResponse
{
    [JsonPropertyName("thread")]
    public required Thread Thread { get; set; }
}
