using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class DeleteGuildMemberParams
{
    [JsonPropertyName("add_blacklist")]
    public bool? AddBlacklist { get; set; }

    [JsonPropertyName("delete_history_msg_days")]
    public int? DeleteHistoryMessageDays { get; set; }
}
