namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents one special entity in a text message. For example, hashtags, usernames, URLs, etc.
    /// </summary>
    public class MessageEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the length of the entity in UTF-16 code units.
        /// </summary>
        [Required]
        [JsonProperty("length", Required = Required.Always)]
        public long Length { get; set; }

        /// <summary>
        /// Gets or sets the offset in UTF-16 code units to the start of the entity.
        /// </summary>
        [Required]
        [JsonProperty("offset", Required = Required.Always)]
        public long Offset { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity. One of mention (@username), hashtag, bot_command, url, email,
        /// bold (bold text), italic (italic text), code (monowidth string), pre (monowidth block), text_link
        /// (for clickable text URLs).
        /// </summary>
        [Required]
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the URL that will be opened after user taps on the text. For "text_link" only.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        #endregion
    }
}