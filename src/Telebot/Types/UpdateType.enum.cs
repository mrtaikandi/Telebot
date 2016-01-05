namespace Taikandi.Telebot.Types
{
    /// <summary>
    /// Provides available types of an update.
    /// </summary>
    public enum UpdateType
    {
        /// <summary>
        /// Indicates that the received update is a <see cref="Message"/>.
        /// </summary>
        Message,

        /// <summary>
        /// Indicates that the received update is a type of <see cref="InlineQuery"/>.
        /// </summary>
        InlineQuery,

        /// <summary>
        /// Indicates that the received update is a result of an inline query that was chosen by a user and sent to their chat partner.
        /// </summary>
        ChosenInlineResult,
    }
}