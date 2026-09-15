using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendUserGroupMessageResponseExtInfo
{
    [JsonPropertyName("ref_idx")]
    public string? ReplyReferenceId { get; init; }
}
