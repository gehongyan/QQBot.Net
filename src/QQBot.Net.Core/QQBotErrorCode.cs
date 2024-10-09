#pragma warning disable CS1591

namespace QQBot;

/// <summary>
///     表示从 QQBot 接收到的错误代码。
/// </summary>
public enum QQBotErrorCode
{
    GeneralError = 0,

    #region Unknown Errors (10XXX)

    UnknownAccount = 10001,
    UnknownChannel = 10003,
    UnknownGuild = 10004,

    #endregion

    #region Checking Errors (11XXX)

    ErrorCheckAdminFailed = 11281,
    ErrorCheckAdminNotPass = 11282,
    ErrorWrongAppid = 11251,
    ErrorCheckAppPrivilegeFailed = 11252,
    ErrorCheckAppPrivilegeNotPass = 11253,
    ErrorInterfaceForbidden = 11254,
    InvalidRequest = 11255,
    ErrorWrongAppidDuplicate = 11261, // Same as 11251
    ErrorCheckRobot = 11262,
    ErrorCheckGuildAuth = 11263,
    ErrorGuildAuthNotPass = 11264,
    ErrorRobotHasBaned = 11265,
    ErrorWrongToken = 11241,
    ErrorCheckTokenFailed = 11242,
    ErrorCheckTokenNotPass = 11243,
    ErrorCheckUserAuth = 11273,
    ErrorUserAuthNotPass = 11274,
    ErrorWrongAppidTriple = 11275, // Same as 11251
    ErrorGetHTTPHeader = 11301,
    ErrorGetHeaderUIN = 11302,
    ErrorGetNick = 11303,
    ErrorGetAvatar = 11304,
    ErrorGetGuildId = 11305,
    ErrorGetGuildInfo = 11306,

    #endregion

    #region Request and Response Errors (12XXX)

    ReplaceIdFailed = 12001,
    RequestInvalid = 12002,
    ResponseInvalid = 12003,

    #endregion

    #region Ratelimit Errors (20XXX)

    ChannelHitWriteRateLimit = 20028,
    MessageLimitExceeded = 22009,

    #endregion

    #region Markdown Errors (50XXX)

    CannotSendEmptyMessage = 50006,
    InvalidFormBody = 50035,
    InvalidMarkdownCombination = 50037,
    DifferentChannelOrSubChannel = 50038,
    GetMessageFailed = 50039,
    MessageTemplateTypeError = 50040,
    MarkdownHasEmptyValue = 50041,
    MarkdownListMaxLength = 50042,
    GuildIdConversionFailed = 50043,
    CannotReplyToOwnMessage = 50045,
    NotAtBotMessage = 50046,
    NotBotOrAtBotMessage = 50047,
    MessageIdCannotBeNull = 50048,
    CanOnlyEditMessageWithKeyboard = 50049,
    KeyboardElementCannotBeNull = 50050,
    CanOnlyEditOwnMessage = 50051,
    EditMessageError = 50053,
    MarkdownTemplateParameterError = 50054,
    InvalidMarkdownContent = 50055,
    SendingMarkdownContentNotAllowed = 50056,
    MarkdownParameterSupportsNativeOrTemplate = 50057,

    #endregion

    #region SubChannel Permission Errors (301XXX)

    SubChannelInvalidParameter = 301000,
    QuerySubChannelInfoError = 301001,
    QuerySubChannelPermissionError = 301002,
    ModifySubChannelPermissionError = 301003,
    PrivateSubChannelMemberLimitReached = 301004,
    RpcServiceCallFailed = 301005,
    NonMemberQueryPermissionDenied = 301006,
    ParameterExceedsLimit = 301007,

    #endregion

    #region Schedule Errors (302XXX)

    ScheduleInvalidParameter = 302000,
    QueryChannelInfoError = 302001,
    QueryScheduleListFailed = 302002,
    QueryScheduleFailed = 302003,
    ModifyScheduleFailed = 302004,
    DeleteScheduleFailed = 302005,
    CreateScheduleFailed = 302006,
    GetCreatorInfoFailed = 302007,
    SubChannelIdCannotBeNull = 302008,
    ChannelSystemError = 302009,
    NoPermissionToModifySchedule = 302010,
    ScheduleDeleted = 302011,
    DailyScheduleLimitReached = 302012,
    ScheduleCreationTriggeredSecurity = 302013,
    ScheduleDurationExceedsLimit = 302014,
    StartTimeCannotBeEarlierThanCurrent = 302015,
    EndTimeCannotBeEarlierThanStartTime = 302016,
    ScheduleObjectIsNull = 302017,
    ParameterTypeConversionFailed = 302018,
    DownstreamCallFailed = 302019,
    ScheduleContentOrAccountViolation = 302020,
    DailyActivityLimitReached = 302021,
    CannotBindSubChannelFromDifferentChannel = 302022,
    StartJumpCannotBindScheduleSubChannel = 302023,
    BoundSubChannelDoesNotExist = 302024,

