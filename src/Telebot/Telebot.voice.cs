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
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="voice">Id of an audio file that is already on the Telegram servers to resend it.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
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
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceAsync(long chatId, [NotNull] string voice, int duration = 0, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendVoiceAsync(chatId.ToString(), voice, duration, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="voice">Id of an audio file that is already on the Telegram servers to resend it.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
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
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceAsync([NotNull] string chatId, [NotNull] string voice, int duration = 0, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(voice, nameof(voice));

            var parameters = new NameValueCollection { { "voice", voice } };

            if( duration > 0 )
                parameters.Add("duration", duration);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendVoice", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="voiceStream">A <see cref="Stream" /> to the audio file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="voiceStream" />.</param>
        /// <param name="duration">Duration of sent audio in seconds</param>
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
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceAsync(long chatId, [NotNull] Stream voiceStream, string fileName, int duration = 0, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendVoiceAsync(chatId.ToString(), voiceStream, fileName, duration, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="voiceStream">A <see cref="Stream" /> to the audio file to send.</param>
        /// <param name="fileName">A name for the file to be sent using <paramref name="voiceStream" />.</param>
        /// <param name="duration">Duration of sent audio in seconds</param>
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
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceAsync([NotNull] string chatId, [NotNull] Stream voiceStream, string fileName, int duration = 0, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(voiceStream, nameof(voiceStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add("voice", voiceStream, fileName);

            if( duration > 0 )
                content.Add("duration", duration);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendVoice", content, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
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
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceFromFileAsync(long chatId, [NotNull] string filePath, int duration = 0, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.SendVoiceFromFileAsync(chatId.ToString(), filePath, duration, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
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
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceFromFileAsync([NotNull] string chatId, [NotNull] string filePath, int duration = 0, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(filePath, nameof(filePath));
            Contracts.EnsureFileExists(filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendVoiceAsync(chatId, fileStream, fileName, duration, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        #endregion
    }
}