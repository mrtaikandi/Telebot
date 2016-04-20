namespace Taikandi.Telebot.Types
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides all available types of a <see cref="InlineQueryResult" />.
    /// </summary>
    public enum InlineQueryResultType
    {
        /// <summary>
        /// Indicates that the type of <see cref="InlineQueryResult" /> is not specified or is unknown.
        /// </summary>
        None = 0, 

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultArticle" />.
        /// </summary>
        [EnumMember(Value = "article")]
        Article, 

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultPhoto" />.
        /// </summary>
        [EnumMember(Value = "photo")]
        Photo, 

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultGif" />.
        /// </summary>
        [EnumMember(Value = "gif")]
        Gif, 

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultMpeg4Gif" />.
        /// </summary>
        [EnumMember(Value = "mpeg4_gif")]
        Mpeg4Gif, 

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultVideo" />.
        /// </summary>
        [EnumMember(Value = "video")]
        Video,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultAudio" />.
        /// </summary>
        [EnumMember(Value = "audio")]
        Audio,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultContact" />.
        /// </summary>
        [EnumMember(Value = "contact")]
        Contact,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultDocument" />.
        /// </summary>
        [EnumMember(Value = "document")]
        Document,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultLocation" />.
        /// </summary>
        [EnumMember(Value = "location")]
        Location,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultVenue" />.
        /// </summary>
        [EnumMember(Value = "venue")]
        Venue,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultVoice" />.
        /// </summary>
        [EnumMember(Value = "voice")]
        Voice,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult" /> is an <see cref="InlineQueryResultCachedSticker" />.
        /// </summary>
        [EnumMember(Value = "sticker")]
        Sticker
    }
}