    #endregion

    #region Message Errors (304XXX)

    UrlNotAllowed = 304003,
    ArkNotAllowed = 304004,
    EmbedLimit = 304005,
    ServerConfigError = 304006,
    GetGuildError = 304007,
    GetBotError = 304008,
    GetChannelError = 304009,
    ChangeImageUrlError = 304010,
    NoTemplate = 304011,
    GetTemplateError = 304012,
    TemplatePrivilegeError = 304014,
    SendMessageError = 304016,
    UploadImageError = 304017,
    SessionNotExist = 304018,
    AtEveryoneTimesLimit = 304019,
    FileSizeLimit = 304020,
    GetFileError = 304021,
    PushTimeLimit = 304022,
    PushMessageAsyncOk = 304023,
    ReplyMessageAsyncOk = 304024,
    MessageBeat = 304025,
    MessageIdError = 304026,
    MessageExpire = 304027,
    MessageProtectError = 304028,
    CorpusError = 304029,
    CorpusNotMatch = 304030,
    PrivateMessageClosed = 304031,
    PrivateMessageNotExist = 304032,
    PullPrivateMessageError = 304033,
    NotPrivateMessageMember = 304034,
    PushMessageExceedsSubChannelLimit = 304035,
    NoMarkdownTemplatePermission = 304036,
    NoMessageButtonComponentPermission = 304037,
    MessageButtonComponentNotExist = 304038,
    MessageButtonComponentParseError = 304039,
    MessageButtonComponentContentError = 304040,
    GetMessageSettingsError = 304044,
    SubChannelActiveMessageLimit = 304045,
    ActiveMessageNotAllowedInSubChannel = 304046,
    ActiveMessagePushExceedsSubChannelLimit = 304047,
    ActiveMessageNotAllowedInChannel = 304048,
    PrivateMessageActiveLimit = 304049,
    PrivateMessageTotalLimit = 304050,
    MessageSettingsGuideRequestError = 304051,
    SendMessageSettingsGuideOverLimit = 304052,
    CustomKeyboardNotAllowed = 304057,
    FetchUploadedMediaInfoFailed = 304082,
    ConvertMediaInfoFailed = 304083,

    #endregion

    #region Message Retraction Errors (306XXX)

    RetractMessageInvalidParameter = 306001,
    RetractMessageIdError = 306002,
    RetractMessageGetMessageFailed = 306003,
    RetractMessageNoPermission = 306004,
    RetractMessageFailed = 306005,
    RetractMessageGetChannelFailed = 306006,
    RetractMessageNotInCurrentGuild = 306007,
    RetractMessageNotSentByCurrentBot = 306008,
    RetractMessageNotSentByCurrentUser = 306009,
    RetractMessageInternalError = 306010,
    RetractMessageTimeExceeded = 306011,

    #endregion

    #region Announcement Errors (501XXX)

    AnnouncementError = 501000,
    ParameterValidationFailed = 501001,
    CreateSubChannelAnnouncementFailed = 501002,
    DeleteSubChannelAnnouncementFailed = 501003,
    GetChannelInfoFailed = 501004,
    AnnouncementMessageIdError = 501005,
    CreateGlobalChannelAnnouncementFailed = 501006,
    DeleteGlobalChannelAnnouncementFailed = 501007,
    MessageIdNotExist = 501008,
    MessageIdParseFailed = 501009,
    MessageNotInSubChannel = 501010,
    CreatePinnedMessageFailed = 501011,
    DeletePinnedMessageFailed = 501012,
    PinnedMessageExceedsLimit = 501013,
    AnnouncementSecurityHit = 501014,
    MessageNotAllowedToSet = 501015,
    ChannelAnnouncementSubChannelRecommendationExceedsLimit = 501016,
    NotChannelOwnerOrAdmin = 501017,
    InvalidRecommendedSubChannelId = 501018,
    AnnouncementTypeError = 501019,
    CreateRecommendedSubChannelTypeAnnouncementFailed = 501020,

