namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Contains information about why a request was unsuccessfull.
    /// </summary>
    public class ResponseParameters
    {
        #region Public Properties

        /// <summary>
        /// The group has been migrated to a supergroup with the specified identifier. This number may be
        /// greater than 32 bits and some programming languages may have difficulty/silent defects in interpreting it.
        /// But it is smaller than 52 bits, so a signed 64 bit integer or double-precision float type are safe for
        /// storing this identifier (Optional).
        /// </summary>
        [JsonProperty("migrate_to_chat_id")]
        public long MigrateToChatId { get; set; }

        /// <summary>
        /// In case of exceeding flood control, the number of seconds left to wait before the request can be repeated (Optional).
        /// </summary>
        [JsonProperty("retry_after")]
        public int RetryAfter { get; set; }

        #endregion
    }
}