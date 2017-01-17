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
        /// Forwards message of any kind.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="fromChatId">Unique identifier for the chat where the original message was sent or username of the target
        /// channel (in the format @channelusername).</param>
        /// <param name="messageId">Unique message identifier</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> ForwardMessageAsync([NotNull] string chatId, string fromChatId, long messageId, bool disableNotification = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));

            var parameters = new NameValueCollection
                                 {
                                     { "from_chat_id", fromChatId },
                                     { "message_id", messageId.ToString() }
                                 };

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "forwardMessage", parameters, chatId, disableNotification: disableNotification);
        }

        /// <summary>Forwards message of any kind.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="fromChatId">
        /// Unique identifier for the chat where the original message was sent or username of the target
        /// channel (in the format @channelusername).
        /// </param>
        /// <param name="messageId">Unique message identifier</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> ForwardMessageAsync(long chatId, string fromChatId, long messageId, bool disableNotification = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.ForwardMessageAsync(chatId.ToString(), fromChatId, messageId, disableNotification, cancellationToken);
        }

        /// <summary>Forwards message of any kind.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="fromChatId">
        /// Unique identifier for the chat where the original message was sent.
        /// </param>
        /// <param name="messageId">Unique message identifier</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> ForwardMessageAsync(long chatId, long fromChatId, long messageId, bool disableNotification = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.ForwardMessageAsync(chatId.ToString(), fromChatId.ToString(), messageId, disableNotification, cancellationToken);
        }

        /// <summary>
        /// Sends a chat action. Use this method when you need to tell the user that something is happening on
        /// the bot's side.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="action">Type of action to broadcast.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Use this method when you need to tell the user that something is happening on the bot's side. The
        /// status is set for 5 seconds or less (when a message arrives from your bot, Telegram clients clear
        /// its typing status).
        /// <example>
        /// The <c>ImageBot</c> needs some time to process a request and upload the image. Instead of sending a
        /// text message along the lines of "Retrieving image, please wait…", the bot may use
        /// <see cref="SendChatActionAsync(long,ChatAction,CancellationToken)" />
        /// with action = upload_photo. The user will see a "sending photo" status for the bot.
        /// </example>
        /// </remarks>
        public Task<bool> SendChatActionAsync(long chatId, ChatAction action, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendChatActionAsync(chatId.ToString(), action, cancellationToken);
        }

        /// <summary>
        /// Sends a chat action. Use this method when you need to tell the user that something is happening on
        /// the bot's side.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="action">Type of action to broadcast.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Use this method when you need to tell the user that something is happening on the bot's side. The
        /// status is set for 5 seconds or less (when a message arrives from your bot, Telegram clients clear
        /// its typing status).
        /// <example>
        /// The <c>ImageBot</c> needs some time to process a request and upload the image. Instead of sending a
        /// text message along the lines of "Retrieving image, please wait…", the bot may use
        /// <see cref="SendChatActionAsync(string,Taikandi.Telebot.Types.ChatAction,System.Threading.CancellationToken)" />
        /// with action = upload_photo. The user will see a "sending photo" status for the bot.
        /// </example>
        /// </remarks>
        public Task<bool> SendChatActionAsync([NotNull] string chatId, ChatAction action, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));

            var parameters = new NameValueCollection { { "action", action.Value } };
            return this.CallTelegramMethodAsync<bool>(cancellationToken, "sendChatAction", parameters, chatId);
        }

        /// <summary>
        /// Sends a text message.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="text">Text of the message to be sent.</param>
        /// <param name="parseMode">Indicates the way that the Telegram should parse the sent message.</param>
        /// <param name="disableWebPagePreview">if set to <c>true</c> disables link previews for links in this message.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendMessageAsync(long chatId, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendMessageAsync(chatId.ToString(), text, parseMode, disableWebPagePreview, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a text message.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="text">Text of the message to be sent.</param>
        /// <param name="parseMode">
        /// Indicates the way that the Telegram should parse the sent message.
        /// </param>
        /// <param name="disableWebPagePreview">
        /// if set to <c>true</c> disables link previews for links in this message.
        /// </param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendMessageAsync([NotNull] string chatId, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(text, nameof(text));

            var parameters = new NameValueCollection { { "text", text } };
            parameters.AddIf(disableWebPagePreview, "disable_web_page_preview", true);
            parameters.AddIf(parseMode != ParseMode.Normal, "parse_mode", parseMode.ToString());

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendMessage", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends a text message and requests to hide the current custom keyboard by default. Optionally if the
        /// message is a reply, ID of the original message will be sent.
        /// </summary>
        /// <param name="message">The original received message.</param>
        /// <param name="text">Text of the message to be sent.</param>
        /// <param name="parseMode">Indicates the way that the Telegram should parse the sent message.</param>
        /// <param name="disableWebPagePreview">if set to <c>true</c> disables link previews for links in this message.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user. Defaults to hide the current
        /// custom keyboard.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendMessageAsync([NotNull] Message message, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, bool disableNotification = false, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(message, nameof(message));
            return this.SendMessageAsync(message.Chat.Id.ToString(), text, parseMode, disableWebPagePreview, disableNotification, message.ReplyToMessage?.Id ?? 0, replyMarkup, cancellationToken);
        }

        #endregion
    }
}