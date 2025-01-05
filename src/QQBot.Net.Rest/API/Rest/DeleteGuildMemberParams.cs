using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class DeleteGuildMemberParams
{
    [JsonPropertyName("add_blacklist")]
    public bool? AddBlacklist { get; init; }

    [JsonPropertyName("delete_history_msg_days")]
    public int? DeleteHistoryMessageDays { get; init; }
}
