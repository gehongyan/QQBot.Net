using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Rest;

internal static class ForumHelper
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static RichText ParseContent(string content)
    {
        API.RichText? model = JsonSerializer.Deserialize<API.RichText>(content, _serializerOptions);
        if (model is null)
            throw new JsonException("Failed to parse the content.");
        return model.ToEntity();
    }
}
