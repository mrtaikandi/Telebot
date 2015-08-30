namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// This object represents a video file.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Video
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the text description of the video (usually empty).
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the duration of the video in seconds as defined by sender.
        /// </summary>
        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for this file.
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Gets or sets the file size (Optional).
        /// </summary>
        [JsonProperty("file_size")]
        public int FileSize { get; set; }

        /// <summary>
        /// Gets or sets the video height as defined by sender.
        /// </summary>
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the file as defined by sender (Optional).
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the video thumbnail.
        /// </summary>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Gets or sets the video width as defined by sender.
        /// </summary>
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        #endregion
    }
}