using System.Collections.Frozen;

namespace QQBot;

/// <summary>
///     提供受支持的表情符号。
/// </summary>
public static class Emotes
{
    /// <summary>
    ///     提供受支持的系统表情符号。
    /// </summary>
    public enum System
    {
        /// <summary> 得意 </summary>
        Proud = 4,

        /// <summary> 流泪 </summary>
        Tearful = 5,

        /// <summary> 睡 </summary>
        Sleep = 8,

        /// <summary> 大哭 </summary>
        Crying = 9,

        /// <summary> 尴尬 </summary>
        Embarrassed = 10,

        /// <summary> 调皮 </summary>
        Naughty = 12,

        /// <summary> 微笑 </summary>
        Smile = 14,

        /// <summary> 酷 </summary>
        Cool = 16,

        /// <summary> 可爱 </summary>
        Cute = 21,

        /// <summary> 傲慢 </summary>
        Arrogant = 23,

        /// <summary> 饥饿 </summary>
        Hungry = 24,

        /// <summary> 困 </summary>
        Sleepy = 25,

        /// <summary> 惊恐 </summary>
        Terrified = 26,

        /// <summary> 流汗 </summary>
        Sweating = 27,

        /// <summary> 憨笑 </summary>
        SillySmile = 28,

        /// <summary> 悠闲 </summary>
        Relaxed = 29,

        /// <summary> 奋斗 </summary>
        Struggling = 30,

        /// <summary> 疑问 </summary>
        Questioning = 32,

        /// <summary> 嘘 </summary>
        Shush = 33,

        /// <summary> 晕 </summary>
        Dizzy = 34,

        /// <summary> 敲打 </summary>
        Knocking = 38,

        /// <summary> 再见 </summary>
        Goodbye = 39,

        /// <summary> 发抖 </summary>
        Shivering = 41,

        /// <summary> 爱情 </summary>
        Love = 42,

        /// <summary> 跳跳 </summary>
        Jumping = 43,

        /// <summary> 拥抱 </summary>
        Hugging = 49,

        /// <summary> 蛋糕 </summary>
        Cake = 53,

        /// <summary> 咖啡 </summary>
        Coffee = 60,

        /// <summary> 玫瑰 </summary>
        Rose = 63,

        /// <summary> 爱心 </summary>
        Heart = 66,

        /// <summary> 太阳 </summary>
        Sun = 74,

        /// <summary> 月亮 </summary>
        Moon = 75,

        /// <summary> 赞 </summary>
        ThumbsUp = 76,

        /// <summary> 握手 </summary>
        Handshake = 78,

        /// <summary> 胜利 </summary>
        Victory = 79,

        /// <summary> 飞吻 </summary>
        FlyingKiss = 85,

        /// <summary> 西瓜 </summary>
        Watermelon = 89,

        /// <summary> 冷汗 </summary>
        ColdSweat = 96,

        /// <summary> 擦汗 </summary>
        WipingSweat = 97,

        /// <summary> 抠鼻 </summary>
        PickingNose = 98,

        /// <summary> 鼓掌 </summary>
        Clapping = 99,

        /// <summary> 糗大了 </summary>
        EmbarrassedBig = 100,

        /// <summary> 坏笑 </summary>
        EvilSmile = 101,

        /// <summary> 左哼哼 </summary>
        LeftHmph = 102,

        /// <summary> 右哼哼 </summary>
        RightHmph = 103,

        /// <summary> 哈欠 </summary>
        Yawn = 104,

        /// <summary> 委屈 </summary>
        Grievance = 106,

        /// <summary> 左亲亲 </summary>
        LeftKiss = 109,

        /// <summary> 可怜 </summary>
        Pitiful = 111,

        /// <summary> 示爱 </summary>
        ShowLove = 116,

        /// <summary> 抱拳 </summary>
        FistSalute = 118,

        /// <summary> 拳头 </summary>
        Fist = 120,

        /// <summary> 爱你 </summary>
        LoveYou = 122,

        /// <summary> NO </summary>
        No = 123,

        /// <summary> OK </summary>
        Ok = 124,

        /// <summary> 转圈 </summary>
        Spinning = 125,

        /// <summary> 挥手 </summary>
        Waving = 129,

        /// <summary> 喝彩 </summary>
        Cheers = 144,

