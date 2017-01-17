namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// This object contains information about one member of the chat.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ChatMember
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets Information about the user.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }

        /// <summary>Gets or sets the member's status in the chat.</summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status", Required = Required.Always)]
        public ChatMemberStatus Status { get; set; }

        #endregion
    }
}
