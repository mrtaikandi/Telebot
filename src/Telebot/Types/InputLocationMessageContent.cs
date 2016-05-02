namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the content of a location message to be sent as the result of an inline query.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InputMessageContent" />
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class InputLocationMessageContent : InputMessageContent
    {
        /// <summary>
        /// Gets or sets the latitude of the location in degrees.
        /// </summary>
        [JsonProperty("latitude", Required = Required.Always)]
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the location in degrees.
        /// </summary>
        [JsonProperty("longitude", Required = Required.Always)]
        public float Longitude { get; set; }
    }
}