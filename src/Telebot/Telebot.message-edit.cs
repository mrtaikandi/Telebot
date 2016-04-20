namespace Taikandi.Telebot
{    
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Taikandi.Telebot.Types;

    public partial class Telebot
    {
        #region Public Methods and Operators

        /// <summary>
        /// Edits captions of the message with the provided identifier sent by the bot or via the bot (for
        /// inline bots).
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="messageId">Unique identifier of the sent message.</param>
        /// <param name="caption">New caption of the message.</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. On success the task results contains the edited
        /// Message is returned.
        /// </returns>
        public Task<Message> EditMessageCaptionAsync(long chatId, int messageId, [NotNull] string caption, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(chatId, nameof(chatId));
            return this.EditMessageCaptionAsync(chatId.ToString(), messageId, caption, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Edits captions of the message with the provided identifier sent by the bot or via the bot (for
        /// inline bots).
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="messageId">Unique identifier of the sent message.</param>
        /// <param name="caption">New caption of the message.</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. On success the task results contains the edited
        /// Message is returned.
        /// </returns>
        public Task<Message> EditMessageCaptionAsync([NotNull] string chatId, int messageId, [NotNull] string caption, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsurePositiveNumber(messageId, nameof(messageId));

            return this.EditMessageCaptionAsync(chatId, messageId, null, caption, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Edits captions of the message with the provided identifier sent by the bot or via the bot (for
        /// inline bots).
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message.</param>
        /// <param name="caption">New caption of the message.</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. On success the task results contains the edited
        /// Message is returned.
        /// </returns>
        public Task<Message> EditMessageCaptionAsync([NotNull] string inlineMessageId, [NotNull] string caption, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(inlineMessageId, nameof(inlineMessageId));

            return this.EditMessageCaptionAsync(null, 0, inlineMessageId, caption, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Edit only the reply markup of messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="messageId">Unique identifier of the sent message.</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. On success the task results contains the edited
        /// Message is returned.
        /// </returns>
        public Task<Message> EditMessageReplyMarkupAsync(long chatId, int messageId, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(chatId, nameof(chatId));            
            return this.EditMessageReplyMarkupAsync(chatId.ToString(), messageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Edit only the reply markup of messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="messageId">Unique identifier of the sent message.</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. On success the task results contains the edited
        /// Message is returned.
        /// </returns>
        public Task<Message> EditMessageReplyMarkupAsync([NotNull] string chatId, int messageId, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsurePositiveNumber(messageId, nameof(messageId));

            return this.EditMessageReplyMarkupAsync(chatId, messageId, null, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Edit only the reply markup of messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="inlineMessageId">Identifier of the inline message.</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. On success the task results contains the edited
        /// Message is returned.
        /// </returns>
        public Task<Message> EditMessageReplyMarkupAsync([NotNull] string inlineMessageId, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(inlineMessageId, nameof(inlineMessageId));

            return this.EditMessageReplyMarkupAsync(null, 0, inlineMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Edits text messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="inlineMessageId">The identifier of the inline message.</param>
        /// <param name="text">New text of the message</param>
        /// <param name="parseMode">
        /// A value from <see cref="ParseMode" /> enum indicates the way that the Telegram should parse the
        /// sent message. Send <see cref="ParseMode.Markdown" />, if you want Telegram apps to show bold,
        /// italic, fixed-width text or inline URLs in your bot's message.
        /// </param>
        /// <param name="disableWebPagePreview">Disables link previews for links in this message</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains the edited
        /// <see cref="Message" /> on success.
        /// </returns>
        public Task<Message> EditMessageTextAsync([NotNull] string inlineMessageId, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(inlineMessageId, nameof(inlineMessageId));

            return this.EditMessageTextAsync(null, 0, inlineMessageId, text, parseMode, disableWebPagePreview, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Edits text messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="messageId">Unique identifier of the sent message.</param>
        /// <param name="text">New text of the message</param>
        /// <param name="parseMode">
        /// A value from <see cref="ParseMode" /> enum indicates the way that the Telegram should parse the
        /// sent message. Send <see cref="ParseMode.Markdown" />, if you want Telegram apps to show bold,
        /// italic, fixed-width text or inline URLs in your bot's message.
        /// </param>
        /// <param name="disableWebPagePreview">Disables link previews for links in this message</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains the edited
        /// <see cref="Message" /> on success.
        /// </returns>
        public Task<Message> EditMessageTextAsync(long chatId, int messageId, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(chatId, nameof(chatId));
            return this.EditMessageTextAsync(chatId.ToString(), messageId, text, parseMode, disableWebPagePreview, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Edits text messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="messageId">Unique identifier of the sent message.</param>
        /// <param name="text">New text of the message</param>
        /// <param name="parseMode">
        /// A value from <see cref="ParseMode" /> enum indicates the way that the Telegram should parse the
        /// sent message. Send <see cref="ParseMode.Markdown" />, if you want Telegram apps to show bold,
        /// italic, fixed-width text or inline URLs in your bot's message.
        /// </param>
        /// <param name="disableWebPagePreview">Disables link previews for links in this message</param>
        /// <param name="replyMarkup">
        /// An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains the edited
        /// <see cref="Message" /> on success.
        /// </returns>
        public Task<Message> EditMessageTextAsync([NotNull] string chatId, int messageId, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsurePositiveNumber(messageId, nameof(messageId));
            
            return this.EditMessageTextAsync(chatId, messageId, null, text, parseMode, disableWebPagePreview, replyMarkup, cancellationToken);
        }

        #endregion
    }
}