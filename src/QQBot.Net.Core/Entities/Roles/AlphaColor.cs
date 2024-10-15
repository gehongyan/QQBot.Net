using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using StandardColor = System.Drawing.Color;

namespace QQBot;

/// <summary>
///     表示 QQBot 中使用的带有不透明度通道的颜色。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct AlphaColor
{
    /// <summary>
    ///     获取一个 QQBot 中带有不透明度通道的颜色的最大值的原始值。
    /// </summary>
    public const uint MaxDecimalValue = 0xFFFFFFFF;

    /// <summary>
    ///     获取默认颜色。
    /// </summary>
    public static readonly AlphaColor Default = new(0xFF, Color.Default);

    /// <summary>
    ///     获取此颜色的原始值。
    /// </summary>
    /// <remarks>
    ///     颜色以 32 位无符号整型值 ARGB 格式进行编码，由高至低的每 8 位分别表示不透明度、红色、绿色和蓝色通道的强度。
    /// </remarks>
    public uint RawValue { get; }

    /// <summary>
    ///     获取此颜色的不透明度通道的强度。
    /// </summary>
    public byte A => (byte)(RawValue >> 24);

    /// <summary>
    ///     获取此颜色的红色通道的强度。
    /// </summary>
    public byte R => (byte)(RawValue >> 16);

    /// <summary>
    ///     获取此颜色的绿色通道的强度。
    /// </summary>
    public byte G => (byte)(RawValue >> 8);

    /// <summary>
    ///     获取此颜色的蓝色通道的强度。
    /// </summary>
    public byte B => (byte)RawValue;

    /// <summary>
    ///     获取此颜色不带有不透明度通道的基础颜色。
    /// </summary>
    public Color BaseColor => new(RawValue & Color.MaxDecimalValue);

    /// <summary>
    ///     使用指定的 32 位无符号整型值初始化一个 <see cref="AlphaColor"/> 结构的新实例。
    /// </summary>
    /// <example>
    ///     创建 #FF607D8B（http://www.color-hex.com/color/607d8b）所表示的颜色，且其完全不透明：
    ///     <code language="cs">
    ///         AlphaColor darkGrey = new AlphaColor(0xFF607D8B);
    ///     </code>
    /// </example>
    /// <param name="rawValue"> 颜色的 32 位无符号整型原始值。 </param>
    public AlphaColor(uint rawValue)
    {
        RawValue = rawValue;
    }

    /// <summary>
    ///     使用指定的 <see cref="QQBot.Color"/> 及不透明度初始化一个 <see cref="AlphaColor"/> 结构的新实例。
    /// </summary>
    /// <example>
    ///     创建 #FF607D8B（http://www.color-hex.com/color/607d8b）所表示的颜色，且其完全不透明：
    ///     <code language="cs">
    ///         AlphaColor darkGrey = new AlphaColor(new Color(0x607D8B), (byte)0xFF);
    ///     </code>
    /// </example>
    /// <param name="alpha"> 不透明度。 </param>
    /// <param name="baseColor"> 基础颜色。 </param>
    public AlphaColor(byte alpha, Color baseColor)
    {
        uint value = ((uint)alpha << 24)
            | ((uint)baseColor.R << 16)
            | ((uint)baseColor.G << 8)
            | ((uint)baseColor.B);

        RawValue = value;
    }

    /// <summary>
    ///     使用指定的 ARGB 通道值初始化一个 <see cref="AlphaColor" /> 结构的新实例。
    /// </summary>
    /// <example>
    ///     创建 #607D8B（http://www.color-hex.com/color/607d8b）所表示的颜色，且其完全不透明：
    ///     <code language="cs">
    ///     AlphaColor darkGrey = new AlphaColor((byte)0xFF, (byte)0x60, (byte)0x7D, (byte)0x8B);
    ///     </code>
    /// </example>
    /// <param name="a"> 不透明度通道的强度。 </param>
    /// <param name="r"> 红色通道的强度。 </param>
    /// <param name="g"> 绿色通道的强度。 </param>
    /// <param name="b"> 蓝色通道的强度。 </param>
    public AlphaColor(byte a, byte r, byte g, byte b)
    {
        uint value = ((uint)a << 24)
            | ((uint)r << 16)
            | ((uint)g << 8)
            | (uint)b;

        RawValue = value;
    }

    /// <summary>
    ///     使用指定的 ARGB 通道值初始化一个 <see cref="AlphaColor" /> 结构的新实例。
    /// </summary>
    /// <example>
    ///     创建 #607D8B（http://www.color-hex.com/color/607d8b）所表示的颜色，且其完全不透明：
    ///     <code language="cs">
    ///         AlphaColor darkGrey = new AlphaColor(255, 96, 125, 139);
    ///     </code>
    /// </example>
    /// <param name="a"> 不透明度通道的强度。 </param>
    /// <param name="r"> 红色通道的强度。 </param>
    /// <param name="g"> 绿色通道的强度。 </param>
    /// <param name="b"> 蓝色通道的强度。 </param>
    public AlphaColor(int a, int r, int g, int b)
    {
        if (a is < 0 or > 255)
            throw new ArgumentOutOfRangeException(nameof(a), "Value must be within [0,255].");

        if (r is < 0 or > 255)
            throw new ArgumentOutOfRangeException(nameof(r), "Value must be within [0,255].");

        if (g is < 0 or > 255)
            throw new ArgumentOutOfRangeException(nameof(g), "Value must be within [0,255].");

        if (b is < 0 or > 255)
            throw new ArgumentOutOfRangeException(nameof(b), "Value must be within [0,255].");

        RawValue = ((uint)a << 24)
            | ((uint)r << 16)
            | ((uint)g << 8)
            | (uint)b;
    }

    /// <summary>
    ///     使用指定的 ARGB 通道值初始化一个 <see cref="AlphaColor" /> 结构的新实例。
    /// </summary>
    /// <example>
    ///     创建 #607D8B（http://www.color-hex.com/color/607d8b）所表示的颜色，且其完全不透明：
    ///     <code language="cs">
    ///         AlphaColor darkGrey = new AlphaColor(1.00f, 0.38f, 0.49f, 0.55f);
    ///     </code>
    /// </example>
    /// <param name="a"> 不透明度通道的强度。 </param>
    /// <param name="r"> 红色通道的强度。 </param>
    /// <param name="g"> 绿色通道的强度。 </param>
    /// <param name="b"> 蓝色通道的强度。 </param>
    public AlphaColor(float a, float r, float g, float b)
    {
        if (a is < 0.0f or > 1.0f)
            throw new ArgumentOutOfRangeException(nameof(a), "Value must be within [0,1].");

        if (r is < 0.0f or > 1.0f)
            throw new ArgumentOutOfRangeException(nameof(r), "Value must be within [0,1].");

        if (g is < 0.0f or > 1.0f)
            throw new ArgumentOutOfRangeException(nameof(g), "Value must be within [0,1].");

        if (b is < 0.0f or > 1.0f)
            throw new ArgumentOutOfRangeException(nameof(b), "Value must be within [0,1].");

        RawValue = ((uint)(a * 255.0f) << 24)
            | ((uint)(r * 255.0f) << 16)
            | ((uint)(g * 255.0f) << 8)
            | (uint)(b * 255.0f);
    }

    /// <summary>
    ///     判定两个 <see cref="AlphaColor"/> 是否相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="AlphaColor"/> 相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(AlphaColor lhs, AlphaColor rhs) => lhs.RawValue == rhs.RawValue;

    /// <summary>
    ///     判定两个 <see cref="AlphaColor"/> 是否不相等。
    /// </summary>
    /// <returns> 如果两个 <see cref="AlphaColor"/> 不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(AlphaColor lhs, AlphaColor rhs) => lhs.RawValue != rhs.RawValue;

    /// <summary>
    ///     使用指定的 32 位无符号整型值初始化一个 <see cref="AlphaColor"/> 结构的新实例。
    /// </summary>
    /// <example>
    ///     创建 #607D8B（http://www.color-hex.com/color/607d8b）所表示的颜色，且其完全不透明：
    ///     <code language="cs">
    ///         AlphaColor darkGrey = 0xFF607D8B;
    ///     </code>
    /// </example>
    /// <param name="rawValue"> 颜色的 32 位无符号整型原始值。 </param>
    public static implicit operator AlphaColor(uint rawValue) => new(rawValue);

    /// <inheritdoc cref="QQBot.AlphaColor.RawValue" />
    public static implicit operator uint(AlphaColor color) => color.RawValue;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is AlphaColor c && RawValue == c.RawValue;

    /// <inheritdoc />
    public override int GetHashCode() => RawValue.GetHashCode();

    /// <summary>
    ///     将由 QQBot.Net 定义的 <see cref="QQBot.Color"/> 颜色转换为 QQBot.Net 定义的 <see cref="QQBot.AlphaColor"/> 颜色。
    /// </summary>
    /// <param name="color"> 要进行转换的 QQBot.Net 定义的 <see cref="QQBot.Color"/> 颜色。 </param>
    /// <returns> 与该 QQBot.Net 定义的 <see cref="QQBot.Color"/> 颜色具有相同色值的 <see cref="QQBot.AlphaColor"/> 颜色。 </returns>
    public static implicit operator AlphaColor(Color color) =>
        new((color.RawValue << 8) | 0xFF);

    /// <inheritdoc cref="QQBot.AlphaColor.BaseColor" />
    /// <remarks>
    ///     <note type="warning">
    ///         此转换会丢失不透明度通道的信息。
    ///     </note>
    /// </remarks>
    public static explicit operator Color(AlphaColor color) => color.BaseColor;

    /// <summary>
    ///     将由 QQBot.Net 定义的 <see cref="QQBot.AlphaColor"/> 颜色转换为由 .NET 定义的 <see cref="System.Drawing.Color"/> 颜色。
    /// </summary>
    /// <param name="color"> 要进行转换的 <see cref="QQBot.AlphaColor"/> 颜色。 </param>
    /// <returns> 与该 <see cref="QQBot.AlphaColor"/> 颜色具有相同色值的 .NET <see cref="System.Drawing.Color"/> 颜色。 </returns>
    public static implicit operator StandardColor(AlphaColor color) =>
        StandardColor.FromArgb(color.A, color.R, color.G, color.B);

    /// <summary>
    ///     将由 .NET 定义的 <see cref="System.Drawing.Color"/> 颜色转换为由 QQBot.Net 定义的 <see cref="QQBot.AlphaColor"/> 颜色。
    /// </summary>
    /// <param name="color"> 要进行转换的 .NET <see cref="System.Drawing.Color"/> 颜色。 </param>
    /// <returns> 与该 .NET <see cref="System.Drawing.Color"/> 颜色具有相同色值的 <see cref="QQBot.AlphaColor"/> 颜色。 </returns>
    public static explicit operator AlphaColor(StandardColor color) =>
        new(color.A, color.R, color.G, color.B);

    /// <summary>
    ///     获取此颜色带有 <c>#</c> 前缀的 ARGB 十六进制字符串表示形式（例如 <c>#FF000CCC</c>）。
    /// </summary>
    /// <returns> 此颜色带有 <c>#</c> 前缀的 ARGB 十六进制字符串表示形式（例如 <c>#FF000CCC</c>）。 </returns>
    public override string ToString() => $"#{RawValue:X8}";

    private string DebuggerDisplay => $"#{RawValue:X8} ({RawValue})";
}
