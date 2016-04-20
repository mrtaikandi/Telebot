namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a venue. By default, the venue will be sent by the user. Alternatively, you can use 
    /// <see cref="InlineQueryResult.MessageContent" /> to send a message with the specified content instead of the venue.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResultLocation" />
    public class InlineQueryResultVenue : InlineQueryResultLocation
    {
        #region Public Properties

        /// <summary>Gets or sets the address of the venue.</summary>
        [Required]
        [JsonProperty("address", Required = Required.Always)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Foursquare identifier of the venue if known (Optional).
        /// </summary>
        [JsonProperty("foursquare_id")]
        public string FoursquareId { get; set; }

        /// <summary>Gets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Venue;

        #endregion
    }
}