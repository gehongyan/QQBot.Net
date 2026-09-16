using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class GetGroupInfoResponse
{
    [JsonPropertyName("group_openid")]
    [GuidJsonConverter]
    public required Guid GroupOpenId { get; init; }

    [JsonPropertyName("group_name")]
    public string? GroupName { get; init; }

    [JsonPropertyName("group_finger_memo")]
    public string? GroupFingerMemo { get; init; }

    [JsonPropertyName("group_class_text")]
    public string? GroupClassText { get; init; }

    [JsonPropertyName("group_tags")]
    public string[]? GroupTags { get; init; }

    [JsonPropertyName("group_member_num")]
    public int GroupMemberNum { get; init; }
}
