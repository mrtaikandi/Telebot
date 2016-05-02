namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to a photo stored on the Telegram servers. By default, this photo will be sent by
    /// the user with an optional caption. Alternatively, you can use
    /// <see cref="InlineQueryResult.MessageContent" /> to send a message with the specified content
    /// instead of the photo.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    public class InlineQueryResultCachedPhoto : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the maximum of 200 character caption of the image file to be sent (Optional).
        /// </summary>
        [StringLength(200)]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the short description of the result (Optional).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a valid file identifier of the photo file.
        /// </summary>
        [Required]
        [JsonProperty("photo_file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>Gets or sets the title of the result (Optional).</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Photo;

        #endregion
    }
}