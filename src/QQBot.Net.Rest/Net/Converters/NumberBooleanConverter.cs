using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class NumberBooleanConverter : JsonConverter<bool>
{
    public EnumWriteType WriteType { get; set; } = EnumWriteType.Boolean;

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number => reader.TryGetInt32(out int value) && value == 1,
            JsonTokenType.String => reader.GetString() == "1",
            _ => throw new JsonException(
                $"{nameof(NumberBooleanConverter)} expects boolean, string or number token, but got {reader.TokenType}")
        };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        switch (WriteType)
        {
            case EnumWriteType.Boolean:
                writer.WriteBooleanValue(value);
                break;
            case EnumWriteType.String:
                writer.WriteStringValue(value ? "1" : "0");
                break;
            case EnumWriteType.Number:
                writer.WriteNumberValue(value ? 1 : 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public enum EnumWriteType
    {
        Boolean,
        String,
        Number
    }
}

internal class NumberBooleanConverterAttribute : JsonConverterAttribute
{
    public NumberBooleanConverter.EnumWriteType WriteType { get; set; } = NumberBooleanConverter.EnumWriteType.Boolean;

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert) =>
        new NumberBooleanConverter
        {
            WriteType = WriteType
        };
}
