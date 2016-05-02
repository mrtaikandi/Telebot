namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to a page containing an embedded video player or a video file. By default, this
    /// video file will be sent by the user with an optional caption. Alternatively, you can use
    /// <see cref="InlineQueryResult.MessageContent" /> to send a message with the specified content
    /// instead of the video.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultVideo : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the caption of the video to be sent, 0-200 characters (Optional).
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
        /// Gets or sets a value indicating the video duration in seconds (Optional).
        /// </summary>
        [JsonProperty("video_duration")]
        public int Duration { get; set; }

        /// <summary>Gets or sets the height of the video (Optional).</summary>
        [JsonProperty("video_height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the mime type of the content of video url, "text/html" or "video/mp4". Use values from
        /// <see cref="MimeTypes" /> class.
        /// </summary>
        [Required]
        [JsonProperty("mime_type", Required = Required.Always)]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the URL of the thumbnail for the result either in <c>.jpeg</c> or <c>.gif</c> format.
        /// </summary>
        [Required]
        [JsonProperty("thumb_url", Required = Required.Always)]
        public string ThumbnailUrl { get; set; }

        /// <summary>Gets or sets the title of the result.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

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