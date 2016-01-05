namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>This object represents a chat.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Chat
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the first name of the other party in a private chat (Optional).
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for this chat.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        [Range(Common.IdentifierMinValue, Common.IdentifierMaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the last name of the other party in a private chat (Optional).
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the title of the channel or group chat (Optional).
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>Gets or sets the type of chat.</summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type", Required = Required.Always)]
        public ChatType Type { get; set; }

        /// <summary>
        /// Gets or sets the username of the private chat or channel (Optional).
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        #endregion
    }
}