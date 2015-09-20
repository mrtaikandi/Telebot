namespace Taikandi.Telebot.Types
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// This object represents a file ready to be downloaded.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class File
    {
        /// <summary>
        /// Gets or sets the unique identifier for this <see cref="File"/>.
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the file size, if known (Optional).
        /// </summary>
        [JsonProperty("file_size")]
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string Path { get; set; }

        internal Uri GetDownloadUrl(string apiKey)
        {
            return new Uri($"https://api.telegram.org/file/bot{apiKey}/{this.Path}", UriKind.Absolute);
        }
    }
}