namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// This object represents a result of an inline query that was chosen by the user and sent to their
    /// chat partner.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ChosenInlineResult
    {
        #region Public Properties

        /// <summary>Gets or sets the user that chose the result.</summary>
        [JsonProperty("from", Required = Required.Always)]
        public User From { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the sent inline message. Available only if there is an inline
        /// keyboard attached to the message. Will be also received in callback queries and can be used to edit
        /// the message.
        /// </summary>
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }

        /// <summary>
        /// Gets or sets the sender location, only for bots that request user location.
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; }

        /// <summary>
        /// Gets or sets the query that was used to obtain the result.
        /// </summary>
        [JsonProperty("query", Required = Required.Always)]
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the result that was chosen.
        /// </summary>
        [JsonProperty("result_id", Required = Required.Always)]
        public string ResultId { get; set; }

        #endregion
    }
}