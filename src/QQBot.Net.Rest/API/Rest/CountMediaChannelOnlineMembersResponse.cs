using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CountMediaChannelOnlineMembersResponse
{
    [JsonPropertyName("online_nums")]
    public int OnlineMembers { get; set; }
}
