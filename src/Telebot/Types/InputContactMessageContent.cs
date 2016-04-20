namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the content of a contact message to be sent as the result of an inline query.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InputContactMessageContent : InputMessageContent
    {
        /// <summary>
        /// Gets or sets the contact's phone number.
        /// </summary>
        [JsonProperty("phone_number", Required = Required.Always)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the contact's first name.
        /// </summary>
        [JsonProperty("first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the contact's last name (Optional).
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}