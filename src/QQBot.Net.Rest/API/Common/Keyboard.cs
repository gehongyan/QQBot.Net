using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Keyboard
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("content")]
    public KeyboardContent? Content { get; set; }
}

internal class KeyboardContent
{
    [JsonPropertyName("rows")]
    public required KeyboardRow[] Rows { get; set; }
}

internal class KeyboardRow
{
    [JsonPropertyName("buttons")]
    public required KeyboardButton[] Buttons { get; set; }
}

internal class KeyboardButton
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("render_data")]
    public required KeyboardRenderData RenderData { get; set; }

    [JsonPropertyName("action")]
    public required KeyboardAction Action { get; set; }
}

internal class KeyboardRenderData
{
    [JsonPropertyName("label")]
    public required string Label { get; set; }

    [JsonPropertyName("visited_label")]
    public required string VisitedLabel { get; set; }

    [JsonPropertyName("style")]
    public required KeyboardButtonStyle Style { get; set; }
}

internal class KeyboardAction
{
    [JsonPropertyName("type")]
    public required KeyboardActionType Type { get; set; }

    [JsonPropertyName("permission")]
    public required KeyboardPermission Permission { get; set; }

    [JsonPropertyName("data")]
    public required string Data { get; set; }

    [JsonPropertyName("reply")]
    public bool? Reply { get; set; }

    [JsonPropertyName("enter")]
    public bool? Enter { get; set; }

    [JsonPropertyName("anchor")]
    public int? Anchor { get; set; }

    [Obsolete]
    [JsonPropertyName("click_limit")]
    public int? ClickLimit { get; set; }

    [Obsolete]
    [JsonPropertyName("at_bot_show_channel_list")]
    public bool? AtBotShowChannelList { get; set; }

    [JsonPropertyName("unsupport_tips")]
    public required string UnsupportedTips { get; set; }
}

internal class KeyboardPermission
{
    [JsonPropertyName("type")]
    public required int Type { get; set; }

    [JsonPropertyName("specify_user_ids")]
    public string[]? SpecifyUserIds { get; set; }

    [JsonPropertyName("specify_role_ids")]
    public string[]? SpecifyRoleIds { get; set; }
}
