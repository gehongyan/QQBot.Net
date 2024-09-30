using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class DateTimeOffsetTimestampJsonConverter : JsonConverter<DateTimeOffset>
{
    public Format Unit { get; set; } = Format.Milliseconds;

    /// <inheritdoc />
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (Unit)
        {
            case Format.Milliseconds:
                if (reader.TokenType is JsonTokenType.Number)
                    return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64());
                if (reader.TokenType is JsonTokenType.String
                    && reader.GetString() is { } millisecondStr)
                    return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(millisecondStr));
                throw new JsonException();
            case Format.Seconds:
                if (reader.TokenType is JsonTokenType.Number)
                    return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64());
                if (reader.TokenType is JsonTokenType.String
                    && reader.GetString() is { } secondStr)
                    return DateTimeOffset.FromUnixTimeSeconds(long.Parse(secondStr));
                throw new JsonException();
            case Format.RFC3339 or Format.ISO8601:
                if (reader.TokenType != JsonTokenType.String
                    || reader.GetString() is not { } dateTimeString)
                    throw new JsonException();
                return DateTimeOffset.Parse(dateTimeString);
            default:
                throw new NotSupportedException($"Unsupported format: {Unit}");
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        switch (Unit)
        {
            case Format.Milliseconds:
                writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
                break;
            case Format.Seconds:
                writer.WriteNumberValue(value.ToUnixTimeSeconds());
                break;
            case Format.RFC3339:
                writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ssK"));
                break;
            case Format.ISO8601:
                writer.WriteStringValue(value.ToString("O"));
                break;
            default:
                throw new NotSupportedException($"Unsupported format: {Unit}");
        }
    }

    public enum Format
    {
        Milliseconds,
        Seconds,
        RFC3339,
        ISO8601
    }
}

internal class DateTimeOffsetTimestampJsonConverterAttribute : JsonConverterAttribute
{
    public DateTimeOffsetTimestampJsonConverter.Format Unit { get; set; } = DateTimeOffsetTimestampJsonConverter.Format.Milliseconds;

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert) =>
        new DateTimeOffsetTimestampJsonConverter
        {
            Unit = Unit
        };
}
