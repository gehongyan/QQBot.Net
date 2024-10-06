using System.Collections.Immutable;
using QQBot.API;

namespace QQBot.Rest;

internal static class EntityExtensions
{
    #region MessageReference

    public static MessageReference ToEntity(this API.MessageReference model) =>
        new(model.MessageId, !model.IgnoreGetMessageError);

    public static API.MessageReference ToModel(this MessageReference entity) =>
        new()
        {
            MessageId = entity.MessageId,
            IgnoreGetMessageError = !entity.FailIfNotExists
        };

    #endregion

    #region Embed

    public static Embed ToEntity(this MessageEmbed model)
    {
        return new Embed(model.Title, model.Prompt, model.Thumbnail?.ToEntity(),
            model.Fields?.Select(x => x.ToEntity()).ToImmutableArray() ?? []);
    }

    public static MessageEmbed ToModel(this Embed entity) =>
        new()
        {
            Title = entity.Title,
            Prompt = entity.Prompt,
            Thumbnail = entity.Thumbnail?.ToModel(),
            Fields = entity.Fields.Select(x => x.ToModel()).ToArray()
        };

    public static EmbedThumbnail ToEntity(this MessageEmbedThumbnail model) => new(model.Url);

    public static MessageEmbedThumbnail ToModel(this EmbedThumbnail entity) =>
        new()
        {
            Url = entity.Url
        };

    public static EmbedField ToEntity(this MessageEmbedField model) => new(model.Name);

    public static MessageEmbedField ToModel(this EmbedField entity) =>
        new()
        {
            Name = entity.Name
        };

    #endregion
}
