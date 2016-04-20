namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents one button of the reply keyboard. For simple text buttons <see cref="string" />s can be
    /// used instead of this object to specify text of the button. Optional fields are mutually exclusive.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class KeyboardButton
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardButton" /> class.
        /// </summary>
        public KeyboardButton()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardButton" /> class.
        /// </summary>
        /// <param name="text">The text of the button.</param>
        public KeyboardButton(string text)
        {
            this.Text = text;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the user's phone number will be sent as a contact when the
        /// button is pressed. Available in private chats only (optional).
        /// </summary>
        [JsonProperty("request_contact")]
        public bool RequestContact { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's current location will be sent when the button is
        /// pressed. Available in private chats only (optional).
        /// </summary>
        [JsonProperty("request_location")]
        public bool RequestLocation { get; set; }

        /// <summary>
        /// Gets or sets the text of the button. If none of the optional fields are used, it will be sent to
        /// the bot as a message when the button is pressed.
        /// </summary>
        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        #endregion
    }
}