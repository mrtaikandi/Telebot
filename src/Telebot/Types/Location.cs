namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>This object represents a point on the map.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Location
    {
        #region Public Properties

        /// <summary>Gets or sets th latitude as defined by sender</summary>
        [JsonProperty("latitude", Required = Required.Always)]
        public double Latitude { get; set; }

        /// <summary>Gets or sets the longitude as defined by sender</summary>
        [JsonProperty("longitude", Required = Required.Always)]
        public double Longitude { get; set; }

        #endregion
    }
}