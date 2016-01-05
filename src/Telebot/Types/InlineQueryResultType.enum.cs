namespace Taikandi.Telebot.Types
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides all available types of a <see cref="InlineQueryResult"/>.
    /// </summary>
    public enum InlineQueryResultType
    {
        /// <summary>
        /// Indicates that the type of <see cref="InlineQueryResult" /> is not specified or is unknown.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult"/> is an <see cref="InlineQueryResultArticle"/>.
        /// </summary>
        [EnumMember(Value = "article")]
        Article,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult"/> is an <see cref="InlineQueryResultPhoto"/>.
        /// </summary>
        [EnumMember(Value = "photo")]
        Photo,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult"/> is an <see cref="InlineQueryResultGif"/>.
        /// </summary>
        [EnumMember(Value = "gif")]
        Gif,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult"/> is an <see cref="InlineQueryResultMpeg4Gif"/>.
        /// </summary>
        [EnumMember(Value = "mpeg4_gif")]
        Mpeg4Gif,

        /// <summary>
        /// Indicates that the <see cref="InlineQueryResult"/> is an <see cref="InlineQueryResultVideo"/>.
        /// </summary>
        [EnumMember(Value = "video")]
        Video,
    }
}