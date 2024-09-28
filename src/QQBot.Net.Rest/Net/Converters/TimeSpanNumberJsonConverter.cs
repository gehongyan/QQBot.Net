using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class TimeSpanNumberJsonConverter : JsonConverter<TimeSpan>
{
    public TimeSpanUnit Unit { get; set; }

    /// <inheritdoc />
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();

        long timestamp = reader.GetInt64();
        return Unit switch
        {
            TimeSpanUnit.Milliseconds => TimeSpan.FromMilliseconds(timestamp),
            TimeSpanUnit.Seconds => TimeSpan.FromSeconds(timestamp),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        long timestamp = Unit switch
        {
            TimeSpanUnit.Milliseconds => (long)value.TotalMilliseconds,
            TimeSpanUnit.Seconds => (long)value.TotalSeconds,
            _ => throw new ArgumentOutOfRangeException()
        };

        writer.WriteNumberValue(timestamp);
    }

    public enum TimeSpanUnit
    {
        Milliseconds,
        Seconds
    }
}

internal class TimeSpanNumberJsonConverterAttribute : JsonConverterAttribute
{
    public TimeSpanNumberJsonConverter.TimeSpanUnit Unit { get; set; }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert) =>
        new TimeSpanNumberJsonConverter
        {
            Unit = Unit
        };
}
