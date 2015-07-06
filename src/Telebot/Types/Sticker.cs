namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// This object represents a sticker.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Sticker
    {
        #region Public Properties

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
        /// Gets or sets the sticker height.
        /// </summary>
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the sticker thumbnail in .webp or .jpg format.
        /// </summary>
        [JsonProperty("thumb", Required = Required.Always)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Gets or sets the sticker width.
        /// </summary>
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        #endregion
    }
}