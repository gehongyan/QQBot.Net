# QQBot.Net

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/gehongyan/QQBot.Net/push.yml?branch=master)
![GitHub Top Language](https://img.shields.io/github/languages/top/gehongyan/QQBot.Net)
[![Nuget Version](https://img.shields.io/nuget/v/QQBot.Net)](https://www.nuget.org/packages/QQBot.Net)
[![Nuget](https://img.shields.io/nuget/dt/QQBot.Net?color=%230099ff)](https://www.nuget.org/packages/QQBot.Net)
[![License](https://img.shields.io/github/license/gehongyan/QQBot.Net)](https://github.com/gehongyan/QQBot.Net/blob/master/LICENSE)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fgehongyan%2FQQBot.Net.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fgehongyan%2FQQBot.Net?ref=badge_shield)
<a href="http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=QiRvU4EFUavyNKQLKyfymezkG9H46cY6&authKey=OnAwAOWySUTds7YJUhaiS%2Bpr%2FWYLKSIPAPzdnhsM4RgAgWRQKZywjc6RSEAnDfNM&noverify=0&group_code=849595128">
    <img src="https://imgQQBot.max-c.com/oa/2023/03/21/47912df9f48f030c784dd6115b91274b.png" height="20" alt="Chat on QQ"/>
</a>

---

**English** | [简体中文](./README.zh-CN.md)

---

**QQBot.Net** is an unofficial C# .NET implementation for QQ Bot API [v1] and [v2].

[v1]: https://bot.q.qq.com/wiki/develop/api/
[v2]: https://bot.q.qq.com/wiki/develop/api-v2/

---

## Source & Documentation

Source code is available on [GitHub](https://github.com/gehongyan/QQBot.Net).

Documents are available on [qqbotnet.dev](https://qqbotnet.dev). (Simplified Chinese available only)

---

## Targets

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)

> [!TIP]
> Targets other than .NET 8.0 have not been fully tested.

---

## Installation

### Main Package

The main package provides all implementations of official APIs.

- QQBot.Net: [NuGet](https://www.nuget.org/packages/QQBot.Net/), [GitHub Packages](https://github.com/gehongyan/QQBot.Net/pkgs/nuget/QQBot.Net)

### Individual Packages

Individual components of the main package can be installed separately. These packages are included in the main package.

- QQBot.Net.Core: [NuGet](https://www.nuget.org/packages/QQBot.Net.Core/),
  [GitHub Packages](https://github.com/gehongyan/QQBot.Net/pkgs/nuget/QQBot.Net.Core)
- QQBot.Net.Rest: [NuGet](https://www.nuget.org/packages/QQBot.Net.Rest/),
  [GitHub Packages](https://github.com/gehongyan/QQBot.Net/pkgs/nuget/QQBot.Net.Rest)
- QQBot.Net.WebSocket: [NuGet](https://www.nuget.org/packages/QQBot.Net.WebSocket/),
  [GitHub Packages](https://github.com/gehongyan/QQBot.Net/pkgs/nuget/QQBot.Net.WebSocket)

---

## License & Copyright

This package is open-source and is licensed under the [MIT license](LICENSE).

QQBot.Net was developed with reference to **[Discord.Net](https://github.com/discord-net/Discord.Net)**.

[Discord.Net contributors](https://github.com/discord-net/Discord.Net/graphs/contributors) holds the copyright
for portion of the code in this repository according to [this license](https://github.com/discord-net/Discord.Net/blob/dev/LICENSE).

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fgehongyan%2FQQBot.Net.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fgehongyan%2FQQBot.Net?ref=badge_large)

---

## Acknowledgements

<img src="./assets/Discord.Net_Logo.svg" alt="drawing" height="50"/>

Special thanks to [Discord.Net](https://github.com/discord-net/Discord.Net) for such a great project.

<p>
  <img src="./assets/Rider_Icon.svg" height="50" alt="RiderIcon"/>
  <img src="./assets/ReSharper_Icon.png" height="50" alt="Resharper_Icon"/>
</p>

Special thanks to [JetBrains](https://www.jetbrains.com) for providing free licenses for their awesome tools -
[Rider](https://www.jetbrains.com/rider/) and [ReSharper](https://www.jetbrains.com/resharper/) -
to develop QQBot.Net.
