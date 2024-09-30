using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class GuidJsonConverterFactory : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert == typeof(Guid) || typeToConvert == typeof(Guid?);

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(Guid))
            return new GuidJsonConverter();
        if (typeToConvert == typeof(Guid?))
            return new NullableGuidJsonConverter();
        return null;
    }
}

internal class GuidJsonConverter : JsonConverter<Guid>
{
    /// <inheritdoc />
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.GetString() is { } str && Guid.TryParse(str, out Guid guid)
            ? guid
            : throw new JsonException("An error occurred while processing the JSON data.");

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString("N").ToUpperInvariant());
}

internal class NullableGuidJsonConverter : JsonConverter<Guid?>
{
    /// <inheritdoc />
    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.GetString() is { } str && Guid.TryParse(str, out Guid guid) ? guid : null;

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString("N").ToUpperInvariant());
        else
            writer.WriteNullValue();
    }
}

internal class GuidJsonConverterAttribute : JsonConverterAttribute
{
    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert) => new GuidJsonConverterFactory();
}
