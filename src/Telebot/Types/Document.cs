namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// This object represents a general file (as opposed to Photos and <see cref="Audio" />s).
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Document
    {
        #region Public Properties

        /// <summary>Gets or sets the unique file identifier.</summary>
        [Required]
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Gets or sets the original filename as defined by sender (Optional).
        /// </summary>
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        /// <summary>Gets or sets the file size (Optional).</summary>
        [JsonProperty("file_size")]
        public int FileSize { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the file as defined by sender (Optional).
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the document thumbnail as defined by sender.
        /// </summary>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        #endregion
    }
}