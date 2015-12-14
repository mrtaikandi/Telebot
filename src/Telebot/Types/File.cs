namespace Taikandi.Telebot.Types
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// This object represents a file ready to be downloaded.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class File
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier for this <see cref="File" />.
        /// </summary>
        [Required]
        [JsonProperty("file_id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>Gets or sets the file path.</summary>
        [JsonProperty("file_path")]
        public string Path { get; set; }

        /// <summary>Gets or sets the file size, if known (Optional).</summary>
        [JsonProperty("file_size")]
        public int Size { get; set; }

        #endregion

        #region Methods

        internal Uri GetDownloadUrl(string apiKey)
        {
            return new Uri($"https://api.telegram.org/file/bot{apiKey}/{this.Path}", UriKind.Absolute);
        }

        #endregion
    }
}