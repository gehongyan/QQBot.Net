using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class BatchRemoveGroupMembersResponse
{
    [JsonPropertyName("remove_members_result")]
    public string? RemoveMembersResult { get; init; }

    [JsonPropertyName("add_to_member_blacklist_fail_openids")]
    public string[]? AddToMemberBlacklistFailOpenids { get; init; }
}
