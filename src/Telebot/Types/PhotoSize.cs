using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Taikandi.Telebot.Types
{
    /// <summary>
    /// This object represents one size of a photo or a <see cref="Document"/> / <see cref="Sticker" />thumbnail.
    /// </summary>
    [JsonObject]
    public class PhotoSize
    {
        /// <summary>
        /// Gets or sets the unique identifier for this file
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Gets or sets the photo width.
        /// </summary>
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the photo height.
        /// </summary>
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the size of the file (Optional).
        /// </summary>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }
}