    #endregion

    #region Mute Errors (502XXX)

    MuteError = 502000,
    InvalidChannelId = 502001,
    ChannelIdIsNull = 502002,
    InvalidUserId = 502003,
    UserIdIsNull = 502004,
    InvalidTimestamp = 502005,
    TimestampIsNull = 502006,
    ParameterConversionError = 502007,
    RpcCallFailed = 502008,
    MuteSecurityHit = 502009,
    RequestHeaderError = 502010,

    #endregion

    #region Post Errors (503XXX)

    PostInvalidChannelId = 503001,
    PostChannelIdIsNull = 503002,
    GetSubChannelInfoFailed = 503003,
    PostFrequencyLimitExceeded = 503004,
    PostTitleIsNull = 503005,
    PostContentIsNull = 503006,
    PostIdIsNull = 503007,
    GetXUinFailed = 503008,
    InvalidOrIllegalPostId = 503009,
    GetTinyIdByUinFailed = 503010,
    InvalidOrIllegalPostIdTimestamp = 503011,
    PostNotExistOrDeleted = 503012,
    InternalServerError = 503013,
    PostJsonContentParseFailed = 503014,
    PostContentConversionFailed = 503015,
    LinkCountExceeded = 503016,
    WordCountExceeded = 503017,
    ImageCountExceeded = 503018,
    VideoCountExceeded = 503019,
    TitleLengthExceeded = 503020,

    #endregion

    #region Message Frequency Errors (504XXX)

    MessageFrequencyError = 504000,
    InvalidRequestParameter = 504001,
    MessageFrequencyGetHttpHeaderFailed = 504002,
    GetBotUinError = 504003,
    GetMessageFrequencySettingsError = 504004,

    #endregion

    #region Channel Permission Errors (610XXX)

    ChannelPermissionError = 610000,
    GetChannelIdFailed = 610001,
    ChannelPermissionGetHttpHeaderFailed = 610002,
    GetBotUinFailed = 610003,
    GetBotRoleFailed = 610004,
    GetBotRoleInternalError = 610005,
    FetchBotPermissionListFailed = 610006,
    BotNotInChannel = 610007,
    InvalidParameter = 610008,
    GetApiInterfaceDetailsFailed = 610009,
    ApiInterfaceAuthorized = 610010,
    GetBotInfoFailed = 610011,
    RateLimitFailed = 610012,
    RateLimited = 610013,
    ApiAuthorizationLinkSendFailed = 610014,

    #endregion

    #region Emoji Reaction Errors (62XXXX)

    InvalidEmojiReactionParameter = 620001,
    EmojiReactionTypeLimitExceeded = 620002,
    EmojiReactionAlreadySet = 620003,
    EmojiReactionNotSet = 620004,
    NoPermissionToSetEmojiReaction = 620005,
    EmojiReactionRateLimited = 620006,
    EmojiReactionOperationFailed = 620007,

    #endregion

    #region Interaction Callback Data Update Errors (63XXXX)

    InvalidInteractionCallbackDataUpdateParameter = 630001,
    GetAppIdFailedForInteractionCallbackDataUpdate = 630002,
    InteractionCallbackDataAppIdMismatch = 630003,
    InternalStorageErrorForInteractionCallbackDataUpdate = 630004,
    InternalStorageReadErrorForInteractionCallbackDataUpdate = 630005,
    ReadRequestAppIdFailedForInteractionCallbackDataUpdate = 630006,
    InteractionCallbackDataTooLarge = 630007,

    #endregion

    #region Message Sending Errors (1XXXXXX)

    SecurityHitMessageRateLimited = 1100100,
    SecurityHitSensitiveContent = 1100101,
    SecurityHitNoNewFeatureAccess = 1100102,
    SecurityHit = 1100103,
    SecurityHitInvalidOrNonexistentGroup = 1100104,
    InternalSystemError = 1100300,
    CallerNotGroupMember = 1100301,
    GetSpecifiedChannelNameFailed = 1100302,
    NonAdminCannotSendMessageInHomeChannel = 1100303,
    AtTimesAuthFailed = 1100304,
    TinyIdToUinConversionFailed = 1100305,
    NotPrivateChannelMember = 1100306,
    NonWhitelistAppSubChannel = 1100307,
    TriggeredChannelRateLimit = 1100308,
    OtherError = 1100499,

    #endregion


    #region Message Editing Errors (3XXXXXX)

    SecurityHitMessageEditing = 3300006

    #endregion
}
