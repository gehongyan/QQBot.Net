using System.Text.Json;
using System.Text.Json.Serialization;
using QQBot.Net.Rest;

namespace QQBot.API.Rest;

internal class SendChannelMessageParams
{
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("embed")]
    public MessageEmbed? Embed { get; init; }

    [JsonPropertyName("ark")]
    public MessageArk? Ark { get; init; }

    [JsonPropertyName("message_reference")]
    public MessageReference? MessageReference { get; init; }

    [JsonPropertyName("image")]
    public string? Image { get; init; }

    [JsonIgnore]
    public MultipartFile? FileImage { get; init; }

    [JsonPropertyName("msg_id")]
    public string? MessageId { get; init; }

    [JsonPropertyName("event_id")]
    public string? EventId { get; init; }

    [JsonPropertyName("markdown")]
    public MessageMarkdown? Markdown { get; init; }

    public IReadOnlyDictionary<string, object> ToDictionary(JsonSerializerOptions options)
    {
        Dictionary<string, object> dict = [];
        if (Content is not null)
            dict["content"] = Content;
        if (Embed is not null)
            dict["embed"] = JsonSerializer.SerializeToElement(Embed, options);
        if (Ark is not null)
            dict["ark"] = JsonSerializer.SerializeToElement(Ark, options);
        if (MessageReference is not null)
            dict["message_reference"] = JsonSerializer.SerializeToElement(MessageReference, options);
        if (Image is not null)
            dict["image"] = Image;
        if (FileImage.HasValue)
            dict["file_image"] = FileImage;
        if (MessageId is not null)
            dict["msg_id"] = MessageId;
        if (EventId is not null)
            dict["event_id"] = EventId;
        if (Markdown is not null)
            dict["markdown"] = JsonSerializer.SerializeToElement(Markdown, options);
        return dict;
    }
}
