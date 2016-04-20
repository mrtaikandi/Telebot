namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>Represents a venue.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Venue
    {
        #region Public Properties

        /// <summary>Gets or sets the address of the venue.</summary>
        [Required]
        [JsonProperty("address", Required = Required.Always)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Foursquare identifier of the venue (Optional).
        /// </summary>
        [JsonProperty("foursquare_id")]
        public string FoursquareId { get; set; }

        /// <summary>Gets or sets the Venue location.</summary>
        [Required]
        [JsonProperty("location", Required = Required.Always)]
        public Location Location { get; set; }

        /// <summary>Gets or sets the title of the result.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        #endregion
    }
}