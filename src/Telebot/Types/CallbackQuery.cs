namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an incoming callback query from a callback button in an inline keyboard. If
    /// the button that originated the query was attached to a message sent by the bot, the <see cref="T:CallbackQuery.Message"/> property
    /// will be presented. If the button was attached to a message sent via the bot (in inline mode), the
    /// <see cref="T:CallbackQuery.InlineMessageId"/> property will be presented.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CallbackQuery
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the data associated with the callback button. Be aware that a bad client can send arbitrary data in
        /// this field
        /// </summary>
        [JsonProperty("data", Required = Required.Always)]
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        [JsonProperty("from", Required = Required.Always)]
        public User From { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for this query.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the message sent via the bot in inline mode, that originated the query (Optional).
        /// </summary>
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Message"/> with the callback button that originated the query. 
        /// Note that message content and message date will not be available if the message is too old (Optional).
        /// </summary>
        [JsonProperty("message")]
        public Message Message { get; set; }

        #endregion
    }
}