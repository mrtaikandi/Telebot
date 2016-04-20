namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a location on a map. By default, the location will be sent by the user. Alternatively,
    /// you can use <see cref="InlineQueryResult.MessageContent" /> to send a message with the specified
    /// content instead of the location.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore
    /// them.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultLocation : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the latitude of the location in degrees.
        /// </summary>
        [Required]
        [JsonProperty("latitude", Required = Required.Always)]
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the location in degrees.
        /// </summary>
        [Required]
        [JsonProperty("longitude", Required = Required.Always)]
        public float Longitude { get; set; }

        /// <summary>
        /// Gets or sets the height of the thumbnail (Optional).
        /// </summary>
        [JsonProperty("thumb_height")]
        public string ThumbnailHeight { get; set; }

        /// <summary>
        /// Gets or sets the URL of the thumbnail for the result (Optional).
        /// </summary>
        [JsonProperty("thumb_url")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the width of the thumbnail (Optional).
        /// </summary>
        [JsonProperty("thumb_width")]
        public string ThumbnailWidth { get; set; }

        /// <summary>Gets or sets the title of the result.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>Gets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Location;

        #endregion
    }
}