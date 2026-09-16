using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SetGroupMuteSettingParams
{
    [JsonPropertyName("members")]
    public required SetGroupMemberMuteState[] Members { get; init; }
}
