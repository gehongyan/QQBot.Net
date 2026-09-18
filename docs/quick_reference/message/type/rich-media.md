---
uid: Guides.QuickReference.Message.Type.RichMedia
title: 富媒体上传
---

# 富媒体上传

预声明变量

```csharp
readonly QQBotSocketClient _socketClient = null;

IUserChannel userChannel = null;   // 单聊
IGroupChannel groupChannel = null; // 群聊

Stream stream = null;
ReadOnlyMemory<byte> memory = default;
```

QQBot.Net 通过统一的 <xref:QQBot.IMediaUploadChannel.UploadMediaAsync*> 处理富媒体上传：
该方法自动完成预上传、分片上传与分片合并的全流程，无需分别调用预上传与分片完成接口。
单聊（<xref:QQBot.IUserChannel>）与群聊（<xref:QQBot.IGroupChannel>）均实现
<xref:QQBot.IMediaUploadChannel>，调用方式一致。

### 上传富媒体

```csharp
// 上传源支持 4 种构造方式（均可指定 AttachmentType 与文件名）：
MediaUploadSource fromFile = MediaUploadSource.FromFile("path/to/image.png", AttachmentType.Image);
MediaUploadSource fromStream = MediaUploadSource.FromStream(stream, "image.png", AttachmentType.Image); // 流须可读可定位
MediaUploadSource fromUri = MediaUploadSource.FromUri(new Uri("https://example.com/i.png"), AttachmentType.Image); // 由 QQ 平台转存，本地不下载
MediaUploadSource fromMemory = MediaUploadSource.FromMemory(memory, "image.png", AttachmentType.Image);

IProgress<MediaUploadProgress> progress = null;   // 可选，已确认分片的进度接收器
MediaUploadOptions uploadOptions = null;          // 可选，自动上传配置

// 单聊富媒体上传（预上传 + 分片上传 + 分片合并，全自动）
MediaUploadResult c2cResult = await userChannel.UploadMediaAsync(fromFile, progress, uploadOptions);
// 群聊富媒体上传（预上传 + 分片上传 + 分片合并，全自动）
MediaUploadResult groupResult = await groupChannel.UploadMediaAsync(fromFile, progress, uploadOptions);

// 上传结果可直接作为附件发送
await userChannel.SendMessageAsync(attachment: c2cResult.Attachment);
```

> 说明：QQ 官方将富媒体上传分为「上传 / 预上传 / 分片上传完成」三个分步接口，并分单聊与群聊两套，
> QQBot.Net 将这六种情况统一封装为 `UploadMediaAsync`，分片细节由 SDK 按服务端下发的并发和重试配置自动管理。
> 本页对应的官方接口如下：
> [单聊富媒体上传]、[单聊富媒体预上传]、[单聊分片上传完成]、[群聊富媒体上传]、[群聊富媒体预上传]、[群聊分片上传完成]。

[单聊富媒体上传]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_users_user_openid_files.post.html
[单聊富媒体预上传]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_users_user_id_upload_prepare.post.html
[单聊分片上传完成]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_users_user_id_upload_part_finish.post.html
[群聊富媒体上传]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_openid_files.post.html
[群聊富媒体预上传]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_id_upload_prepare.post.html
[群聊分片上传完成]: https://bot.q.qq.com/wiki/develop/api-v2/autogen/api/v2_groups_group_id_upload_part_finish.post.html
