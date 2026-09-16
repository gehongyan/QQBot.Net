using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class GroupMemberRoleJsonConverter : JsonConverter<GroupMemberRole>
{
    /// <inheritdoc />
    public override GroupMemberRole Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "owner" => GroupMemberRole.Owner,
            "admin" => GroupMemberRole.Admin,
            _ => GroupMemberRole.Member
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, GroupMemberRole value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            GroupMemberRole.Owner => "owner",
            GroupMemberRole.Admin => "admin",
            _ => "member"
        });
}
