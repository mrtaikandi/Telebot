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
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="videoId">A file id as string to resend a video that is already on the Telegram servers.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="Types.IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Types.Message" />.
        /// </returns>
        /// <remarks>
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Types.Document" />). Bots
        /// can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVideoAsync(long chatId, [NotNull] string videoId, int duration = 0, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendVideoAsync(chatId.ToString(), videoId, duration, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="videoId">A file id as string to resend a video that is already on the Telegram servers.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
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
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />). Bots
        /// can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVideoAsync([NotNull] string chatId, [NotNull] string videoId, int duration = 0, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(videoId, nameof(videoId));

            var parameters = new NameValueCollection { { "video", videoId } };
            if( duration > 0 )
                parameters.Add("duration", duration);

            if( caption != null )
                parameters.Add("caption", caption);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendVideo", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="videoStream">A <see cref="Stream" /> to the video file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="videoStream" />.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
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
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />). Bots
        /// can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVideoAsync(long chatId, [NotNull] Stream videoStream, string fileName, int duration = 0, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendVideoAsync(chatId.ToString(), videoStream, fileName, duration, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="videoStream">A <see cref="Stream" /> to the video file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="videoStream" />.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
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
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />). Bots
        /// can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVideoAsync([NotNull] string chatId, [NotNull] Stream videoStream, string fileName, int duration = 0, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(videoStream, nameof(videoStream));

            var content = new MultipartFormDataContent { { new StreamContent(videoStream), "video", fileName } };
            if( duration > 0 )
                content.Add("duration", duration);

            if( caption != null )
                content.Add("caption", caption);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendVideo", content, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the video file to send.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
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
        public Task<Message> SendVideoFromFileAsync(long chatId, [NotNull] string filePath, int duration = 0, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendVideoFromFileAsync(chatId.ToString(), filePath, duration, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="filePath">Fully qualified path to the video file to send.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
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
        public Task<Message> SendVideoFromFileAsync([NotNull] string chatId, [NotNull] string filePath, int duration = 0, string caption = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(filePath, nameof(filePath));
            Contracts.EnsureFileExists(filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendVideoAsync(chatId, fileStream, fileName, duration, caption, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        #endregion
    }
}