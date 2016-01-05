namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents link to a page containing an embedded video player or a video file.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultVideo : InlineQueryResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InlineQueryResultVideo" /> class.
        /// </summary>
        public InlineQueryResultVideo()
            : this(null, null, null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineQueryResultVideo" /> class.
        /// </summary>
        /// <param name="id">The unique identifier of this result. Must be less than 64 bytes.</param>
        /// <param name="url">A valid URL of the photo. Photo size must not exceed 5MB.</param>
        /// <param name="thumbnailUrl">The URL of the thumbnail for the result either in <c>.jpeg</c> or <c>.gif</c> format.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="messageText">The text of a message to be sent.</param>
        public InlineQueryResultVideo(string id, string url, string thumbnailUrl, MimeTypes mimeType, string messageText)
        {
            ExceptionHelper.ValidateArgumentByteCount(nameof(id), id);

            this.Id = id;
            this.Url = url;
            this.ThumbnailUrl = thumbnailUrl;
            this.MimeType = mimeType;
            this.MessageText = messageText;
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets a short description of the result (Optional).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Gets or sets the title of the result.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the video duration in seconds (Optional).
        /// </summary>
        [JsonProperty("video_duration")]
        public int Duration { get; set; }

        /// <summary>Gets or sets the height of the video (Optional).</summary>
        [JsonProperty("video_height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the mime type of the content of video url, "text/html" or "video/mp4". 
        /// Use values from <see cref="MimeTypes"/> class.
        /// </summary>
        [Required]
        [JsonProperty("mime_type", Required = Required.Always)]
        public string MimeType { get; set; }

        /// <summary>Gets or sets the text of a message to be sent with the video.</summary>
        [Required]
        [StringLength(512)]
        [JsonProperty("message_text", Required = Required.Always)]
        public string MessageText { get; set; }

        /// <summary>
        /// Gets or sets the URL of the thumbnail for the result either in <c>.jpeg</c> or <c>.gif</c> format.
        /// </summary>
        [Required]
        [JsonProperty("thumb_url", Required = Required.Always)]
        public string ThumbnailUrl { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Video;

        /// <summary>
        /// Gets or sets a valid URL for the embedded video player or video file.
        /// </summary>
        [Required]
        [JsonProperty("video_url", Required = Required.Always)]
        public string Url { get; set; }

        /// <summary>Gets or sets the width of the video (Optional).</summary>
        [JsonProperty("video_width")]
        public int Width { get; set; }

        #endregion
    }
}