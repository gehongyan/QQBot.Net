namespace QQBot.Rest;

internal sealed class RestUserMessageStream : IUserMessageStream
{
    private readonly IUserChannel _channel;
    private readonly BaseQQBotClient _client;
    private readonly IUserMessage? _passiveSource;
    private readonly string? _eventId;
    private readonly bool _isWakeup;
    private readonly int _messageSequence;
    private readonly SemaphoreSlim _gate = new(1, 1);

    public string Id { get; private set; }
    public StreamMessageContentType ContentType { get; }
    public int NextIndex { get; private set; }
    public int? RemainingMessageLength { get; private set; }
    public MessageReference? ReplyReference { get; private set; }
    public bool IsCompleted { get; private set; }

    public RestUserMessageStream(IUserChannel channel, BaseQQBotClient client, StreamMessageContentType contentType,
        IUserMessage? passiveSource, string? eventId, bool isWakeup, int messageSequence,
        StreamMessageChunkResult initialResult)
    {
        _channel = channel;
        _client = client;
        ContentType = contentType;
        _passiveSource = passiveSource;
        _eventId = eventId;
        _isWakeup = isWakeup;
        _messageSequence = messageSequence;
        Id = initialResult.StreamMessageId;
        NextIndex = 1;
        RemainingMessageLength = initialResult.RemainingMessageLength;
        ReplyReference = initialResult.ReplyReference;
    }

    public Task<StreamMessageChunkResult> AppendAsync(string content, RequestOptions? options = null) =>
        SendAsync(content, StreamMessageInputState.Generating, options);

    public Task<StreamMessageChunkResult> CompleteAsync(string content, RequestOptions? options = null) =>
        SendAsync(content, StreamMessageInputState.Completed, options);

    private async Task<StreamMessageChunkResult> SendAsync(string content, StreamMessageInputState inputState,
        RequestOptions? options)
    {
        ArgumentNullException.ThrowIfNull(content);
        await _gate.WaitAsync().ConfigureAwait(false);
        try
        {
            if (IsCompleted)
                throw new InvalidOperationException("The stream message has already completed.");

            StreamMessageChunk chunk = new(content, ContentType, StreamMessageInputMode.Append,
                inputState, NextIndex, Id, _messageSequence);
            StreamMessageChunkResult result = await ChannelHelper.SendStreamMessageChunkAsync(
                    _channel, _client, chunk, _passiveSource, _eventId, _isWakeup, options)
                .ConfigureAwait(false);

            Id = result.StreamMessageId;
            NextIndex++;
            RemainingMessageLength = result.RemainingMessageLength;
            ReplyReference = result.ReplyReference;
            if (inputState == StreamMessageInputState.Completed)
                IsCompleted = true;
            return result;
        }
        finally
        {
            _gate.Release();
        }
    }
}
