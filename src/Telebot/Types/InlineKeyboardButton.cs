namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents one button of an inline keyboard. You must use exactly one of the optional fields.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineKeyboardButton
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineKeyboardButton"/> class.
        /// </summary>
        public InlineKeyboardButton()
        {
            this.Url = string.Empty;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the data to be sent in a callback query to the bot when button is pressed (Optional).
        /// </summary>
        [JsonProperty(PropertyName = "callback_data")]
        public string CallbackData { get; set; }

        /// <summary>
        /// Gets or sets the inline query of the selected chat (Optional). See remarks for more information.
        /// </summary>
        /// <remarks>
        /// If set, pressing the button will prompt the user to select one of their chats, open that chat and
        /// insert the bot‘s username and the specified inline query in the input field. Can be empty, in which
        /// case just the bot’s username will be inserted.
        /// <para>
        /// Note: This offers an easy way for users to start using your bot in inline mode when they are
        /// currently in a private chat with it. Especially useful when combined with switch_pm… actions – in
        /// this case the user will be automatically returned to the chat they switched from, skipping the chat
        /// selection screen.
        /// </para>
        /// </remarks>
        [JsonProperty(PropertyName = "switch_inline_query")]
        public string SwitchInlineQuery { get; set; }

        /// <summary>
        /// Gets or sets the inline query of the current chat (Optional). See remarks for more information.
        /// </summary>
        /// <remarks>
        /// If set, pressing the button will insert the bot‘s username and the specified inline query in the
        /// current chat's input field. Can be empty, in which case only the bot’s username will be inserted.
        /// <para>
        /// This offers a quick way for the user to open your bot in inline mode in the same chat – good for
        /// selecting something from multiple options.
        /// </para>
        /// </remarks>
        [JsonProperty(PropertyName = "switch_inline_query_current_chat")]
        public string SwitchInlineQueryCurrentChat { get; set; }

        /// <summary>Gets or sets the label text on the button.</summary>
        [JsonProperty(PropertyName = "text", Required = Required.Always)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets description of the game that will be launched when the user presses the button.
        /// </summary>
        /// <remarks>
        /// <c>NOTE:</c> This type of button <c>must</c> always be the first button in the first row.
        /// </remarks>
        [JsonProperty(PropertyName = "callback_game")]
        public CallbackGame CallbackGame { get; set; }

        /// <summary>
        /// Gets or sets the HTTP url to be opened when button is pressed (Optional).
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        #endregion
    }
}