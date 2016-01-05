namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to an animated GIF file. By default, this animated GIF file will be sent by the
    /// user with optional caption. Alternatively, you can provide
    /// <see cref="MessageText" />
    /// to send it instead of the animation.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultGif : InlineQueryResult
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineQueryResultGif" /> class.
        /// </summary>
        public InlineQueryResultGif()
            : this(null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineQueryResultGif" /> class.
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
        public InlineQueryResultGif(string id, string url, string thumbnailUrl)
        {
            ExceptionHelper.ValidateArgumentByteCount(nameof(id), id);

            this.Id = id;
            this.Url = url;
            this.ThumbnailUrl = thumbnailUrl;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the maximum of 200 character caption of the <c>.gif</c> file to be sent (Optional).
        /// </summary>
        [JsonProperty("caption")]
        [StringLength(200)]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the height of the <c>.gif</c> file (Optional).
        /// </summary>
        [JsonProperty("gif_height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the text of a message to be sent instead of the animation (Optional).
        /// </summary>
        [StringLength(512)]
        [JsonProperty("message_text")]
        public string MessageText { get; set; }

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
        public override InlineQueryResultType Type => InlineQueryResultType.Gif;

        /// <summary>
        /// Gets or sets a valid URL for the <c>.gif</c> file. File size must not exceed 1MB.
        /// </summary>
        [Required]
        [JsonProperty("gif_url", Required = Required.Always)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the width of the <c>.gif</c> file (Optional).
        /// </summary>
        [JsonProperty("gif_width")]
        public int Width { get; set; }

        #endregion
    }
}