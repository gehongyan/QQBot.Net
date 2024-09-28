using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MemberWithGuildId : Member
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }
}
