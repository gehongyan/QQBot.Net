using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个子频道的权限集。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public struct ChannelPermissions
{
    /// <summary>
    ///     获取一个空的 <see cref="ChannelPermissions"/>，不包含任何权限。
    /// </summary>
    public static readonly ChannelPermissions None = new();

    /// <summary>
    ///     获取一个包含所有可以为文字子频道设置的权限的 <see cref="ChannelPermissions"/>。
    /// </summary>
    public static readonly ChannelPermissions Text = new(0b0111);

    /// <summary>
    ///     获取一个包含所有可以为直播子频道设置的权限的 <see cref="ChannelPermissions"/>。
    /// </summary>
    public static readonly ChannelPermissions LiveStream = new(0b1011);

    /// <summary>
    ///     为指定的子频道根据其类型获取一个包含所有权限的 <see cref="ChannelPermissions"/>。
    /// </summary>
    /// <param name="channel"> 要获取其包含所有权限的子频道。 </param>
    /// <returns> 一个包含所有该子频道可以拥有的权限的 <see cref="ChannelPermissions"/>。 </returns>
    /// <exception cref="ArgumentException"> 未知的子频道类型。 </exception>
    public static ChannelPermissions All(IChannel channel) =>
        channel switch
        {
            ILiveStreamChannel => LiveStream,
            IGuildChannel => Text,
            _ => throw new NotSupportedException("Not supported channel type.")
        };

    /// <summary>
    ///     获取此权限集的原始值。
    /// </summary>
    public ulong RawValue { get; }

    /// <summary>
    ///     获取此权限集是否允许相关用户查看文字与语音子频道。
    /// </summary>
    public bool ViewChannel => Permissions.GetValue(RawValue, ChannelPermission.ViewChannel);

    /// <summary>
    ///     获取此权限集是否允许相关用户管理子频道。
    /// </summary>
    public bool ManageChannels => Permissions.GetValue(RawValue, ChannelPermission.ManageChannels);

    /// <summary>
    ///     获取此权限集是否允许相关用户发送文字消息。
    /// </summary>
    public bool SendMessages => Permissions.GetValue(RawValue, ChannelPermission.SendMessages);

    /// <summary>
    ///     获取此权限集是否允许相关用户在发起直播。
    /// </summary>
    public bool Stream => Permissions.GetValue(RawValue, ChannelPermission.Stream);

    /// <summary>
    ///     使用指定的权限原始值创建一个 <see cref="ChannelPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="rawValue"> 权限原始值。 </param>
    public ChannelPermissions(ulong rawValue)
    {
        RawValue = rawValue;
    }

    private ChannelPermissions(ulong initialValue,
        bool? viewChannel = null,
        bool? manageChannels = null,
        bool? sendMessages = null,
        bool? stream = null)
    {
        ulong value = initialValue;

        Permissions.SetValue(ref value, viewChannel, ChannelPermission.ViewChannel);
        Permissions.SetValue(ref value, manageChannels, ChannelPermission.ManageChannels);
        Permissions.SetValue(ref value, sendMessages, ChannelPermission.SendMessages);
        Permissions.SetValue(ref value, stream, ChannelPermission.Stream);

        RawValue = value;
    }

    /// <summary>
    ///     使用指定的权限位信息创建一个 <see cref="ChannelPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="viewChannel"> 查看文字与语音子频道。 </param>
    /// <param name="manageChannels"> 子频道管理。 </param>
    /// <param name="sendMessages"> 发送文字消息。 </param>
    /// <param name="stream"> 发起直播。 </param>
    public ChannelPermissions(
        bool? viewChannel = false,
        bool? manageChannels = false,
        bool? sendMessages = false,
        bool? stream = false)
        : this(0, viewChannel, manageChannels, sendMessages, stream)
    {
    }

    /// <summary>
    ///     以当前权限集为基础，更改指定的权限，返回一个 <see cref="ChannelPermissions"/> 结构的新实例。
    /// </summary>
    /// <param name="viewChannel"> 查看文字与语音子频道。 </param>
    /// <param name="manageChannels"> 子频道管理。 </param>
    /// <param name="sendMessages"> 发送文字消息。 </param>
    /// <param name="stream"> 发起直播。 </param>
    /// <returns> 更改了指定权限的新的权限集。 </returns>
    public ChannelPermissions Modify(
        bool? viewChannel = null,
        bool? manageChannels = null,
        bool? sendMessages = null,
        bool? stream = null) =>
        new(RawValue,
            viewChannel,
            manageChannels,
            sendMessages,
            stream);

    /// <summary>
    ///     获取当前权限集是否包含指定的权限。
    /// </summary>
    /// <param name="permission"> 要检查的权限。 </param>
    /// <returns> 如果当前权限集包含了所有指定的权限信息，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public bool Has(ChannelPermission permission) => Permissions.GetValue(RawValue, permission);

    /// <summary>
    ///     获取一个包含当前权限集所包含的所有已设置的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合。
    /// </summary>
    /// <returns> 一个包含当前权限集所包含的所有已设置的 <see cref="ChannelPermission"/> 独立位标志枚举值的集合；如果当前权限集未包含任何已设置的权限位，则会返回一个空集合。 </returns>
    public List<ChannelPermission> ToList()
    {
        List<ChannelPermission> perms = [];

        // bitwise operations on raw value
        // each of the ChannelPermissions increments by 2^i from 0 to MaxBits
        for (byte i = 0; i < Permissions.MaxBits; i++)
        {
            ulong flag = (ulong)1 << i;
            if ((RawValue & flag) != 0)
                perms.Add((ChannelPermission)flag);
        }

        return perms;
    }

    /// <summary>
    ///     获取此权限集原始值的字符串表示。
    /// </summary>
    /// <returns> 此权限集原始值的字符串表示。 </returns>
    public override string ToString() => RawValue.ToString();

    private string DebuggerDisplay => $"{string.Join(", ", ToList())}";
}
