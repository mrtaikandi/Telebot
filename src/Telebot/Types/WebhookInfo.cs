namespace Taikandi.Telebot.Types
{
    using System;
    using Newtonsoft.Json;
    using Converters;

    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Contains information about the current status of a webhook.
    /// </summary>
    public class WebhookInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets webhook URL, may be empty if webhook is not set up
        /// </summary>
        [JsonProperty("url", Required = Required.Always)]
        public string Url { get; set; }

        /// <summary>Gets True, if a custom certificate was provided for webhook certificate checks</summary>
        [JsonProperty("has_custom_certificate", Required = Required.Always)]
        public bool HasCustomCertificate { get; set; }

        /// <summary>Gets number of updates awaiting delivery.</summary>
        [JsonProperty("pending_update_count", Required = Required.Always)]
        public int PendingUpdateCount { get; set; }

        /// <summary>
        /// Gets unix time for the most recent error that happened when trying to deliver an update via webhook (Optional).
        /// </summary>
        [JsonProperty("last_error_date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset LastErrorDate { get; set; }

        /// <summary>Gets number of updates awaiting delivery.</summary>
        [JsonProperty("last_error_message")]
        public string LastErrorMessage { get; set; }

        /// <summary>Gets number of updates awaiting delivery.</summary>
        [JsonProperty("max_connections")]
        public int MaxConnections { get; set; }

        /// <summary>
        /// Gets a list of update types the bot is subscribed to. Defaults to all update types (Optional).
        /// </summary>
        [JsonProperty("allowed_updates")]
        public UpdateType[] AllowedUpdates { get; set; }

        #endregion
    }
}