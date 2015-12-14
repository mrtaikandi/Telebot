namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>This object represents an incoming update.</summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Update
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the update‘s unique identifier.
        /// <para>
        /// Update identifiers start from a certain positive number and increase sequentially. This ID becomes
        /// especially handy if you’re using Webhooks, since it allows you to ignore repeated updates or to
        /// restore the correct update sequence, should they get out of order.
        /// </para>
        /// </summary>
        [JsonProperty("update_id", Required = Required.Always)]
        [Range(Common.IdentifierMinValue, long.MaxValue)]
        public long Id { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        #endregion
    }
}