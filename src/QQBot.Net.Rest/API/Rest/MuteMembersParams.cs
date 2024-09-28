using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class MuteMembersParams : MuteMemberParams
{
    [JsonPropertyName("user_ids")]
    public required long[] UserIds { get; set; }
}
