namespace Taikandi.Telebot
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Newtonsoft.Json;

    using Taikandi.Telebot.Types;

    public class Telebot : IDisposable
    {
        #region Fields

        private HttpClient _client;

        private volatile bool _disposedValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Telebot" /> class.
        /// </summary>
        /// <param name="apiKey">
        /// Telegram API key.
        /// </param>
        public Telebot([NotNull] string apiKey)
        {
            if( string.IsNullOrWhiteSpace(apiKey) )
                throw new ArgumentNullException(nameof(apiKey));

            this.ApiKey = apiKey;
        }

        #endregion

        #region Public Properties

        public HttpClient Client => this._client ?? (this._client = this.CreateHttpClient());

        #endregion

        #region Properties

        protected string ApiKey { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);

            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Forwards message of any kind.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="fromChatId">
        /// Unique identifier for the chat where the original message was sent, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="messageId">
        /// Unique message identifier
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public async Task<TelegramResult<Message>> ForwardMessageAsync(int chatId, int fromChatId, int messageId)
        {
            var parameters = new NameValueCollection
                {
                    { "chat_id", chatId.ToString() },
                    { "from_chat_id", fromChatId.ToString() },
                    { "message_id", messageId.ToString() }
                };

            var content = new FormUrlEncodedContent(parameters);
            var response = await this.Client.PostAsync("forwardMessage", content).ConfigureAwait(false);
            EnsureSuccessStatusCode(response);

            return await response.Content.ReadAsAsync<TelegramResult<Message>>().ConfigureAwait(false);
        }

        /// <summary>
        /// A simple method for testing your bot's auth token. Requires no parameters.
        /// Returns basic information about the bot in form of a User object.
        /// </summary>
        /// <returns>
        /// Basic information about the bot in form of a User object.
        /// </returns>
        public async Task<TelegramResult<User>> GetMeAsync()
        {
            var response = await this.Client.GetAsync("getMe").ConfigureAwait(false);
            EnsureSuccessStatusCode(response);
            return await response.Content.ReadAsAsync<TelegramResult<User>>().ConfigureAwait(false);
        }

        /// <summary>
        /// Use this method to receive incoming updates using long polling.
        /// <para>
        /// Note: In order to avoid getting duplicate updates, recalculate offset after each server response.
        /// </para>
        /// </summary>
        /// <param name="offset">
        /// Identifier of the first update to be returned. Must be greater by one than the highest among
        /// the identifiers of previously received updates. By default, updates starting with the earliest
        /// unconfirmed update are returned. An update is considered confirmed as soon as getUpdates is
        /// called with an offset higher than its update_id.
        /// </param>
        /// <param name="limit">
        /// Limits the number of updates to be retrieved. Values between 1—100 are accepted. Defaults to 100.
        /// </param>
        /// <param name="timeout">
        /// Timeout in seconds for long polling. Defaults to 0, i.e. usual short polling.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// An Array of Update objects.
        /// </returns>
        public async Task<TelegramResult<IList<Update>>> GetUpdatesAsync(int offset = 0, int limit = 100, int timeout = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await this.Client
                                     .GetAsync($"getUpdates?offset={offset}&limit={limit}&timeout={timeout}", cancellationToken)
                                     .ConfigureAwait(false);

            EnsureSuccessStatusCode(response);
            return await response.Content.ReadAsAsync<TelegramResult<IList<Update>>>(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of profile pictures for a user.
        /// </summary>
        /// <param name="userId">
        /// Unique identifier of the target user.
        /// </param>
        /// <param name="offset">
        /// Sequential number of the first photo to be returned. By default, all photos are returned.
        /// </param>
        /// <param name="limit">
        /// Limits the number of photos to be retrieved. Values between 1-100 are accepted. Defaults to 100.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="UserProfilePhotos" />.
        /// </returns>
        public async Task<TelegramResult<UserProfilePhotos>> GetUserProfillePhotos(int userId, int offset = -1, int limit = 100, CancellationToken cancellationToken = default(CancellationToken))
        {
            var builder = new StringBuilder();
            builder.AppendFormat("getUserProfilePhotos?user_id={0}&limit={1}", userId, limit);

            if( offset >= 0 )
                builder.AppendFormat("offset={0}", offset);

            var response = await this.Client
                                     .GetAsync(builder.ToString(), cancellationToken)
                                     .ConfigureAwait(false);

            EnsureSuccessStatusCode(response);
            return await response.Content.ReadAsAsync<TelegramResult<UserProfilePhotos>>(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable music on Telegram clients music player.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="audioId">
        /// Id of an audio file that is already on the Telegram servers.
        /// </param>
        /// <param name="duration">Duration of the audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be
        /// sent as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size,
        /// this limit may be changed in the future.
        /// </remarks>
        public async Task<TelegramResult<Message>> SendAudioAsync(int chatId, [NotNull] string audioId, int duration = 0, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(audioId) )
                throw new ArgumentNullException(nameof(audioId));

            var parameters = new NameValueCollection { { "audio", audioId } };

            if( duration > 0 )
                parameters.Add("duration", duration.ToString());

            if( performer != null )
                parameters.Add("performer", performer);

            if( title != null )
                parameters.Add("title", title);

            return await this.CallTelegramMethodAsync("sendAudio", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable music on Telegram clients music player.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="audioStream">A <see cref="Stream" /> to the audio file to send.</param>
        /// <param name="fileName">
        /// A name for the file to be sent using <paramref name="audioStream" />.
        /// </param>
        /// <param name="duration">Duration of the audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be
        /// sent as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size,
        /// this limit may be changed in the future.
        /// </remarks>
        public async Task<TelegramResult<Message>> SendAudioAsync(int chatId, [NotNull] Stream audioStream, string fileName, int duration, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( audioStream == null )
                throw new ArgumentNullException(nameof(audioStream));

            var content = new MultipartFormDataContent { { new StreamContent(audioStream), "audio", fileName } };

            if( duration > 0 )
                content.Add(new StringContent(duration.ToString()), "duration");

            if( performer != null )
                content.Add(new StringContent(performer), "performer");

            if( title != null )
                content.Add(new StringContent(title), "title");

            return await this.CallTelegramMethodAsync("sendAudio", content, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be
        /// sent as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size,
        /// this limit may be changed in the future.
        /// </remarks>
        public Task<TelegramResult<Message>> SendAudioFromFileAsync(int chatId, [NotNull] string filePath, int duration = 0, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the audio file at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendAudioAsync(chatId, fileStream, fileName, duration, performer, title, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a chat action. Use this method when you need to tell the user that something is happening on the bot's side.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="action">
        /// Type of action to broadcast.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Use this method when you need to tell the user that something is happening on the bot's side. The status is
        /// set for 5 seconds or less (when a message arrives from your bot, Telegram clients clear its typing status).
        /// <example>
        /// The <c>ImageBot</c> needs some time to process a request and upload the image. Instead of sending a
        /// text message along the lines of "Retrieving image, please wait…", the bot may use <see cref="SendChatAction" />
        /// with action = upload_photo. The user will see a "sending photo" status for the bot.
        /// </example>
        /// </remarks>
        public async Task<TelegramResult<bool>> SendChatAction(int chatId, ChatAction action, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new NameValueCollection { { "action", action.Value } };
            return await this.CallTelegramMethodAsync<bool>("sendChatAction", parameters, chatId, -1, null, cancellationToken);
        }

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="documentId">
        /// A file id as string to resend a file that is already on the Telegram servers.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public async Task<TelegramResult<Message>> SendDocumentAsync(int chatId, [NotNull] string documentId, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(documentId) )
                throw new ArgumentNullException(nameof(documentId));

            var parameters = new NameValueCollection { { "document", documentId } };
            return await this.CallTelegramMethodAsync("sendDocument", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="documentStream">
        /// A <see cref="Stream" /> to the document file to send.
        /// </param>
        /// <param name="fileName">
        /// A name for the file to be sent using <paramref name="documentStream" />.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public async Task<TelegramResult<Message>> SendDocumentAsync(int chatId, [NotNull] Stream documentStream, string fileName, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( documentStream == null )
                throw new ArgumentNullException(nameof(documentStream));

            var content = new MultipartFormDataContent { { new StreamContent(documentStream), "document", fileName } };
            return await this.CallTelegramMethodAsync("sendDocument", content, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a general file.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="filePath">
        /// Fully qualified path to the file to send.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<TelegramResult<Message>> SendDocumentFromFileAsync(int chatId, [NotNull] string filePath, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the document at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendDocumentAsync(chatId, fileStream, fileName, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a point on the map.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="latitude">
        /// Latitude of location
        /// </param>
        /// <param name="longitude">
        /// Longitude of location
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public async Task<TelegramResult<Message>> SendLocationAsync(int chatId, double latitude, double longitude, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new NameValueCollection
                {
                    { "latitude", latitude.ToString(CultureInfo.InvariantCulture) },
                    { "longitude", longitude.ToString(CultureInfo.InvariantCulture) }
                };

            return await this.CallTelegramMethodAsync("sendLocation", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a text message.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient — <see cref="User" /> or <see cref="GroupChat" /> id.</param>
        /// <param name="text">Text of the message to be sent.</param>
        /// <param name="parseMode">Indicates the way that the Telegram should parse the sent message.</param>
        /// <param name="disableWebPagePreview">if set to <c>true</c> disables link previews for links in this message.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public async Task<TelegramResult<Message>> SendMessageAsync(int chatId, [NotNull] string text, ParseMode parseMode = null, bool disableWebPagePreview = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( text == null )
                throw new ArgumentNullException(nameof(text));

            var parameters = new NameValueCollection { { "text", text } };

            if( disableWebPagePreview )
                parameters.Add("disable_web_page_preview", "true");

            if( parseMode != null && parseMode != ParseMode.Normal )
                parameters.Add("parse_mode", parseMode.Value);

            return await this.CallTelegramMethodAsync("sendMessage", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a text message and requests to hide the current custom keyboard by default.
        /// Optionally if the message is a reply, ID of the original message will be sent.
        /// </summary>
        /// <param name="message">
        /// The original received message.
        /// </param>
        /// <param name="text">
        /// Text of the message to be sent.
        /// </param>
        /// <param name="parseMode">Indicates the way that the Telegram should parse the sent message.</param>
        /// <param name="disableWebPagePreview">
        /// if set to <c>true</c> disables link previews for links in this message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user. Defaults to hide the current custom keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<TelegramResult<Message>> SendMessageAsync(Message message, [NotNull] string text, ParseMode parseMode = null, bool disableWebPagePreview = false, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendMessageAsync(message.Chat.Id, text, parseMode, disableWebPagePreview, message.ReplyToMessage?.Id ?? 0, replyMarkup ?? new ReplyKeyboardHide(), cancellationToken);
        }

        /// <summary>
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="documentId">
        /// A file id as string to resend a photo that is already on the Telegram servers.
        /// </param>
        /// <param name="caption">
        /// Photo caption (may also be used when resending photos by file id).
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public async Task<TelegramResult<Message>> SendPhotoAsync(int chatId, [NotNull] string documentId, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(documentId) )
                throw new ArgumentNullException(nameof(documentId));

            var parameters = new NameValueCollection { { "photo", documentId } };

            if( !string.IsNullOrWhiteSpace(caption) )
                parameters.Add("caption", caption);

            return await this.CallTelegramMethodAsync("sendPhoto", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="photoStream">
        /// A <see cref="Stream" /> to the photo to upload.
        /// </param>
        /// <param name="fileName">
        /// A name for the file to be sent using <paramref name="photoStream" />.
        /// </param>
        /// <param name="caption">
        /// Photo caption (may also be used when resending photos by file id).
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public async Task<TelegramResult<Message>> SendPhotoAsync(int chatId, [NotNull] Stream photoStream, string fileName, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( photoStream == null )
                throw new ArgumentNullException(nameof(photoStream));

            var content = new MultipartFormDataContent { { new StreamContent(photoStream), "photo", fileName } };

            if( !string.IsNullOrWhiteSpace(caption) )
                content.Add(new StringContent(caption), "caption");

            return await this.CallTelegramMethodAsync("sendPhoto", content, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a photo.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="filePath">
        /// The fully qualified path to the photo to send.
        /// </param>
        /// <param name="caption">
        /// Photo caption (may also be used when resending photos by file id).
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<TelegramResult<Message>> SendPhotoFromFileAsync(int chatId, [NotNull] string filePath, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the photo at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendPhotoAsync(chatId, fileStream, fileName, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="stickerId">
        /// A file id as string to resend a sticker that is already on the Telegram servers.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public async Task<TelegramResult<Message>> SendStickerAsync(int chatId, [NotNull] string stickerId, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(stickerId) )
                throw new ArgumentNullException(nameof(stickerId));

            var parameters = new NameValueCollection { { "sticker", stickerId } };
            return await this.CallTelegramMethodAsync("sendSticker", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="stickerStream">
        /// A <see cref="Stream" /> to the sticker file to send.
        /// </param>
        /// <param name="fileName">
        /// A name for the file to be sent using <paramref name="stickerStream" />.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public async Task<TelegramResult<Message>> SendStickerAsync(int chatId, [NotNull] Stream stickerStream, string fileName, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( stickerStream == null )
                throw new ArgumentNullException(nameof(stickerStream));

            var content = new MultipartFormDataContent { { new StreamContent(stickerStream), "sticker", fileName } };

            return await this.CallTelegramMethodAsync("sendSticker", content, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends <c>.webp</c> sticker.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="filePath">
        /// Fully qualified path to the sticker to send.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<TelegramResult<Message>> SendStickerFromFileAsync(int chatId, [NotNull] string filePath, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the sticker at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendStickerAsync(chatId, fileStream, fileName, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="videoId">
        /// A file id as string to resend a video that is already on the Telegram servers.
        /// </param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />).
        /// Bots can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public async Task<TelegramResult<Message>> SendVideoAsync(int chatId, [NotNull] string videoId, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(videoId) )
                throw new ArgumentNullException(nameof(videoId));

            var parameters = new NameValueCollection { { "video", videoId } };
            if( duration > 0 )
                parameters.Add("duration", duration.ToString());

            if( caption != null )
                parameters.Add("caption", caption);

            return await this.CallTelegramMethodAsync("sendVideo", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="videoStream">A <see cref="Stream" /> to the video file to send.</param>
        /// <param name="fileName">
        /// A name for the file to be sent using <paramref name="videoStream" />.
        /// </param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />).
        /// Bots can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public async Task<TelegramResult<Message>> SendVideoAsync(int chatId, [NotNull] Stream videoStream, string fileName, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( videoStream == null )
                throw new ArgumentNullException(nameof(videoStream));

            var content = new MultipartFormDataContent { { new StreamContent(videoStream), "video", fileName } };
            if( duration > 0 )
                content.Add(new StringContent(duration.ToString()), "duration");

            if( caption != null )
                content.Add(new StringContent(caption), "caption");

            return await this.CallTelegramMethodAsync("sendVideo", content, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a video file.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="filePath">Fully qualified path to the video file to send.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<TelegramResult<Message>> SendVideoFromFileAsync(int chatId, [NotNull] string filePath, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the video file at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendVideoAsync(chatId, fileStream, fileName, duration, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="voice">
        /// Id of an audio file that is already on the Telegram servers to resend it.
        /// </param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be
        /// sent as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size,
        /// this limit may be changed in the future.
        /// </remarks>
        public async Task<TelegramResult<Message>> SendVoiceAsync(int chatId, [NotNull] string voice, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(voice) )
                throw new ArgumentNullException(nameof(voice));

            var parameters = new NameValueCollection { { "voice", voice } };

            if( duration > 0 )
                parameters.Add("duration", duration.ToString());

            return await this.CallTelegramMethodAsync("sendVoice", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="voiceStream">A <see cref="Stream" /> to the audio file to send.</param>
        /// <param name="fileName">
        /// A name for the file to be sent using <paramref name="voiceStream" />.
        /// </param>
        /// <param name="duration">Duration of sent audio in seconds</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be
        /// sent as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size,
        /// this limit may be changed in the future.
        /// </remarks>
        public async Task<TelegramResult<Message>> SendVoiceAsync(int chatId, [NotNull] Stream voiceStream, string fileName, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( voiceStream == null )
                throw new ArgumentNullException(nameof(voiceStream));

            var content = new MultipartFormDataContent { { new StreamContent(voiceStream), "voice", fileName } };
            if( duration > 0 )
                content.Add(new StringContent(duration.ToString()), "duration");

            return await this.CallTelegramMethodAsync("sendVoice", content, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient, <see cref="User" /> or <see cref="GroupChat" /> id.
        /// </param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard, instructions to hide
        /// keyboard or to force a reply from the
        /// user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be
        /// sent as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size,
        /// this limit may be changed in the future.
        /// </remarks>
        public Task<TelegramResult<Message>> SendVoiceFromFileAsync(int chatId, [NotNull] string filePath, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the audio file at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendVoiceAsync(chatId, fileStream, fileName, duration, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Use this method to specify a url and receive incoming updates via an outgoing webhook.
        /// </summary>
        /// <param name="url">
        /// HTTPS url to send updates to. Use an empty string to remove webhook integration
        /// </param>
        /// <param name="certificatePath">
        /// The fully qualified path to the certificate path so that the root certificate in use can be checked.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Whenever there is an update for the bot, Telegram will send an HTTPS POST request to the specified url,
        /// containing a JSON-serialized <see cref="Update" />. In case of an unsuccessful request,
        /// it will give up after a reasonable amount of attempts.
        /// <para>
        /// If you'd like to make sure that the Webhook request comes from Telegram, we recommend using
        /// a secret path in the URL, e.g. <c>www.example.com/YOUR_TOKEN</c>. Since nobody else knows your
        /// bot's token, you can be pretty sure it’s Telegram.
        /// </para>
        /// <list type="bullet">
        ///     <item>
        ///         <description>
        /// You will not be able to receive updates using <see cref="GetUpdatesAsync" /> for as long as an outgoing webhook is set
        /// up.
        ///         </description>
        ///     </item><item>
        ///         <description>
        /// To use a self-signed certificate, you need to upload your public key certificate using
        /// <paramref name="certificatePath" /> parameter.
        ///         </description>
        ///     </item><item>
        ///         <description>
        /// Ports currently supported for Webhooks: 443, 80, 88, 8443.
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        public async Task SetWebhookAsync(string url, string certificatePath = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(certificatePath) )
                await this.Client.PostAsync(url, null, cancellationToken).ConfigureAwait(false);
            else
            {
                if( !File.Exists(certificatePath) )
                    throw new FileNotFoundException("Unable to find the certificate file at the specified location.", certificatePath);

                using( var content = new MultipartFormDataContent() )
                {
                    var fileName = Path.GetFileName(certificatePath);
                    var fileStream = File.Open(certificatePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    content.Add(new StreamContent(fileStream), "certificate", fileName);
                    content.Add(new StringContent(url), "url");

                    var response = await this.Client.PostAsync("setWebhook", content, cancellationToken).ConfigureAwait(false);
                    EnsureSuccessStatusCode(response);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of <see cref="HttpClient" /> to connect to the Telegram bot API.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="HttpClient" /> configured to connect to the Telegram bot API.
        /// </returns>
        protected virtual HttpClient CreateHttpClient()
        {
            var client = new HttpClient
                {
                    BaseAddress = new Uri($"https://api.telegram.org/bot{this.ApiKey}/", UriKind.Absolute)
                };

            return client;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if( !this._disposedValue )
            {
                if( disposing )
                {
                    // dispose managed state (managed objects).
                    this.Client.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.
                this._client = null;
                this._disposedValue = true;
            }
        }

        private static void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if( !response.IsSuccessStatusCode )
            {
                if( response.StatusCode == HttpStatusCode.BadGateway )
                    throw new ServiceUnavailableException();

                response.EnsureSuccessStatusCode();
            }
        }

        private Task<TelegramResult<Message>> CallTelegramMethodAsync(string uri, NameValueCollection parameters, int chatId, int replyToMessageId, IReply replyMarkup, CancellationToken cancellationToken)
        {
            return this.CallTelegramMethodAsync<Message>(uri, parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        private async Task<TelegramResult<T>> CallTelegramMethodAsync<T>(string uri, NameValueCollection parameters, int chatId, int replyToMessageId, IReply replyMarkup, CancellationToken cancellationToken)
        {
            parameters.Add("chat_id", chatId.ToString());

            if( replyToMessageId > 0 )
                parameters.Add("reply_to_message_id", replyToMessageId.ToString());

            if( replyMarkup != null )
                parameters.Add("reply_markup", JsonConvert.SerializeObject(replyMarkup));

            using( var content = new FormUrlEncodedContent(parameters) )
            {
                var response = await this.Client.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);
                EnsureSuccessStatusCode(response);

                return await response.Content.ReadAsAsync<TelegramResult<T>>(cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task<TelegramResult<Message>> CallTelegramMethodAsync([NotNull] string url, MultipartFormDataContent content, int chatId, int replyToMessageId, IReply replyMarkup, CancellationToken cancellationToken)
        {
            using( content )
            {
                content.Add(new StringContent(chatId.ToString()), "chat_id");

                if( replyToMessageId >= 0 )
                    content.Add(new StringContent(replyToMessageId.ToString()), "reply_to_message_id");

                if( replyMarkup != null )
                    content.Add(new StringContent(JsonConvert.SerializeObject(replyMarkup)), "reply_markup");

                var response = await this.Client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
                EnsureSuccessStatusCode(response);

                return await response.Content.ReadAsAsync<TelegramResult<Message>>(cancellationToken).ConfigureAwait(false);
            }
        }

        #endregion
    }
}