namespace QQBot;

/// <summary>
///     表示一个用于构建 <see cref="QQBot.Embed"/> 的构建器类。
/// </summary>
public class EmbedBuilder : IEquatable<EmbedBuilder>
{
    private EmbedThumbnail? _thumbnail;

    /// <summary>
    ///     获取或设置要为嵌入式消息设置的标题。
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///     获取或设置要为嵌入式消息设置的弹窗内容。
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    ///     获取或设置要为嵌入式消息设置的缩略图 URL。
    /// </summary>
    public string? ThumbnailUrl
    {
        get => _thumbnail?.Url;
        set => _thumbnail = new EmbedThumbnail(value);
    }

    /// <summary>
    ///     获取或设置要为嵌入式消息设置的字段。
    /// </summary>
    public List<EmbedFieldBuilder> Fields { get; } = [];

    /// <summary>
    ///     设置嵌入式消息的标题。
    /// </summary>
    /// <param name="title"> 要设置的标题。 </param>
    /// <returns> 当前构建器。 </returns>
    public EmbedBuilder WithTitle(string title)
    {
        Title = title;
        return this;
    }

    /// <summary>
    ///     设置嵌入式消息的弹窗内容。
    /// </summary>
    /// <param name="prompt"> 要设置的弹窗内容。 </param>
    /// <returns> 当前构建器。 </returns>
    public EmbedBuilder WithPrompt(string prompt)
    {
        Prompt = prompt;
        return this;
    }

    /// <summary>
    ///     设置嵌入式消息的缩略图 URL。
    /// </summary>
    /// <param name="thumbnailUrl"> 要设置的缩略图 URL。 </param>
    /// <returns> 当前构建器。 </returns>
    public EmbedBuilder WithThumbnailUrl(string thumbnailUrl)
    {
        ThumbnailUrl = thumbnailUrl;
        return this;
    }

    /// <summary>
    ///     添加一个字段到 <see cref="QQBot.Embed"/>。
    /// </summary>
    /// <param name="name"> 字段的名称。 </param>
    /// <returns> 当前构建器。 </returns>
    public EmbedBuilder AddField(string name)
    {
        EmbedFieldBuilder field = new EmbedFieldBuilder().WithName(name);
        AddField(field);
        return this;
    }

    /// <summary>
    ///     添加一个字段到 <see cref="QQBot.Embed"/>。
    /// </summary>
    /// <param name="field"> 要添加的字段。 </param>
    /// <returns> 当前构建器。 </returns>
    public EmbedBuilder AddField(EmbedFieldBuilder field)
    {
        Fields.Add(field);
        return this;
    }

    /// <summary>
    ///     添加一个字段到 <see cref="QQBot.Embed"/>。
    /// </summary>
    /// <param name="action"> 一个委托，用于配置字段构建器。 </param>
    /// <returns> 当前构建器。 </returns>
    public EmbedBuilder AddField(Action<EmbedFieldBuilder> action)
    {
        EmbedFieldBuilder field = new();
        action(field);
        AddField(field);
        return this;
    }

    /// <summary>
    ///     将此构建器构建为 <see cref="QQBot.Embed"/> 实例。
    /// </summary>
    /// <returns> 构建的嵌入式消息。 </returns>
    public Embed Build()
    {
        if (!string.IsNullOrEmpty(ThumbnailUrl))
            UrlValidation.Validate(ThumbnailUrl);
        return new Embed(Title, Prompt, _thumbnail, [..Fields.Select(x => x.Build())]);
    }

    /// <summary>
    ///     比较两个 <see cref="EmbedBuilder"/> 是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个 <see cref="EmbedBuilder"/>。 </param>
    /// <param name="right"> 要比较的第二个 <see cref="EmbedBuilder"/>。 </param>
    /// <returns> 如果两个 <see cref="EmbedBuilder"/> 相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public static bool operator ==(EmbedBuilder? left, EmbedBuilder? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     比较两个 <see cref="EmbedBuilder"/> 是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个 <see cref="EmbedBuilder"/>。 </param>
    /// <param name="right"> 要比较的第二个 <see cref="EmbedBuilder"/>。 </param>
    /// <returns> 如果两个 <see cref="EmbedBuilder"/> 不相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public static bool operator !=(EmbedBuilder? left, EmbedBuilder? right) => !(left == right);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="EmbedBuilder"/>.
    /// </summary>
    /// <remarks>
    /// If the object passes is an <see cref="EmbedBuilder"/>, <see cref="Equals(EmbedBuilder)"/> will be called to compare the 2 instances
    /// </remarks>
    /// <param name="obj">The object to compare with the current <see cref="EmbedBuilder"/></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
        => obj is EmbedBuilder embedBuilder && Equals(embedBuilder);

    /// <summary>
    /// Determines whether the specified <see cref="EmbedBuilder"/> is equal to the current <see cref="EmbedBuilder"/>
    /// </summary>
    /// <param name="embedBuilder">The <see cref="EmbedBuilder"/> to compare with the current <see cref="EmbedBuilder"/></param>
    /// <returns></returns>
    public bool Equals(EmbedBuilder? embedBuilder)
    {
        if (embedBuilder is null)
            return false;

        if (Fields.Count != embedBuilder.Fields.Count)
            return false;

        return Title == embedBuilder.Title
            && Prompt == embedBuilder.Prompt
            && _thumbnail == embedBuilder._thumbnail
            && Fields.SequenceEqual(embedBuilder.Fields);
    }

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}
