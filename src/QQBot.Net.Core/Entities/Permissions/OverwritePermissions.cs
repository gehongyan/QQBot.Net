using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一组权限重写配置。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct OverwritePermissions
{
    /// <summary>
    ///     获取一个空的 <see cref="OverwritePermissions"/>，继承所有权限。
    /// </summary>
    public static OverwritePermissions InheritAll { get; } = new();

    /// <summary>
    ///     获取一个在权限重写配置中为指定频道重写允许所有权限的 <see cref="OverwritePermissions"/>。
    /// </summary>
    /// <exception cref="ArgumentException"> 未知的频道类型。 </exception>
    public static OverwritePermissions AllowAll(IChannel channel) =>
        new(ChannelPermissions.All(channel).RawValue, 0);

    /// <summary>
    ///     获取一个在权限重写配置中为指定频道重写禁止所有权限的 <see cref="OverwritePermissions"/>。
    /// </summary>
    /// <exception cref="ArgumentException"> 未知的频道类型。 </exception>
    public static OverwritePermissions DenyAll(IChannel channel) =>
        new(0, ChannelPermissions.All(channel).RawValue);

    /// <summary>
    ///     获取一个表示此重写中所有允许的权限的原始值。
    /// </summary>
    public ulong AllowValue { get; }

    /// <summary>
    ///     获取一个表示此重写中所有禁止的权限的原始值。
    /// </summary>
    public ulong DenyValue { get; }

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="QQBot.ChannelPermission.ViewChannel"/> 的重写配置。
    /// </summary>
    public PermValue ViewChannel => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.ViewChannel);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="QQBot.ChannelPermission.ManageChannels"/> 的重写配置。
    /// </summary>
    public PermValue ManageChannels => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.ManageChannels);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="QQBot.ChannelPermission.SendMessages"/> 的重写配置。
    /// </summary>
    public PermValue SendMessages => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.SendMessages);

    /// <summary>
    ///     获取此权限重写配置对频道权限位 <see cref="QQBot.ChannelPermission.Stream"/> 的重写配置。
    /// </summary>
    public PermValue Stream => Permissions.GetValue(AllowValue, DenyValue, ChannelPermission.Stream);

    /// <summary>
    ///     使用指定的原始值初始化一个 <see cref="OverwritePermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="allowValue"> 重写允许的权限的原始值。 </param>
    /// <param name="denyValue"> 重写禁止的权限的原始值。 </param>
    public OverwritePermissions(ulong allowValue, ulong denyValue)
    {
        AllowValue = allowValue;
        DenyValue = denyValue;
    }

    private OverwritePermissions(ulong allowValue, ulong denyValue,
        PermValue? viewChannel = null,
        PermValue? manageChannels = null,
        PermValue? sendMessages = null,
        PermValue? stream = null)
    {
        Permissions.SetValue(ref allowValue, ref denyValue, viewChannel, ChannelPermission.ViewChannel);
        Permissions.SetValue(ref allowValue, ref denyValue, manageChannels, ChannelPermission.ManageChannels);
        Permissions.SetValue(ref allowValue, ref denyValue, sendMessages, ChannelPermission.SendMessages);
        Permissions.SetValue(ref allowValue, ref denyValue, stream, ChannelPermission.Stream);

        AllowValue = allowValue;
        DenyValue = denyValue;
    }

    /// <summary>
    ///     使用指定的权限重写信息创建一个 <see cref="OverwritePermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="viewChannel"> 查看文字与语音频道。 </param>
    /// <param name="manageChannels"> 频道管理。 </param>
    /// <param name="sendMessages"> 发送文字消息。 </param>
    /// <param name="stream"> 发起直播。 </param>
    public OverwritePermissions(
        PermValue viewChannel = PermValue.Inherit,
        PermValue manageChannels = PermValue.Inherit,
        PermValue sendMessages = PermValue.Inherit,
        PermValue stream = PermValue.Inherit)
        : this(0, 0, viewChannel, manageChannels, sendMessages, stream)
    {
    }

    /// <summary>
    ///     以当前权限重写配置为基础，更改指定的重写，返回一个 <see cref="OverwritePermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="viewChannel"> 查看文字与语音频道。 </param>
    /// <param name="manageChannels"> 频道管理。 </param>
    /// <param name="sendMessages"> 发送文字消息。 </param>
    /// <param name="stream"> 发起直播。 </param>
    /// <returns> 更改了指定权限的新的权限集。 </returns>
    public OverwritePermissions Modify(
        PermValue? viewChannel = null,
        PermValue? manageChannels = null,
        PermValue? sendMessages = null,
        PermValue? stream = null) =>
        new(AllowValue, DenyValue, viewChannel, manageChannels, sendMessages, stream);

    /// <summary>
    ///     获取一个包含当前权限重写配置所包含的所有重写允许的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合。
    /// </summary>
    /// <returns> 一个包含当前权限重写配置所包含的所有重写允许的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合；如果当前权限重写配置未包含任何重写允许的权限位，则会返回一个空集合。 </returns>
    public List<ChannelPermission> ToAllowList()
    {
        List<ChannelPermission> perms = [];
        for (byte i = 0; i < Permissions.MaxBits; i++)
        {
            // first operand must be long or ulong to shift >31 bits
            ulong flag = (ulong)1 << i;
            if ((AllowValue & flag) != 0)
                perms.Add((ChannelPermission)flag);
        }

        return perms;
    }

    /// <summary>
    ///     获取一个包含当前权限重写配置所包含的所有重写禁止的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合。
    /// </summary>
    /// <returns> 一个包含当前权限重写配置所包含的所有重写禁止的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合；如果当前权限重写配置未包含任何重写禁止的权限位，则会返回一个空集合。 </returns>
    public List<ChannelPermission> ToDenyList()
    {
        List<ChannelPermission> perms = new();
        for (byte i = 0; i < Permissions.MaxBits; i++)
        {
            ulong flag = (ulong)1 << i;
            if ((DenyValue & flag) != 0)
                perms.Add((ChannelPermission)flag);
        }

        return perms;
    }

    /// <summary>
    ///     获取此权限重写配置所重写允许与重写禁止的权限的原始值的字符串表示。
    /// </summary>
    /// <returns> 此权限重写配置所重写允许与重写禁止的权限的原始值的字符串表示。 </returns>
    public override string ToString() => $"Allow {AllowValue}, Deny {DenyValue}";

    private string DebuggerDisplay =>
        $"Allow {string.Join(", ", ToAllowList())}, " + $"Deny {string.Join(", ", ToDenyList())}";
}
