namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// This object represents a point on the map.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Location
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets th latitude as defined by sender
        /// </summary>
        [JsonProperty("latitude", Required = Required.Always)]
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude as defined by sender
        /// </summary>
        [JsonProperty("longitude", Required = Required.Always)]
        public float Longitude { get; set; }

        #endregion
    }
}