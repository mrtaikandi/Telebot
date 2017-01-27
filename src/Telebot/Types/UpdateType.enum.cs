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
        /// New version of a message that is known to the bot and was edited
        /// </summary>
        EditedMessage,

        /// <summary>
        /// New incoming channel post of any kind — text, photo, sticker, etc.
        /// </summary>
        ChannelPost,

        /// <summary>
        /// New version of a channel post that is known to the bot and was edited
        /// </summary>
        EditedChannelPost,

        /// <summary>
        /// Indicates that the received update is a type of <see cref="InlineQuery"/>.
        /// </summary>
        InlineQuery,

        /// <summary>
        /// Indicates that the received update is a result of an inline query that was chosen by a user and sent to their chat partner.
        /// </summary>
        ChosenInlineResult,

        /// <summary>
        /// New incoming callback query
        /// </summary>
        CallbackQuery
    }
}