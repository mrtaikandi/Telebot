namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    using Taikandi.Telebot.Converters;

    /// <summary>
    /// Represents the content of a text message to be sent as the result of an inline query.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InputMessageContent" />
    public class InputTextMessageContent : InputMessageContent
    {
        /// <summary>
        /// Gets or sets the text of the message to be sent, 1-4096 characters.
        /// </summary>
        [Required]
        [StringLength(4096, MinimumLength = 1)]
        [JsonProperty("message_text", Required = Required.Always)]
        public string MessageText { get; set; }

        /// <summary>
        /// Gets or sets the method that Telegram should send the content to the Telegram clients. 
        /// Use <see cref="T:ParseMode.Markdown"/>, if you want Telegram apps to show bold, italic, fixed-width text or 
        /// inline URLs in your bot's message.
        /// </summary>
        [JsonProperty("parse_mode")]
        [JsonConverter(typeof(ParseModeTypeConverter))]
        public ParseMode ParseMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disables link previews for links in the sent message.
        /// </summary>
        [JsonProperty("disable_web_page_preview")]
        public bool DisableWebPagePreview { get; set; }
    }
}