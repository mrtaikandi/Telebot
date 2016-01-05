namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using Taikandi.Telebot.Converters;

    /// <summary>
    /// A base class to represent one result of an inline query.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to disable link previews for links in the sent message
        /// (Optional).
        /// </summary>
        [JsonProperty("disable_web_page_preview")]
        public bool DisableWebPagePreview { get; set; } = false;

        /// <summary>
        /// Gets or sets the unique identifier of this result. Must be less than 64 bytes.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicates the way that the Telegram should parse the sent message (Optional).
        /// </summary>
        [JsonProperty("parse_mode")]
        [JsonConverter(typeof(ParseModeTypeConverter))]
        public ParseMode ParseMode { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract InlineQueryResultType Type { get; }

        #endregion
    }
}