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
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="documentId">A file id as string to resend a photo that is already on the Telegram servers.</param>
        /// <param name="caption">Photo caption (may also be used when resending photos by file id).</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendPhotoAsync(long chatId, [NotNull] string documentId, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendPhotoAsync(chatId.ToString(), documentId, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="documentId">A file id as string to resend a photo that is already on the Telegram servers.</param>
        /// <param name="caption">Photo caption (may also be used when resending photos by file id).</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendPhotoAsync([NotNull] string chatId, [NotNull] string documentId, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(documentId, nameof(documentId));

            var parameters = new NameValueCollection { { "photo", documentId } };

            if( !string.IsNullOrWhiteSpace(caption) )
                parameters.Add("caption", caption);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendPhoto", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="photoStream">A <see cref="Stream" /> to the photo to upload.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="photoStream" />.</param>
        /// <param name="caption">Photo caption (may also be used when resending photos by file id).</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendPhotoAsync(long chatId, [NotNull] Stream photoStream, string fileName, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendPhotoAsync(chatId.ToString(), photoStream, fileName, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="photoStream">A <see cref="Stream" /> to the photo to upload.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="photoStream" />.</param>
        /// <param name="caption">Photo caption (may also be used when resending photos by file id).</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendPhotoAsync([NotNull] string chatId, [NotNull] Stream photoStream, string fileName, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(photoStream, nameof(photoStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(photoStream), "photo", fileName);

            if( !string.IsNullOrWhiteSpace(caption) )
                content.Add("caption", caption);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendPhoto", content, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">The fully qualified path to the photo to send.</param>
        /// <param name="caption">Photo caption (may also be used when resending photos by file id).</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendPhotoFromFileAsync(long chatId, [NotNull] string filePath, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendPhotoFromFileAsync(chatId.ToString(), filePath, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="filePath">The fully qualified path to the photo to send.</param>
        /// <param name="caption">Photo caption (may also be used when resending photos by file id).</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendPhotoFromFileAsync([NotNull] string chatId, [NotNull] string filePath, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(filePath, nameof(filePath));
            Contracts.EnsureFileExists(filePath);
            
            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendPhotoAsync(chatId, fileStream, fileName, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        #endregion
    }
}