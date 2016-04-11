namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>This object represents a phone contact.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Contact
    {
        #region Public Properties

        /// <summary>Gets or sets the contact's first name</summary>
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
        /// Gets or sets the contact's user identifier in Telegram (Optional).
        /// </summary>
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        #endregion
    }
}