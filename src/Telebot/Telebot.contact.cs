namespace Taikandi.Telebot
{
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Taikandi.Telebot.Types;

    public partial class Telebot
    {
        #region Public Methods and Operators

        /// <summary>Send phone contacts.</summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername)
        /// </param>
        /// <param name="phoneNumber">Contact's phone number.</param>
        /// <param name="firstName">Contact's first name.</param>
        /// <param name="lastName">Contact's last name.</param>
        /// <param name="disableNotification">
        /// If set to <c>true</c> sends the message silently. iOS users will not receive a notification,
        /// Android users will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains sent
        /// <see cref="Message" /> on success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// chatId cannot be null -or- phoneNumber cannot be null -or- firstName cannot be null.
        /// </exception>
        public Task<Message> SendContactAsync(long chatId, [NotNull] string phoneNumber, [NotNull] string firstName, string lastName = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));

            return this.SendContactAsync(chatId.ToString(), phoneNumber, firstName, lastName, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Send phone contacts.</summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername)
        /// </param>
        /// <param name="phoneNumber">Contact's phone number.</param>
        /// <param name="firstName">Contact's first name.</param>
        /// <param name="lastName">Contact's last name.</param>
        /// <param name="disableNotification">
        /// If set to <c>true</c> sends the message silently. iOS users will not receive a notification,
        /// Android users will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains sent
        /// <see cref="Message" /> on success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// chatId cannot be null -or- phoneNumber cannot be null -or- firstName cannot be null.
        /// </exception>
        public Task<Message> SendContactAsync([NotNull] string chatId, [NotNull] string phoneNumber, [NotNull] string firstName, string lastName = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(phoneNumber, nameof(phoneNumber));
            Contracts.EnsureNotNull(firstName, nameof(firstName));

            var parameters = new NameValueCollection
                                 {
                                     { "chat_id", chatId }, 
                                     { "phone_number", phoneNumber }, 
                                     { "first_name", firstName }
                                 };

            parameters.AddIf(!string.IsNullOrWhiteSpace(lastName), "last_name", lastName);
            parameters.AddIf(!disableNotification, "disable_notification", true);
            parameters.AddIf(replyToMessageId > 0, "reply_to_message_id", replyToMessageId);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendContact", parameters, replyMarkup: replyMarkup);
        }

        #endregion
    }
}