        /// <summary> 棒棒糖 </summary>
        Lollipop = 147,

        /// <summary> 茶 </summary>
        Tea = 171,

        /// <summary> 泪奔 </summary>
        CryingRun = 173,

        /// <summary> 无奈 </summary>
        Helpless = 174,

        /// <summary> 卖萌 </summary>
        CuteAct = 175,

        /// <summary> 小纠结 </summary>
        LittleConfused = 176,

        /// <summary> doge </summary>
        Doge = 179,

        /// <summary> 惊喜 </summary>
        Surprise = 180,

        /// <summary> 骚扰 </summary>
        Harassment = 181,

        /// <summary> 笑哭 </summary>
        LaughCry = 182,

        /// <summary> 我最美 </summary>
        IAmTheBest = 183,

        /// <summary> 点赞 </summary>
        Like = 201,

        /// <summary> 托脸 </summary>
        FaceSupport = 203,

        /// <summary> 托腮 </summary>
        FacePalm = 212,

        /// <summary> 啵啵 </summary>
        KissKiss = 214,

        /// <summary> 蹭一蹭 </summary>
        RubRub = 219,

        /// <summary> 抱抱 </summary>
        Hug = 222,

        /// <summary> 拍手 </summary>
        ClapHands = 227,

        /// <summary> 佛系 </summary>
        Zen = 232,

        /// <summary> 喷脸 </summary>
        FaceSpray = 240,

        /// <summary> 甩头 </summary>
        HeadShake = 243,

        /// <summary> 加油抱抱 </summary>
        CheerUpHug = 246,

        /// <summary> 脑阔疼 </summary>
        Headache = 262,

        /// <summary> 捂脸 </summary>
        Facepalm = 264,

        /// <summary> 辣眼睛 </summary>
        EyeBurn = 265,

        /// <summary> 哦哟 </summary>
        OhMy = 266,

        /// <summary> 头秃 </summary>
        Bald = 267,

        /// <summary> 问号脸 </summary>
        QuestionFace = 268,

        /// <summary> 暗中观察 </summary>
        Observing = 269,

        /// <summary> emm </summary>
        Emm = 270,

        /// <summary> 吃瓜 </summary>
        EatingMelon = 271,

        /// <summary> 呵呵哒 </summary>
        Hehe = 272,

        /// <summary> 我酸了 </summary>
        IAmJealous = 273,

        /// <summary> 汪汪 </summary>
        WoofWoof = 277,

        /// <summary> 汗 </summary>
        Sweat = 278,

        /// <summary> 无眼笑 </summary>
        NoEyesSmile = 281,

        /// <summary> 敬礼 </summary>
        Salute = 282,

        /// <summary> 面无表情 </summary>
        Expressionless = 284,

        /// <summary> 摸鱼 </summary>
        Fishing = 285,

        /// <summary> 哦 </summary>
        Oh = 287,

        /// <summary> 睁眼 </summary>
        OpenEyes = 289,

        /// <summary> 敲开心 </summary>
        VeryHappy = 290,

        /// <summary> 摸锦鲤 </summary>
        TouchKoi = 293,

        /// <summary> 期待 </summary>
        Expectation = 294,

        /// <summary> 拜谢 </summary>
        ThankYou = 297,

        /// <summary> 元宝 </summary>
        Yuanbao = 298,

        /// <summary> 牛啊 </summary>
        Awesome = 299,

        /// <summary> 右亲亲 </summary>
        RightKiss = 305,

        /// <summary> 牛气冲天 </summary>
        Bullish = 306,

        /// <summary> 喵喵 </summary>
        MeowMeow = 307,

        /// <summary> 仔细分析 </summary>
        CarefulAnalysis = 314,

        /// <summary> 加油 </summary>
        CheerUp = 315,

        /// <summary> 崇拜 </summary>
        Worship = 318,

        /// <summary> 比心 </summary>
        HeartGesture = 319,

        /// <summary> 庆祝 </summary>
        Celebration = 320,

        /// <summary> 拒绝 </summary>
        Refuse = 322,

        /// <summary> 吃糖 </summary>
        EatCandy = 324,

        /// <summary> 生气 </summary>
        Angry = 326
    }

