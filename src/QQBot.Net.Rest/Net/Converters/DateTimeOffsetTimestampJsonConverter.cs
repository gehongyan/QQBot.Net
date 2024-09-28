using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class DateTimeOffsetTimestampJsonConverter : JsonConverter<DateTimeOffset>
{
    public TimestampUnit Unit { get; set; } = TimestampUnit.Milliseconds;

    /// <inheritdoc />
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();

        long timestamp = reader.GetInt64();
        return Unit switch
        {
            TimestampUnit.Milliseconds => DateTimeOffset.FromUnixTimeMilliseconds(timestamp),
            TimestampUnit.Seconds => DateTimeOffset.FromUnixTimeSeconds(timestamp),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        long timestamp = Unit switch
        {
            TimestampUnit.Milliseconds => value.ToUnixTimeMilliseconds(),
            TimestampUnit.Seconds => value.ToUnixTimeSeconds(),
            _ => throw new ArgumentOutOfRangeException()
        };

        writer.WriteNumberValue(timestamp);
    }

    public enum TimestampUnit
    {
        Milliseconds,
        Seconds
    }
}

internal class DateTimeOffsetTimestampJsonConverterAttribute : JsonConverterAttribute
{
    public DateTimeOffsetTimestampJsonConverter.TimestampUnit Unit { get; set; } = DateTimeOffsetTimestampJsonConverter.TimestampUnit.Milliseconds;

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert) =>
        new DateTimeOffsetTimestampJsonConverter
        {
            Unit = Unit
        };
}
