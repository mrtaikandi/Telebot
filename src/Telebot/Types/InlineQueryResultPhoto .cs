namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to a photo. By default, this photo will be sent by the user with optional
    /// caption. Alternatively, you can provide <see cref="MessageText" /> to send it instead of photo.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultPhoto : InlineQueryResult
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineQueryResultPhoto" /> class.
        /// </summary>
        public InlineQueryResultPhoto()
            : this(null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineQueryResultPhoto" /> class.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of this result. Must be less than 64 bytes.
        /// </param>
        /// <param name="url">
        /// A valid URL of the photo. Photo size must not exceed 5MB.
        /// </param>
        /// <param name="thumbnailUrl">
        /// The URL of the thumbnail for the result either in <c>.jpeg</c> or <c>.gif</c> format.
        /// </param>
        public InlineQueryResultPhoto(string id, string url, string thumbnailUrl)
        {
            ExceptionHelper.ValidateArgumentByteCount(nameof(id), id);

            this.Id = id;
            this.Url = url;
            this.ThumbnailUrl = thumbnailUrl;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the maximum of 200 character caption of the image file to be sent (Optional).
        /// </summary>
        [JsonProperty("caption")]
        [StringLength(200)]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the short description of the result (Optional).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Gets or sets the height of the photo (Optional).</summary>
        [JsonProperty("photo_height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the text of a message to be sent instead of the photo (Optional).
        /// </summary>
        [StringLength(512)]
        [JsonProperty("message_text")]
        public string MessageText { get; set; }

        /// <summary>
        /// Gets or sets the mime type of the photo, defaults to image/jpeg (Optional).
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the URL of the thumbnail for the result either in <c>.jpeg</c> or <c>.gif</c> format.
        /// </summary>
        [Required]
        [JsonProperty("thumb_url", Required = Required.Always)]
        public string ThumbnailUrl { get; set; }

        /// <summary>Gets or sets the title of the result (Optional).</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Photo;

        /// <summary>
        /// Gets or sets a valid URL of the photo. Photo size must not exceed 5MB.
        /// </summary>
        [Required]
        [JsonProperty("photo_url", Required = Required.Always)]
        public string Url { get; set; }

        /// <summary>Gets or sets the width of the photo (Optional).</summary>
        [JsonProperty("photo_width")]
        public int Width { get; set; }

        #endregion
    }
}