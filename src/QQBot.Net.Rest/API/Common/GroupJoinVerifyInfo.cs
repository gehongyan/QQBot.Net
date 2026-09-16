using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class GroupJoinVerifyInfo
{
    [JsonPropertyName("method")]
    [JsonConverter(typeof(GroupJoinVerifyMethodJsonConverter))]
    public GroupJoinVerifyMethod Method { get; init; }

    [JsonPropertyName("verify_message")]
    public string? VerifyMessage { get; init; }

    [JsonPropertyName("review_qa_list")]
    public GroupJoinReviewQa[]? ReviewQaList { get; init; }
}
