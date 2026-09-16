using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetGroupMuteSettingResponse
{
    [JsonPropertyName("global_rule")]
    public GroupGlobalMuteRule? GlobalRule { get; init; }

    [JsonPropertyName("members")]
    public GroupMemberMuteState[]? Members { get; init; }
}
