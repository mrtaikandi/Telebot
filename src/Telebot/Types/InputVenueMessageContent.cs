namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the content of a venue message to be sent as the result of an inline query.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InputMessageContent" />
    [JsonObject(MemberSerialization.OptIn)]
    public class InputVenueMessageContent : InputLocationMessageContent
    {
        #region Public Properties

        /// <summary>Gets or sets the address of the venue</summary>
        [JsonProperty("address", Required = Required.Always)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Foursquare identifier of the venue, if known.
        /// </summary>
        [JsonProperty("foursquare_id")]
        public string FoursquareId { get; set; }

        /// <summary>Gets or sets the name of the venue.</summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        #endregion
    }
}