﻿using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Schedule
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("start_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Milliseconds)]
    public required DateTimeOffset StartTimestamp { get; set; }

    [JsonPropertyName("end_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Milliseconds)]
    public required DateTimeOffset EndTimestamp { get; set; }

    [JsonPropertyName("creator")]
    public required Member Creator { get; set; }

    [JsonPropertyName("jump_channel_id")]
    public required ulong? JumpChannelId { get; set; }

    [JsonPropertyName("remind_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required RemindType RemindType { get; set; }
}
