namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will hide the current custom keyboard
    /// and display the default letter-keyboard. By default, custom keyboards are displayed until a new
    /// keyboard is sent by a bot. An exception is made for one-time keyboards that are hidden immediately
    /// after the user presses a button. See also <seealso cref="ReplyKeyboardMarkup" />.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ReplyKeyboardHide : IReply
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyKeyboardHide" /> class.
        /// </summary>
        /// <param name="hideKeyboard">
        /// if set to <c>true</c> requests clients to hide the custom keyboard. Defaults to <c>true</c>.
        /// </param>
        /// <param name="selective">
        /// if set to <c>true</c> hides custom keyboard for specific users only. Defaults to <c>true</c>.
        /// </param>
        public ReplyKeyboardHide(bool hideKeyboard = true, bool selective = true)
        {
            this.HideKeyboard = hideKeyboard;
            this.Selective = selective;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to request clients to hide the custom keyboard.
        /// </summary>
        [JsonProperty("hide_keyboard", Required = Required.Always)]
        public bool HideKeyboard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether you want to hide keyboard for specific users only
        /// (Optional).
        /// <para>
        /// Use this parameter if you want to hide keyboard for specific users only. Targets: 1) users that are
        /// @mentioned in the text of the Message object; 2) if the bot's message is a reply (has
        /// reply_to_message_id), sender of the original message. Example: A user votes in a poll, bot returns
        /// confirmation message in reply to the vote and hides keyboard for that user, while still showing the
        /// keyboard with poll options to users who haven't voted yet.
        /// </para>
        /// </summary>
        [JsonProperty("selective")]
        public bool Selective { get; set; }

        #endregion
    }
}