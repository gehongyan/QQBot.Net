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

    #region Markdown

    public static MessageMarkdown ToModel(this IMarkdown entity) => new()
    {
        Content = (entity as MarkdownText)?.Text,
        CustomTemplateId = (entity as MarkdownTemplate)?.TemplateId,
        Params = (entity as MarkdownTemplate)?.Parameters.Select(x => new MessageMarkdownParam
        {
            Key = x.Key,
            Values = [..x.Value]
        }).ToArray()
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

    #region Ark

    public static MessageArk ToModel(this Ark entity) => new()
    {
        TemplateId = entity.TemplateId,
        KeyValues = entity.Parameters.Select(x => x.ToModel()).ToArray()
    };

    private static MessageArkKeyValue ToModel(this KeyValuePair<string, IArkParameter> entity) => new()
    {
        Key = entity.Key,
        Value = (entity.Value as ArkSingleParameter?)?.Value,
        Obj = (entity.Value as ArkMultiDictionaryParameter?)?.Value.Select(x => x.ToModel()).ToArray()
    };

    private static MessageArkObject ToModel(this IReadOnlyDictionary<string,string> entity) => new()
    {
        ObjectKeyValues = entity.Select(x => new MessageArkObjectKeyValue
        {
            Key = x.Key,
            Value = x.Value
        }).ToArray()
    };

    #endregion

    #region Keyboard

    public static Keyboard ToModel(this IKeyboard entity) => new()
    {
        Id = (entity as KeyboardTemplate)?.TemplateId,
        Content = (entity as KeyboardContent)?.ToModel()
    };

    private static API.KeyboardContent ToModel(this KeyboardContent entity) => new()
    {
        Rows = entity.Rows.Select(x => x.ToModel()).ToArray()
    };

    private static KeyboardRow ToModel(this KeyboardButtonRow entity) => new()
    {
        Buttons = entity.Buttons.Select(x => x.ToModel()).ToArray()
    };

    private static API.KeyboardButton ToModel(this KeyboardButton entity) => new()
    {
        Id = entity.Id,
        RenderData = new KeyboardRenderData
        {
            Label = entity.Label,
            LabelVisited = entity.LabelVisited,
            Style = entity.Style
        },
        Action = new KeyboardAction
        {
            Type = entity.Action,
            Permission = new KeyboardPermission
            {
                Type = entity.Permission,
                SpecifyUserIds = entity.AllowedUserIds?.ToArray(),
                SpecifyRoleIds = entity.AllowedRoleIds?.ToArray()
            },
            Data = entity.Data,
            Reply = entity.IsCommandReply,
            Enter = entity.IsCommandAutoSend,
            Anchor = entity.ActionAnchor,
            UnsupportedTips = entity.UnsupportedVersionTip
        }
    };

    #endregion
}
