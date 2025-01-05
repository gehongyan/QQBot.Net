using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Keyboard
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("content")]
    public KeyboardContent? Content { get; init; }
}

internal class KeyboardContent
{
    [JsonPropertyName("rows")]
    public required KeyboardRow[] Rows { get; init; }
}

internal class KeyboardRow
{
    [JsonPropertyName("buttons")]
    public required KeyboardButton[] Buttons { get; init; }
}

internal class KeyboardButton
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("render_data")]
    public required KeyboardRenderData RenderData { get; init; }

    [JsonPropertyName("action")]
    public required KeyboardAction Action { get; init; }
}

internal class KeyboardRenderData
{
    [JsonPropertyName("label")]
    public required string Label { get; init; }

    [JsonPropertyName("visited_label")]
    public required string LabelVisited { get; init; }

    [JsonPropertyName("style")]
    public required ButtonStyle Style { get; init; }
}

internal class KeyboardAction
{
    [JsonPropertyName("type")]
    public required ButtonAction Type { get; init; }

    [JsonPropertyName("permission")]
    public required KeyboardPermission Permission { get; init; }

    [JsonPropertyName("data")]
    public required string Data { get; init; }

    [JsonPropertyName("reply")]
    public bool? Reply { get; init; }

    [JsonPropertyName("enter")]
    public bool? Enter { get; init; }

    [JsonPropertyName("anchor")]
    public ButtonActionAnchor? Anchor { get; init; }

    [Obsolete]
    [JsonPropertyName("click_limit")]
    public int? ClickLimit { get; init; }

    [Obsolete]
    [JsonPropertyName("at_bot_show_channel_list")]
    public bool? AtBotShowChannelList { get; init; }

    [JsonPropertyName("unsupport_tips")]
    public required string UnsupportedTips { get; init; }
}

internal class KeyboardPermission
{
    [JsonPropertyName("type")]
    public required ButtonPermission Type { get; init; }

    [JsonPropertyName("specify_user_ids")]
    public string[]? SpecifyUserIds { get; init; }

    [JsonPropertyName("specify_role_ids")]
    public uint[]? SpecifyRoleIds { get; init; }
}
