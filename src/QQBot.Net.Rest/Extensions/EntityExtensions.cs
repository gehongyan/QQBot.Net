using System.Collections.Immutable;

namespace QQBot.Rest;

internal static class EntityExtensions
{
    public static Embed ToEntity(this API.MessageEmbed model)
    {
        return new Embed(model.Title, model.Prompt, model.Thumbnail?.ToEntity(),
            model.Fields?.Select(x => x.ToEntity()).ToImmutableArray() ?? []);
    }

    public static API.MessageEmbed ToModel(this Embed entity) =>
        new()
        {
            Title = entity.Title,
            Prompt = entity.Prompt,
            Thumbnail = entity.Thumbnail?.ToModel(),
            Fields = entity.Fields.Select(x => x.ToModel()).ToArray()
        };

    public static EmbedThumbnail ToEntity(this API.MessageEmbedThumbnail model) => new(model.Url);

    public static API.MessageEmbedThumbnail ToModel(this EmbedThumbnail entity) =>
        new()
        {
            Url = entity.Url
        };

    public static EmbedField ToEntity(this API.MessageEmbedField model) => new(model.Name);

    public static API.MessageEmbedField ToModel(this EmbedField entity) =>
        new()
        {
            Name = entity.Name
        };
}
