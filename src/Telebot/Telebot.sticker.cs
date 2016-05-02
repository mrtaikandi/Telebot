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
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="stickerId">A file id as string to resend a sticker that is already on the Telegram servers.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendStickerAsync(long chatId, [NotNull] string stickerId, int replyToMessageId = 0, bool disableNotification = false, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(chatId, nameof(chatId));
            return this.SendStickerAsync(chatId.ToString(), stickerId, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="stickerId">A file id as string to resend a sticker that is already on the Telegram servers.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendStickerAsync([NotNull] string chatId, [NotNull] string stickerId, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(stickerId, nameof(stickerId));

            var parameters = new NameValueCollection { { "sticker", stickerId } };
            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendSticker", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="stickerStream">A <see cref="Stream" /> to the sticker file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="stickerStream" />.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendStickerAsync(long chatId, [NotNull] Stream stickerStream, string fileName, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(chatId, nameof(chatId));
            return this.SendStickerAsync(chatId.ToString(), stickerStream, fileName, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="stickerStream">A <see cref="Stream" /> to the sticker file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="stickerStream" />.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendStickerAsync([NotNull] string chatId, [NotNull] Stream stickerStream, string fileName, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(stickerStream, nameof(stickerStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add("sticker", stickerStream, fileName);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendSticker", content, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the sticker to send.</param>
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
        public Task<Message> SendStickerFromFileAsync(long chatId, [NotNull] string filePath, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(chatId, nameof(chatId));
            return this.SendStickerFromFileAsync(chatId.ToString(), filePath, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="filePath">Fully qualified path to the sticker to send.</param>
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
        public Task<Message> SendStickerFromFileAsync([NotNull] string chatId, [NotNull] string filePath, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(filePath, nameof(filePath));
            Contracts.EnsureFileExists(filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendStickerAsync(chatId, fileStream, fileName, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        #endregion
    }
}