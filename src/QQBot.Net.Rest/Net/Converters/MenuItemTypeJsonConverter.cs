using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class MenuItemTypeJsonConverter : JsonConverter<MenuItemType>
{
    /// <inheritdoc />
    public override MenuItemType Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "switch" => MenuItemType.Switch,
            "link" => MenuItemType.Link,
            "menu" => MenuItemType.Menu,
            _ => MenuItemType.SendMessage
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, MenuItemType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            MenuItemType.Switch => "switch",
            MenuItemType.Link => "link",
            MenuItemType.Menu => "menu",
            _ => "send_message"
        });
}
