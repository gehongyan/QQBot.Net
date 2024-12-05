namespace QQBot;

/// <summary>
///     表示一个通用的嵌套子频道，即可以嵌套在分组子频道中的频道子频道。
/// </summary>
public interface INestedChannel : IGuildChannel
{
    /// <summary>
    ///     获取此嵌套子频道在子频道列表中所属的分组子频道的 ID。
    /// </summary>
    /// <remarks> 如果当前子频道不属于任何分组子频道，则会返回 <c>null</c>。 </remarks>
    ulong? CategoryId { get; }

    /// <summary>
    ///     获取此子频道的私密类型。
    /// </summary>
    PrivateType? PrivateType { get; }

    /// <summary>
    ///     获取此子频道的发言权限。
    /// </summary>
    SpeakPermission? SpeakPermission { get; }

    /// <summary>
    ///     获取当前用户在此子频道的权限。
    /// </summary>
    ChannelPermission? Permission { get; }

    /// <summary>
    ///     获取指定用户在此子频道的权限。
    /// </summary>
    /// <param name="user"> 要获取权限的用户。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 用户在此子频道的权限。 </returns>
    Task<ChannelPermissions> GetPermissionsAsync(IGuildMember user, RequestOptions? options = null);

    /// <summary>
    ///     获取指定角色在此子频道的权限。
    /// </summary>
    /// <param name="role"> 要获取权限的角色。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 角色在此子频道的权限。 </returns>
    Task<ChannelPermissions> GetPermissionsAsync(IRole role, RequestOptions? options = null);

    /// <summary>
    ///     修改指定用户在此子频道的权限。
    /// </summary>
    /// <param name="user"> 要修改权限的用户。 </param>
    /// <param name="permissions"> 要修改的权限。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改用户权限操作的任务。 </returns>
    Task ModifyPermissionsAsync(IGuildMember user, OverwritePermissions permissions, RequestOptions? options = null);

    /// <summary>
    ///     修改指定用户在此子频道的权限。
    /// </summary>
    /// <param name="role"> 要修改权限的角色。 </param>
    /// <param name="permissions"> 要修改的权限。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改用户权限操作的任务。 </returns>
    Task ModifyPermissionsAsync(IRole role, OverwritePermissions permissions, RequestOptions? options = null);
}
