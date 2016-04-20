namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a contact with a phone number. By default, this contact will be sent by the user.
    /// Alternatively, you can use <see cref="InlineQueryResult.MessageContent" />  to send a message with
    /// the specified content instead of the contact.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore
    /// them.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultContact : InlineQueryResult
    {
        #region Public Properties

        /// <summary>Gets or sets the contact's first name.</summary>
        [Required]
        [JsonProperty("first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>Gets or sets the contact's last name (Optional).</summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>Gets or sets the contact's phone number.</summary>
        [Required]
        [JsonProperty("phone_number", Required = Required.Always)]
        public string PhoneNumber { get; set; }

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

        /// <summary>Gets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Contact;

        #endregion
    }
}