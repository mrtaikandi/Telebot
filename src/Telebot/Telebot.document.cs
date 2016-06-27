namespace Taikandi.Telebot
{    
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Taikandi.Telebot.Types;

    using File = System.IO.File;

    public partial class Telebot
    {
        #region Public Methods and Operators

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="documentId">A file id as string to resend a file that is already on the Telegram servers.</param>
        /// <param name="caption">The document caption, 0-200 characters.</param>
        /// <param name="disableNotification">if set to <c>true</c> [disable notification].</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentAsync(long chatId, [NotNull] string documentId, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendDocumentAsync(chatId.ToString(), documentId, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="documentId">A file id as string to resend a file that is already on the Telegram servers.</param>
        /// <param name="caption">The document caption, 0-200 characters.</param>
        /// <param name="disableNotification">if set to <c>true</c> [disable notification].</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentAsync([NotNull] string chatId, [NotNull] string documentId, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(documentId, nameof(documentId));
            
            var parameters = new NameValueCollection { { "document", documentId } };
            parameters.AddIf(!string.IsNullOrWhiteSpace(caption), "caption", caption);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendDocument", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="documentStream">A <see cref="Stream" /> to the document file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="documentStream" />.</param>
        /// <param name="caption">Document caption, 0-200 characters.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentAsync(long chatId, [NotNull] Stream documentStream, string fileName, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendDocumentAsync(chatId.ToString(), documentStream, fileName, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="documentStream">A <see cref="Stream" /> to the document file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="documentStream" />.</param>
        /// <param name="caption">Document caption, 0-200 characters.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentAsync([NotNull] string chatId, [NotNull] Stream documentStream, string fileName, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(documentStream, nameof(documentStream));
            
            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add("document", documentStream, fileName);
            content.AddIf(!string.IsNullOrWhiteSpace(caption), "caption", caption);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendDocument", content, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="filePath">Fully qualified path to the file to send.</param>
        /// <param name="caption">The document caption.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentFromFileAsync(long chatId, [NotNull] string filePath, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendDocumentFromFileAsync(chatId.ToString(), filePath, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="filePath">Fully qualified path to the file to send.</param>
        /// <param name="caption">The document caption.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentFromFileAsync([NotNull] string chatId, [NotNull] string filePath, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(filePath, nameof(filePath));
            Contracts.EnsureFileExists(filePath);            

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendDocumentAsync(chatId, fileStream, fileName, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        #endregion
    }
}