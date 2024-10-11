namespace QQBot;

/// <summary>
///     表示实体对象可以被删除。
/// </summary>
public interface IDeletable
{
    /// <summary>
    ///     删除此对实体象及其所有子实体对象。
    /// </summary>
    /// <param name="hideTip"> 是否隐藏删除提示，仅在 <see cref="ITextChannel"/> 或 <see cref="IDMChannel"/> 内的消息支持。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    Task DeleteAsync(bool? hideTip = null, RequestOptions? options = null);
}
