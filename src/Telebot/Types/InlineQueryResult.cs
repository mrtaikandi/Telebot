namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// A base class to represent one result of an inline query.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of this result. Must be less than 64 bytes.
        /// </summary>
        [Required]
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the message to be sent.
        /// </summary>
        [JsonProperty("input_message_content")]
        public virtual InputMessageContent MessageContent { get; set; }

        /// <summary>
        /// Gets or sets the inline keyboard attached to the message (Optional).
        /// </summary>
        [JsonProperty("reply_markup")]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>Gets the type of the result.</summary>
        [Required]
        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public abstract InlineQueryResultType Type { get; }

        #endregion
    }
}