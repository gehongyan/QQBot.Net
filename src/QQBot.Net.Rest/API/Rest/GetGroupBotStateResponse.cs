using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class GetGroupBotStateResponse
{
    [JsonPropertyName("member_openid")]
    [GuidJsonConverter]
    public required Guid MemberOpenId { get; init; }

    [JsonPropertyName("joined_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset JoinedAt { get; init; }

    [JsonPropertyName("allow_proactive_msg")]
    public bool AllowProactiveMsg { get; init; }

    [JsonPropertyName("recv_msg_setting")]
    [JsonConverter(typeof(GroupMessageReceiveSettingJsonConverter))]
    public GroupMessageReceiveSetting RecvMsgSetting { get; init; }

    [JsonPropertyName("member_role")]
    [JsonConverter(typeof(GroupMemberRoleJsonConverter))]
    public GroupMemberRole MemberRole { get; init; }
}