    /// <summary>
    ///     提供受支持的 QQ 表情符号。
    /// </summary>
    public enum Emoji
    {
        /// <summary> ☀ 晴天 </summary>
        Sun = 9728,

        /// <summary> ☕ 咖啡 </summary>
        HotBeverage = 9749,

        /// <summary> ☺ 可爱 </summary>
        SmilingFace = 9786,

        /// <summary> ✨ 闪光 </summary>
        Sparkles = 10024,

        /// <summary> ❌ 错误 </summary>
        CrossMark = 10060,

        /// <summary> ❔ 问号 </summary>
        WhiteQuestionMark = 10068,

        /// <summary> 🌹 玫瑰 </summary>
        Rose = 127801,

        /// <summary> 🍉 西瓜 </summary>
        Watermelon = 127817,

        /// <summary> 🍎 苹果 </summary>
        RedApple = 127822,

        /// <summary> 🍓 草莓 </summary>
        Strawberry = 127827,

        /// <summary> 🍜 拉面 </summary>
        SteamingBowl = 127836,

        /// <summary> 🍞 面包 </summary>
        Bread = 127838,

        /// <summary> 🍧 刨冰 </summary>
        ShavedIce = 127847,

        /// <summary> 🍺 啤酒 </summary>
        BeerMug = 127866,

        /// <summary> 🍻 干杯 </summary>
        ClinkingBeerMugs = 127867,

        /// <summary> 🎉 庆祝 </summary>
        PartyPopper = 127881,

        /// <summary> 🐛 虫 </summary>
        Bug = 128027,

        /// <summary> 🐮 牛 </summary>
        CowFace = 128046,

        /// <summary> 🐳 鲸鱼 </summary>
        SpoutingWhale = 128051,

        /// <summary> 🐵 猴 </summary>
        MonkeyFace = 128053,

        /// <summary> 👊 拳头 </summary>
        OncomingFist = 128074,

        /// <summary> 👌 好的 </summary>
        OkHand = 128076,

        /// <summary> 👍 厉害 </summary>
        ThumbsUp = 128077,

        /// <summary> 👏 鼓掌 </summary>
        ClappingHands = 128079,

        /// <summary> 👙 内衣 </summary>
        Bikini = 128089,

        /// <summary> 👦 男孩 </summary>
        Boy = 128102,

        /// <summary> 👨 爸爸 </summary>
        Man = 128104,

        /// <summary> 💓 爱心 </summary>
        BeatingHeart = 128147,

        /// <summary> 💝 礼物 </summary>
        HeartWithRibbon = 128157,

        /// <summary> 💤 睡觉 </summary>
        Zzz = 128164,

        /// <summary> 💦 水 </summary>
        SweatDroplets = 128166,

        /// <summary> 💨 吹气 </summary>
        DashingAway = 128168,

        /// <summary> 💪 肌肉 </summary>
        FlexedBiceps = 128170,

        /// <summary> 📫 邮箱 </summary>
        ClosedMailboxWithRaisedFlag = 128235,

        /// <summary> 🔥 火 </summary>
        Fire = 128293,

        /// <summary> 😁 呲牙 </summary>
        GrinningFaceWithSmilingEyes = 128513,

        /// <summary> 😂 激动 </summary>
        FaceWithTearsOfJoy = 128514,

        /// <summary> 😄 高兴 </summary>
        SmilingFaceWithOpenMouth = 128516,

        /// <summary> 😊 嘿嘿 </summary>
        SmilingFaceWithSmilingEyes = 128522,

        /// <summary> 😌 羞涩 </summary>
        RelievedFace = 128524,

        /// <summary> 😏 哼哼 </summary>
        SmirkingFace = 128527,

        /// <summary> 😒 不屑 </summary>
        UnamusedFace = 128530,

        /// <summary> 😓 汗 </summary>
        FaceWithColdSweat = 128531,

        /// <summary> 😔 失落 </summary>
        PensiveFace = 128532,

        /// <summary> 😘 飞吻 </summary>
        FaceBlowingAKiss = 128536,

        /// <summary> 😚 亲亲 </summary>
        KissingFaceWithClosedEyes = 128538,

        /// <summary> 😜 淘气 </summary>
        WinkingFaceWithTongue = 128540,

        /// <summary> 😝 吐舌 </summary>
        SquintingFaceWithTongue = 128541,

