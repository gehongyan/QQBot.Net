using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class GetForumThreadsResponse
{
    [JsonPropertyName("threads")]
    public required Thread[] Threads { get; set; }

    [JsonPropertyName("is_finish")]
    [JsonConverter(typeof(NumberBooleanConverter))]
    public required bool IsFinish { get; set; }
}
