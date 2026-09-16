using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class GroupJoinVerifyMethodJsonConverter : JsonConverter<GroupJoinVerifyMethod>
{
    /// <inheritdoc />
    public override GroupJoinVerifyMethod Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "admin_review_qa" => GroupJoinVerifyMethod.AdminReviewQa,
            _ => GroupJoinVerifyMethod.VerifyMessage
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, GroupJoinVerifyMethod value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            GroupJoinVerifyMethod.AdminReviewQa => "admin_review_qa",
            _ => "verify_message"
        });
}
