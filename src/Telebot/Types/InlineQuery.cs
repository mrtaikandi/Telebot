namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>represents an incoming inline query.</summary>
    /// <remarks>
    /// When the user sends an empty query, your bot could return some default or trending results.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQuery
    {
        #region Public Properties

        /// <summary>Gets or sets the sender of the query.</summary>
        [JsonProperty("from", Required = Required.Always)]
        public User From { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="InlineQuery" />.
        /// </summary>
        [Range(Common.IdentifierMinValue, long.MaxValue)]
        [JsonProperty("id", Required = Required.AllowNull)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the offset of the results to be returned, can be controlled by the bot.
        /// </summary>
        [JsonProperty("offset")]
        public string Offset { get; set; }

        /// <summary>Gets or sets the text of the query.</summary>
        [JsonProperty("query")]
        public string Query { get; set; }

        #endregion
    }
}