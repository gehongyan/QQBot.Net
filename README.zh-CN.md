# QQBot.Net

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/gehongyan/QQBot.Net/push.yml?branch=master)
![GitHub Top Language](https://img.shields.io/github/languages/top/gehongyan/QQBot.Net)
[![Nuget Version](https://img.shields.io/nuget/v/QQBot.Net)](https://www.nuget.org/packages/QQBot.Net)
[![Nuget](https://img.shields.io/nuget/dt/QQBot.Net?color=%230099ff)](https://www.nuget.org/packages/QQBot.Net)
[![License](https://img.shields.io/github/license/gehongyan/QQBot.Net)](https://github.com/gehongyan/QQBot.Net/blob/master/LICENSE)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fgehongyan%2FQQBot.Net.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fgehongyan%2FQQBot.Net?ref=badge_shield)
<a href="https://chat.xiaoheihe.cn/iugh82ns">
    <img src="https://imgQQBot.max-c.com/oa/2023/03/21/47912df9f48f030c784dd6115b91274b.png" height="20" alt="Chat on QQBot"/>
</a>

---

[English](./README.md) | **简体中文**

---

**QQBot.Net** 是一个为 QQ 机器人 API [v1] 及 [v2] 提供的非官方 C# .NET SDK 实现。

[v1]: https://bot.q.qq.com/wiki/develop/api/
[v2]: https://bot.q.qq.com/wiki/develop/api-v2/

---

## 源码与文档

源代码提供在 [GitHub](https://github.com/gehongyan/QQBot.Net) 上。

可在 [qqbotnet.dev](https://qqbotnet.dev) 上查看文档。

---

## 目标框架

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)

> [!TIP]
> .NET 8 之外的目标框架尚未完全测试。

---

## 安装

### 主程序包

主程序包可以提供所有官方 API 的实现。

- QQBot.Net: [NuGet](https://www.nuget.org/packages/QQBot.Net/), [GitHub Packages](https://github.com/gehongyan/QQBot.Net/pkgs/nuget/QQBot.Net)

### 独立程序包

主程序包的各个组件可以单独安装，这些程序包包含在主程序包中。

- QQBot.Net.Core: [NuGet](https://www.nuget.org/packages/QQBot.Net.Core/),
  [GitHub Packages](https://github.com/gehongyan/QQBot.Net/pkgs/nuget/QQBot.Net.Core)
- QQBot.Net.Rest: [NuGet](https://www.nuget.org/packages/QQBot.Net.Rest/),
  [GitHub Packages](https://github.com/gehongyan/QQBot.Net/pkgs/nuget/QQBot.Net.Rest)
- QQBot.Net.WebSocket: [NuGet](https://www.nuget.org/packages/QQBot.Net.WebSocket/),
  [GitHub Packages](https://github.com/gehongyan/QQBot.Net/pkgs/nuget/QQBot.Net.WebSocket)

---

## 许可证与版权

此程序包是开源的，根据 [MIT 许可证](LICENSE) 授权。

QQBot.Net 参考了 **[Discord.Net](https://github.com/discord-net/Discord.Net)** 进行开发，本仓库中的部分代码版权归
[Discord.Net 贡献者](https://github.com/discord-net/Discord.Net/graphs/contributors) 所有，根据
[此许可证](https://github.com/discord-net/Discord.Net/blob/dev/LICENSE) 进行授权。

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fgehongyan%2FQQBot.Net.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fgehongyan%2FQQBot.Net?ref=badge_large)

---

## 致谢

<img src="./assets/Discord.Net_Logo.svg" alt="drawing" height="50"/>

特别感谢 [Discord.Net](https://github.com/discord-net/Discord.Net) 项目，感谢他们开发了如此优秀的项目。

<p>
  <img src="./assets/Rider_Icon.svg" height="50" alt="RiderIcon"/>
  <img src="./assets/ReSharper_Icon.png" height="50" alt="Resharper_Icon"/>
</p>

特别感谢 [JetBrains](https://www.jetbrains.com) 提供的免费许可证，使我们能使用他们出色的工具 -
[Rider](https://www.jetbrains.com/rider/) and [ReSharper](https://www.jetbrains.com/resharper/) -
来开发 QQBot.Net。
