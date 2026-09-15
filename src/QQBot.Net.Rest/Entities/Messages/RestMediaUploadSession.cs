namespace QQBot.Rest;

internal sealed class RestMediaUploadSession : IMediaUploadSession
{
    private readonly IMediaUploadChannel _channel;
    private readonly BaseQQBotClient _client;
    private readonly MediaUploadDescriptor _descriptor;
    private readonly Dictionary<int, MediaUploadPart> _partsByIndex;
    private readonly HashSet<int> _confirmedParts = [];
    private readonly SemaphoreSlim _gate = new(1, 1);

    public string UploadId { get; }
    public long BlockSize { get; }
    public IReadOnlyList<MediaUploadPart> Parts { get; }
    public MediaUploadConfiguration Configuration { get; }

    public RestMediaUploadSession(IMediaUploadChannel channel, BaseQQBotClient client, MediaUploadDescriptor descriptor,
        string uploadId, long blockSize, IReadOnlyList<MediaUploadPart> parts, MediaUploadConfiguration configuration)
    {
        _channel = channel;
        _client = client;
        _descriptor = descriptor;
        UploadId = uploadId;
        BlockSize = blockSize;
        Parts = parts;
        Configuration = configuration;
        _partsByIndex = parts.ToDictionary(x => x.Index);
    }

    public async Task ConfirmPartAsync(MediaUploadPartCompletion completion, RequestOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(completion);
        if (!_partsByIndex.TryGetValue(completion.Index, out MediaUploadPart? part)
            || part.Length != completion.Length)
            throw new ArgumentException("The completion does not match a prepared upload part.", nameof(completion));

        await _gate.WaitAsync(options?.CancellationToken ?? CancellationToken.None).ConfigureAwait(false);
        try
        {
            if (_confirmedParts.Contains(completion.Index))
                return;
            await MediaUploadHelper.ConfirmPartAsync(_channel, _client, UploadId, completion, options)
                .ConfigureAwait(false);
            _confirmedParts.Add(completion.Index);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<MediaUploadResult> CompleteAsync(MediaUploadCompletionOptions? completionOptions = null,
        RequestOptions? options = null)
    {
        await _gate.WaitAsync(options?.CancellationToken ?? CancellationToken.None).ConfigureAwait(false);
        try
        {
            if (_confirmedParts.Count != Parts.Count)
                throw new InvalidOperationException("All upload parts must be confirmed before completing the upload.");
            return await MediaUploadHelper.CompleteAsync(_channel, _client, _descriptor, UploadId,
                    completionOptions ?? new MediaUploadCompletionOptions(), options)
                .ConfigureAwait(false);
        }
        finally
        {
            _gate.Release();
        }
    }
}