        /// <summary> 😭 大哭 </summary>
        LoudlyCryingFace = 128557,

        /// <summary> 😰 紧张 </summary>
        AnxiousFaceWithSweat = 128560,

        /// <summary> 😳 瞪眼 </summary>
        FlushedFace = 128563
    }

    internal static readonly FrozenDictionary<System, string> SystemNames = new Dictionary<System, string>
    {
        [System.Proud] = "得意",
        [System.Tearful] = "流泪",
        [System.Sleep] = "睡",
        [System.Crying] = "大哭",
        [System.Embarrassed] = "尴尬",
        [System.Naughty] = "调皮",
        [System.Smile] = "微笑",
        [System.Cool] = "酷",
        [System.Cute] = "可爱",
        [System.Arrogant] = "傲慢",
        [System.Hungry] = "饥饿",
        [System.Sleepy] = "困",
        [System.Terrified] = "惊恐",
        [System.Sweating] = "流汗",
        [System.SillySmile] = "憨笑",
        [System.Relaxed] = "悠闲",
        [System.Struggling] = "奋斗",
        [System.Questioning] = "疑问",
        [System.Shush] = "嘘",
        [System.Dizzy] = "晕",
        [System.Knocking] = "敲打",
        [System.Goodbye] = "再见",
        [System.Shivering] = "发抖",
        [System.Love] = "爱情",
        [System.Jumping] = "跳跳",
        [System.Hugging] = "拥抱",
        [System.Cake] = "蛋糕",
        [System.Coffee] = "咖啡",
        [System.Rose] = "玫瑰",
        [System.Heart] = "爱心",
        [System.Sun] = "太阳",
        [System.Moon] = "月亮",
        [System.ThumbsUp] = "赞",
        [System.Handshake] = "握手",
        [System.Victory] = "胜利",
        [System.FlyingKiss] = "飞吻",
        [System.Watermelon] = "西瓜",
        [System.ColdSweat] = "冷汗",
        [System.WipingSweat] = "擦汗",
        [System.PickingNose] = "抠鼻",
        [System.Clapping] = "鼓掌",
        [System.EmbarrassedBig] = "糗大了",
        [System.EvilSmile] = "坏笑",
        [System.LeftHmph] = "左哼哼",
        [System.RightHmph] = "右哼哼",
        [System.Yawn] = "哈欠",
        [System.Grievance] = "委屈",
        [System.LeftKiss] = "左亲亲",
        [System.Pitiful] = "可怜",
        [System.ShowLove] = "示爱",
        [System.FistSalute] = "抱拳",
        [System.Fist] = "拳头",
        [System.LoveYou] = "爱你",
        [System.No] = "NO",
        [System.Ok] = "OK",
        [System.Spinning] = "转圈",
        [System.Waving] = "挥手",
        [System.Cheers] = "喝彩",
        [System.Lollipop] = "棒棒糖",
        [System.Tea] = "茶",
        [System.CryingRun] = "泪奔",
        [System.Helpless] = "无奈",
        [System.CuteAct] = "卖萌",
        [System.LittleConfused] = "小纠结",
        [System.Doge] = "doge",
        [System.Surprise] = "惊喜",
        [System.Harassment] = "骚扰",
        [System.LaughCry] = "笑哭",
        [System.IAmTheBest] = "我最美",
        [System.Like] = "点赞",
        [System.FaceSupport] = "托脸",
        [System.FacePalm] = "托腮",
        [System.KissKiss] = "啵啵",
        [System.RubRub] = "蹭一蹭",
        [System.Hug] = "抱抱",
        [System.ClapHands] = "拍手",
        [System.Zen] = "佛系",
        [System.FaceSpray] = "喷脸",
        [System.HeadShake] = "甩头",
        [System.CheerUpHug] = "加油抱抱",
        [System.Headache] = "脑阔疼",
        [System.Facepalm] = "捂脸",
        [System.EyeBurn] = "辣眼睛",
        [System.OhMy] = "哦哟",
        [System.Bald] = "头秃",
        [System.QuestionFace] = "问号脸",
        [System.Observing] = "暗中观察",
        [System.Emm] = "emm",
        [System.EatingMelon] = "吃瓜",
        [System.Hehe] = "呵呵哒",
        [System.IAmJealous] = "我酸了",
        [System.WoofWoof] = "汪汪",
        [System.Sweat] = "汗",
        [System.NoEyesSmile] = "无眼笑",
        [System.Salute] = "敬礼",
        [System.Expressionless] = "面无表情",
        [System.Fishing] = "摸鱼",
        [System.Oh] = "哦",
        [System.OpenEyes] = "睁眼",
        [System.VeryHappy] = "敲开心",
        [System.TouchKoi] = "摸锦鲤",
        [System.Expectation] = "期待",
        [System.ThankYou] = "拜谢",
        [System.Yuanbao] = "元宝",
        [System.Awesome] = "牛啊",
        [System.RightKiss] = "右亲亲",
        [System.Bullish] = "牛气冲天",
        [System.MeowMeow] = "喵喵",
        [System.CarefulAnalysis] = "仔细分析",
        [System.CheerUp] = "加油",
        [System.Worship] = "崇拜",
        [System.HeartGesture] = "比心",
        [System.Celebration] = "庆祝",
        [System.Refuse] = "拒绝",
        [System.EatCandy] = "吃糖",
        [System.Angry] = "生气",
    }.ToFrozenDictionary();

