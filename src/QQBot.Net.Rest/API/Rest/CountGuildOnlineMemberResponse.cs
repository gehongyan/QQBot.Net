using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CountGuildOnlineMemberResponse
{
    [JsonPropertyName("online_nums")]
    public int OnlineMembers { get; set; }
}
