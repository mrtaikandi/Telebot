namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an inline keyboard that appears right next to the message it belongs to.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineKeyboardMarkup : IReply
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets an array of button rows, each represented by an Array of
        /// <see cref="InlineKeyboardButton" /> objects.
        /// </summary>
        [JsonProperty("inline_keyboard", Required = Required.Always)]
        public InlineKeyboardButton[][] InlineKeyboard { get; set; }

        #endregion
    }
}