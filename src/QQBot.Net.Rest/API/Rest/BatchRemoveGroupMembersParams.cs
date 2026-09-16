using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class BatchRemoveGroupMembersParams
{
    [JsonPropertyName("member_openids")]
    public required string[] MemberOpenIds { get; init; }

    [JsonPropertyName("add_to_member_blacklist")]
    public bool? AddToMemberBlacklist { get; init; }
}
