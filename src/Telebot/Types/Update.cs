namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using JetBrains.Annotations;

    using Newtonsoft.Json;

    /// <summary>Represents an incoming update.</summary>
    /// <remarks>
    /// Only one of the optional parameters can be present in any given update.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class Update
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the new incoming callback query (Optional).
        /// </summary>
        [JsonProperty("callback_query", Required = Required.Default)]
        public CallbackQuery CallbackQuery { get; set; }

        /// <summary>
        /// Gets or sets the result of an inline query that was chosen by a user and sent to their chat partner
        /// (Optional).
        /// </summary>
        [CanBeNull]
        [JsonProperty("chosen_inline_result")]
        public ChosenInlineResult ChosenInlineResult { get; set; }

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

        /// <summary>
        /// Gets or sets the new incoming inline query (Optional).
        /// </summary>
        [CanBeNull]
        [JsonProperty("inline_query")]
        public InlineQuery InlineQuery { get; set; }

        /// <summary>
        /// Gets or sets the new incoming message of any kind - <see cref="MessageType.Text" />,
        /// <see cref="MessageType.Photo" />, <see cref="MessageType.Sticker" />, etc (Optional).
        /// </summary>
        [CanBeNull]
        [JsonProperty("message")]
        public Message Message { get; set; }

        /// <summary>Gets the type of this update.</summary>
        [JsonIgnore]
        public UpdateType Type
        {
            get
            {
                if( this.InlineQuery != null )
                    return UpdateType.InlineQuery;

                if( this.ChosenInlineResult != null )
                    return UpdateType.ChosenInlineResult;

                return UpdateType.Message;
            }
        }

        #endregion
    }
}