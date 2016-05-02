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
        /// Sends an audio file. If you want Telegram clients to display them in the music player. Your audio must be in the .mp3 format.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="audioId">Id of an audio file that is already on the Telegram servers.</param>
        /// <param name="duration">Duration of the audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendAudioAsync([NotNull] string chatId, [NotNull] string audioId, int duration = 0, string performer = null, string title = null, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(chatId, nameof(audioId));

            var parameters = new NameValueCollection { { "audio", audioId } };
            parameters.AddIf(duration > 0, "duration", duration);
            parameters.AddIf(!string.IsNullOrWhiteSpace(performer), "performer", performer);
            parameters.AddIf(!string.IsNullOrWhiteSpace(title), "title", title);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendAudio", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends an audio file. If you want Telegram clients to display them in the music player. Your audio must be in the .mp3 format.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="audioId">Id of an audio file that is already on the Telegram servers.</param>
        /// <param name="duration">Duration of the audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendAudioAsync(long chatId, [NotNull] string audioId, int duration = 0, string performer = null, string title = null, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(chatId, nameof(chatId));
            return this.SendAudioAsync(chatId.ToString(), audioId, duration, performer, title, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file. If you want Telegram clients to display them in the music player. Your audio must be in the .mp3 format.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="audioStream">A <see cref="Stream" /> to the audio file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="audioStream" />.</param>
        /// <param name="duration">Duration of the audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendAudioAsync(long chatId, [NotNull] Stream audioStream, string fileName, int duration, string performer = null, string title = null, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(chatId, nameof(chatId));
            return this.SendAudioAsync(chatId.ToString(), audioStream, fileName, duration, performer, title, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file. If you want Telegram clients to display them in the music player. Your audio must be in the .mp3 format.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="audioStream">A <see cref="Stream" /> to the audio file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="audioStream" />.</param>
        /// <param name="duration">Duration of the audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendAudioAsync([NotNull] string chatId, [NotNull] Stream audioStream, string fileName, int duration, string performer = null, string title = null, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(audioStream, nameof(audioStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add("audio", audioStream, fileName);
            content.AddIf(duration > 0, "duration", duration);
            content.AddIf(!string.IsNullOrWhiteSpace(performer), "performer", performer);
            content.AddIf(!string.IsNullOrWhiteSpace(title), "title", title);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendAudio", content, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendAudioFromFileAsync(long chatId, [NotNull] string filePath, int duration = 0, string performer = null, string title = null, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendAudioFromFileAsync(chatId.ToString(), filePath, duration, performer, title, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendAudioFromFileAsync([NotNull] string chatId, [NotNull] string filePath, int duration = 0, string performer = null, string title = null, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(filePath, nameof(filePath));
            Contracts.EnsureFileExists(filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendAudioAsync(chatId, fileStream, fileName, duration, performer, title, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        #endregion
    }
}