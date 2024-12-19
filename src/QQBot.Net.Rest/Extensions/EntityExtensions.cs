using System.Collections.Immutable;
using System.Drawing;
using System.Text.Json;
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

    private static MessageArkObject ToModel(this IReadOnlyDictionary<string, string> entity) => new()
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

    #region RichText Model -> Entity

    public static TextElement ToEntity(this API.TextElement model)
    {
        TextStyle style = TextStyle.None;
        if (model.Properties is { Bold: true })
            style |= TextStyle.Bold;
        if (model.Properties is { Italic: true })
            style |= TextStyle.Italic;
        if (model.Properties is { Underline: true })
            style |= TextStyle.Underline;
        return new TextElement(model.Text, style);
    }

    public static ImageElement ToEntity(this API.PlatformImage model)
    {
        Size size = new(model.Width, model.Height);
        return new ImageElement(model.ImageId, model.Url, size);
    }

    public static VideoElement ToEntity(this API.PlatformVideo model)
    {
        Size size = new(model.Width, model.Height);
        Size coverSize = new(model.Cover.Width, model.Cover.Height);
        return new VideoElement(model.VideoId, model.Url, size,
            model.Duration.HasValue ? TimeSpan.FromSeconds(model.Duration.Value) : null,
            model.Cover.Url, coverSize);
    }

    public static UrlElement ToEntity(this API.UrlElement model)
    {
        return new UrlElement(model.Url, model.Description);
    }

    public static IElement ToEntity(this API.Element model)
    {
        return model switch
        {
            { ElementType: null } => new EmptyElement(),
            { ElementType: ElementType.Text, Text: { } text } => text.ToEntity(),
            { ElementType: ElementType.Image, Image.PlatformImage: { } image } => image.ToEntity(),
            { ElementType: ElementType.Video, Video.PlatformVideo: { } video } => video.ToEntity(),
            { ElementType: ElementType.Url, Url: { } url } => url.ToEntity(),
            _ => throw new JsonException("Missing element values for a rich text element.")
        };
    }

    public static Paragraph ToEntity(this API.Paragraph model)
    {
        ImmutableArray<IElement> elements = [..model.Elements.Select(x => x.ToEntity())];
        return new Paragraph(elements, model.Properties.Alignment);
    }

    public static RichText ToEntity(this API.RichText model)
    {
        return new RichText(model.Paragraphs.Select(x => x.ToEntity()).ToImmutableArray());
    }

    #endregion

    #region RichText Builder -> Model

    public static API.TextElement ToModel(this TextElementBuilder builder)
    {
        if (string.IsNullOrEmpty(builder.Text))
            throw new ArgumentNullException(nameof(builder.Text), "The text cannot be null or empty.");
        return new API.TextElement
        {
            Text = builder.Text,
            Properties = new TextProperties
            {
                Bold = builder.Style.HasFlag(TextStyle.Bold),
                Italic = builder.Style.HasFlag(TextStyle.Italic),
                Underline = builder.Style.HasFlag(TextStyle.Underline)
            }
        };
    }

    public static API.ImageElement ToModel(this ImageElementBuilder builder)
    {
        if (string.IsNullOrEmpty(builder.Url))
            throw new ArgumentNullException(nameof(builder.Url), "The url cannot be null or empty.");
        if (!builder.Ratio.HasValue)
            throw new ArgumentNullException(nameof(builder.Url), "The ratio cannot be null.");
        return new API.ImageElement
        {
            Url = builder.Url,
            Ratio = builder.Ratio.Value
        };
    }

    public static API.VideoElement ToModel(this VideoElementBuilder builder)
    {
        if (string.IsNullOrEmpty(builder.Url))
            throw new ArgumentNullException(nameof(builder.Url), "The url cannot be null or empty.");
        return new API.VideoElement
        {
            Url = builder.Url
        };
    }

    public static API.UrlElement ToModel(this UrlElementBuilder builder)
    {
        if (string.IsNullOrEmpty(builder.Url))
            throw new ArgumentNullException(nameof(builder.Url), "The url cannot be null or empty.");
        if (string.IsNullOrEmpty(builder.Description))
            throw new ArgumentNullException(nameof(builder.Description), "The description cannot be null or empty.");
        return new API.UrlElement
        {
            Url = builder.Url,
            Description = builder.Description
        };
    }

    public static API.Element ToModel(this IElementBuilder builder) => new()
    {
        ElementType = builder.Type is not ElementType.Empty ? builder.Type : null,
        Text = builder is TextElementBuilder text ? text.ToModel() : null,
        Image = builder is ImageElementBuilder image ? image.ToModel() : null,
        Video = builder is VideoElementBuilder video ? video.ToModel() : null,
        Url = builder is UrlElementBuilder url ? url.ToModel() : null
    };

    public static API.Paragraph ToModel(this ParagraphBuilder builder) => new()
    {
        Elements = [..builder.Elements.Select(x => x.ToModel())],
        Properties = new ParagraphProperties
        {
            Alignment = builder.Alignment
        }
    };

    public static API.RichText ToModel(this RichTextBuilder builder) => new()
    {
        Paragraphs = [..builder.Paragraphs.Select(x => x.ToModel())]
    };

    #endregion
}
