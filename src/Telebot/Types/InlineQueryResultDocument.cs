namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to a file. By default, this file will be sent by the user with an optional
    /// caption. Alternatively, you can use input_message_content to send a message with the specified
    /// content instead of the file. Currently, only .PDF and .ZIP files can be sent using this method.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore
    /// them.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultDocument : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the caption of the document to be sent, 0-200 characters (Optional).
        /// </summary>
        [StringLength(200)]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets a short description of the result (Optional).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Mime type of the content of the file, either “application/pdf” or
        /// “application/zip”.
        /// </summary>
        [Required]
        [JsonProperty("mime_type", Required = Required.Always)]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the height of the document thumbnail (Optional).
        /// </summary>
        [JsonProperty("thumb_height")]
        public string ThumbnailHeight { get; set; }

        /// <summary>
        /// Gets or sets the URL of the thumbnail (JPEG only) for the file (Optional).
        /// </summary>
        [JsonProperty("thumb_url")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the width of the document thumbnail (Optional).
        /// </summary>
        [JsonProperty("thumb_width")]
        public string ThumbnailWidth { get; set; }

        /// <summary>Gets or sets the title of the result.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>Gets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Document;

        /// <summary>Gets or sets a valid URL for the file.</summary>
        [Required]
        [JsonProperty("document_url", Required = Required.Always)]
        public string Url { get; set; }

        #endregion
    }
}