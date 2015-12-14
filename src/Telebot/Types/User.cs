namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>This object represents a Telegram user or bot.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        #region Public Properties

        /// <summary>Gets or sets the User‘s or bot’s first name.</summary>
        [Required]
        [JsonProperty("first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for this user or bot.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        [Range(Common.IdentifierMinValue, Common.IdentifierMaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the User‘s or bot’s last name (optional).
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets user‘s or bot’s username (optional).
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        #endregion
    }
}