    internal static readonly FrozenDictionary<Emoji, string> EmojiNames = new Dictionary<Emoji, string>
    {
        [Emoji.Sun] = "晴天",
        [Emoji.HotBeverage] = "咖啡",
        [Emoji.SmilingFace] = "可爱",
        [Emoji.Sparkles] = "闪光",
        [Emoji.CrossMark] = "错误",
        [Emoji.WhiteQuestionMark] = "问号",
        [Emoji.Rose] = "玫瑰",
        [Emoji.Watermelon] = "西瓜",
        [Emoji.RedApple] = "苹果",
        [Emoji.Strawberry] = "草莓",
        [Emoji.SteamingBowl] = "拉面",
        [Emoji.Bread] = "面包",
        [Emoji.ShavedIce] = "刨冰",
        [Emoji.BeerMug] = "啤酒",
        [Emoji.ClinkingBeerMugs] = "干杯",
        [Emoji.PartyPopper] = "庆祝",
        [Emoji.Bug] = "虫",
        [Emoji.CowFace] = "牛",
        [Emoji.SpoutingWhale] = "鲸鱼",
        [Emoji.MonkeyFace] = "猴",
        [Emoji.OncomingFist] = "拳头",
        [Emoji.OkHand] = "好的",
        [Emoji.ThumbsUp] = "厉害",
        [Emoji.ClappingHands] = "鼓掌",
        [Emoji.Bikini] = "内衣",
        [Emoji.Boy] = "男孩",
        [Emoji.Man] = "爸爸",
        [Emoji.BeatingHeart] = "爱心",
        [Emoji.HeartWithRibbon] = "礼物",
        [Emoji.Zzz] = "睡觉",
        [Emoji.SweatDroplets] = "水",
        [Emoji.DashingAway] = "吹气",
        [Emoji.FlexedBiceps] = "肌肉",
        [Emoji.ClosedMailboxWithRaisedFlag] = "邮箱",
        [Emoji.Fire] = "火",
        [Emoji.GrinningFaceWithSmilingEyes] = "呲牙",
        [Emoji.FaceWithTearsOfJoy] = "激动",
        [Emoji.SmilingFaceWithOpenMouth] = "高兴",
        [Emoji.SmilingFaceWithSmilingEyes] = "嘿嘿",
        [Emoji.RelievedFace] = "羞涩",
        [Emoji.SmirkingFace] = "哼哼",
        [Emoji.UnamusedFace] = "不屑",
        [Emoji.FaceWithColdSweat] = "汗",
        [Emoji.PensiveFace] = "失落",
        [Emoji.FaceBlowingAKiss] = "飞吻",
        [Emoji.KissingFaceWithClosedEyes] = "亲亲",
        [Emoji.WinkingFaceWithTongue] = "淘气",
        [Emoji.SquintingFaceWithTongue] = "吐舌",
        [Emoji.LoudlyCryingFace] = "大哭",
        [Emoji.AnxiousFaceWithSweat] = "紧张",
        [Emoji.FlushedFace] = "瞪眼",
    }.ToFrozenDictionary();
}
