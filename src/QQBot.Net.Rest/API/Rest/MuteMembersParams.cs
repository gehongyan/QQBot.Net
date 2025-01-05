using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class MuteMembersParams : MuteMemberParams
{
    [JsonPropertyName("user_ids")]
    public required ulong[] UserIds { get; init; }
}
