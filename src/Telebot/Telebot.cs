﻿namespace Taikandi.Telebot
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Taikandi.Telebot.Types;

    using File = Taikandi.Telebot.Types.File;

    public partial class Telebot : IDisposable
    {
        #region Fields

        private HttpClient _client;

        private volatile bool _disposedValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Telebot" /> class.
        /// </summary>
        /// <param name="apiKey">Telegram API key.</param>
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
        /// Sends answers to an inline query.
        /// </summary>
        /// <param name="inlineQueryId">Unique identifier for answered query.</param>
        /// <param name="results">Results of the inline query.</param>
        /// <param name="cacheTime">The maximum amount of time in seconds the result of the inline query may be cached on the Telegram
        /// server. Default is 300.</param>
        /// <param name="isPersonal"><c>true</c>, to cache the results on the Telegram server only for the user that sent the query. By
        /// default, results may be returned to any user who sends the same query.</param>
        /// <param name="nextOffset">The offset that a client should send in the next query with the same text to receive more results.
        /// <c>null</c> or <see cref="string.Empty" /> if there are no more results or if you don't support
        /// pagination. Offset length can't exceed 64 bytes.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>true</c> if the
        /// answer successfully sent; otherwise <c>false</c>.
        /// </returns>        
        /// <exception cref="ArgumentException">No more than 50 results per query are allowed.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="inlineQueryId" /> is null -or- <paramref name="results" /> is null.</exception>
        /// <exception cref="System.ArgumentException">The nextOffset argument must be less than 64 bytes.</exception>
        public async Task<bool> AnswerInlineQueryAsync(long inlineQueryId, [NotNull] IEnumerable<InlineQueryResult> results, int cacheTime = 300, bool isPersonal = false, string nextOffset = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( inlineQueryId <= 0 )
                throw new ArgumentException("The value must be greater than zero.", nameof(inlineQueryId));

            if( results == null )
                throw new ArgumentNullException(nameof(results));

            var inlineQueryResults = results as IList<InlineQueryResult> ?? results.ToList();
            if( inlineQueryResults.Count > 50 )
                throw new ArgumentException("No more than 50 results per query are allowed.", nameof(results));

            var parameters = new MultipartFormDataContent();

            if( !string.IsNullOrWhiteSpace(nextOffset) )
            {
                ExceptionHelper.ValidateArgumentByteCount(nameof(nextOffset), nextOffset);

                parameters.Add("next_offset", nextOffset);
            }

            parameters.Add("inline_query_id", inlineQueryId);
            parameters.Add("results", inlineQueryResults);
            parameters.AddIf(isPersonal, "is_peronal", isPersonal);
            parameters.AddIf(cacheTime != 300, "cache_time", cacheTime);

            return await this.CallTelegramMethodAsync("answerInlineQuery", parameters, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
        }

        /// <summary>
        /// Downloads the file requested by the <see cref="GetFileAsync(string,System.Threading.CancellationToken)" /> method.
        /// </summary>
        /// <param name="file">
        /// The file info received by calling <see cref="GetFileAsync(string,System.Threading.CancellationToken)" />.
        /// </param>
        /// <param name="fullPath">
        /// The full directory and file name to the location where the downloaded file should be saved.
        /// </param>
        /// <param name="overwrite">
        /// If set to <c>true</c> overwrites the file that exists in the <paramref name="fullPath" /> path.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// File already exists in the destination path but overwrite parameter is set to <c>false</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">file cannot be null | fullPath cannot be null.</exception>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DownloadFileAsync([NotNull] File file, [NotNull] string fullPath, bool overwrite = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( file == null )
                throw new ArgumentNullException(nameof(file));

            if( string.IsNullOrWhiteSpace(fullPath) )
                throw new ArgumentNullException(nameof(fullPath));

            if( System.IO.File.Exists(fullPath) )
            {
                if( !overwrite )
                    throw new InvalidOperationException($"File \"{fullPath}\" already exists.");

                System.IO.File.Delete(fullPath);
            }

            using( var response = await this.DownloadFileAsync(file, cancellationToken).ConfigureAwait(false) )
            {
                using( var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None) )
                {
                    await response.CopyToAsync(fileStream, 81920, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Downloads the file requested by the <see cref="GetFileAsync(string,System.Threading.CancellationToken)" /> method.
        /// </summary>
        /// <param name="file">
        /// The file info received by calling <see cref="GetFileAsync(string,System.Threading.CancellationToken)" />.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <exception cref="ArgumentNullException">File cannot be null.</exception>
        /// <returns>
        /// Returns a task containing the downloaded file as a stream.
        /// </returns>
        public async Task<Stream> DownloadFileAsync([NotNull] File file, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( file == null )
                throw new ArgumentNullException(nameof(file));

            var url = file.GetDownloadUrl(this.ApiKey);
            return await this.Client.GetStreamAsync(url).ConfigureAwait(false);
        }

        /// <summary>Forwards message of any kind.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="fromChatId">
        /// Unique identifier for the chat where the original message was sent or username of the target
        /// channel (in the format @channelusername).
        /// </param>
        /// <param name="messageId">Unique message identifier</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public async Task<Message> ForwardMessageAsync([NotNull] string chatId, string fromChatId, long messageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            var parameters = new NameValueCollection
                                 {
                                     { "from_chat_id", fromChatId },
                                     { "message_id", messageId.ToString() }
                                 };

            return await this.CallTelegramMethodAsync("forwardMessage", parameters, chatId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Forwards message of any kind.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="fromChatId">
        /// Unique identifier for the chat where the original message was sent or username of the target
        /// channel (in the format @channelusername).
        /// </param>
        /// <param name="messageId">Unique message identifier</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> ForwardMessageAsync(long chatId, string fromChatId, long messageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ForwardMessageAsync(chatId.ToString(), fromChatId, messageId, cancellationToken);
        }

        /// <summary>Forwards message of any kind.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="fromChatId">
        /// Unique identifier for the chat where the original message was sent.
        /// </param>
        /// <param name="messageId">Unique message identifier</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> ForwardMessageAsync(long chatId, long fromChatId, long messageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ForwardMessageAsync(chatId.ToString(), fromChatId.ToString(), messageId, cancellationToken);
        }

        /// <summary>
        /// Downloads the file with the specified <paramref name="fileId" />.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="fullPath">
        /// The full directory and file name to the location where the downloaded file should be saved.
        /// </param>
        /// <param name="overwrite">
        /// If set to <c>true</c> overwrites the file that exists in the <paramref name="fullPath" /> path.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task GetFileAsync([NotNull] string fileId, [NotNull] string fullPath, bool overwrite = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var file = await this.GetFileAsync(fileId, cancellationToken).ConfigureAwait(false);
            await this.DownloadFileAsync(file, fullPath, overwrite, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns basic info about a file and prepare it for downloading. For the moment, bots can download
        /// files of up to 20MB in size.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, a <see cref="Types.File" /> object containing basic info about the file to download.
        /// </returns>
        public async Task<File> GetFileAsync([NotNull] string fileId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( string.IsNullOrWhiteSpace(fileId) )
                throw new ArgumentNullException(nameof(fileId));

            return await this.CallTelegramMethodAsync<File>($"getFile?file_id={fileId}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// A simple method for testing your bot's auth token. Requires no parameters. Returns basic
        /// information about the bot in form of a User object.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// Basic information about the bot in form of a User object.
        /// </returns>
        public async Task<User> GetMeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.CallTelegramMethodAsync<User>("getMe", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Use this method to receive incoming updates using long polling.
        /// <para>
        /// Note: In order to avoid getting duplicate updates, recalculate offset after each server response.
        /// </para>
        /// </summary>
        /// <param name="offset">
        /// Identifier of the first update to be returned. Must be greater by one than the highest among the
        /// identifiers of previously received updates. By default, updates starting with the earliest
        /// unconfirmed update are returned. An update is considered confirmed as soon as getUpdates is called
        /// with an offset higher than its update_id.
        /// </param>
        /// <param name="limit">
        /// Limits the number of updates to be retrieved. Values between 1—100 are accepted. Defaults to 100.
        /// </param>
        /// <param name="timeout">
        /// Timeout in seconds for long polling. Defaults to 0, i.e. usual short polling.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>An Array of Update objects.</returns>
        public async Task<IEnumerable<Update>> GetUpdatesAsync(long offset = 0, int limit = 100, int timeout = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.CallTelegramMethodAsync<IEnumerable<Update>>($"getUpdates?offset={offset}&limit={limit}&timeout={timeout}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Gets a list of profile pictures for a user.</summary>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="offset">
        /// Sequential number of the first photo to be returned. By default, all photos are returned.
        /// </param>
        /// <param name="limit">
        /// Limits the number of photos to be retrieved. Values between 1-100 are accepted. Defaults to 100.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="UserProfilePhotos" />.
        /// </returns>
        public async Task<UserProfilePhotos> GetUserProfilePhotosAsync(int userId, int offset = -1, int limit = 100, CancellationToken cancellationToken = default(CancellationToken))
        {
            var builder = new StringBuilder();
            builder.AppendFormat("getUserProfilePhotos?user_id={0}&limit={1}", userId, limit);

            if( offset >= 0 )
                builder.AppendFormat("offset={0}", offset);

            return await this.CallTelegramMethodAsync<UserProfilePhotos>(builder.ToString(), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable music on Telegram clients music player.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size, this limit
        /// may be changed in the future.
        /// </remarks>
        public async Task<Message> SendAudioAsync([NotNull] string chatId, [NotNull] string audioId, int duration = 0, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(audioId) )
                throw new ArgumentNullException(nameof(audioId));

            var parameters = new NameValueCollection { { "audio", audioId } };

            if( duration > 0 )
                parameters.Add("duration", duration.ToString());

            if( performer != null )
                parameters.Add("performer", performer);

            if( title != null )
                parameters.Add("title", title);

            return await this.CallTelegramMethodAsync("sendAudio", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable music on Telegram clients music player.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size, this limit
        /// may be changed in the future.
        /// </remarks>
        public Task<Message> SendAudioAsync(long chatId, [NotNull] string audioId, int duration = 0, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendAudioAsync(chatId.ToString(), audioId, duration, performer, title, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable music on Telegram clients music player.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size, this limit
        /// may be changed in the future.
        /// </remarks>
        public Task<Message> SendAudioAsync(long chatId, [NotNull] Stream audioStream, string fileName, int duration, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendAudioAsync(chatId.ToString(), audioStream, fileName, duration, performer, title, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable music on Telegram clients music player.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size, this limit
        /// may be changed in the future.
        /// </remarks>
        public async Task<Message> SendAudioAsync([NotNull] string chatId, [NotNull] Stream audioStream, string fileName, int duration, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( audioStream == null )
                throw new ArgumentNullException(nameof(audioStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add("audio", audioStream, fileName);

            if( duration > 0 )
                content.Add("duration", duration.ToString());

            if( performer != null )
                content.Add("performer", performer);

            if( title != null )
                content.Add("title", title);

            return await this.CallTelegramMethodAsync("sendAudio", content, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size, this limit
        /// may be changed in the future.
        /// </remarks>
        public Task<Message> SendAudioFromFileAsync(long chatId, [NotNull] string filePath, int duration = 0, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendAudioFromFileAsync(chatId.ToString(), filePath, duration, performer, title, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
        /// <param name="performer">The performer of the audio.</param>
        /// <param name="title">The track name.</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Document" />). Bots can currently send audio files of up to 50 MB in size, this limit
        /// may be changed in the future.
        /// </remarks>
        public Task<Message> SendAudioFromFileAsync([NotNull] string chatId, [NotNull] string filePath, int duration = 0, string performer = null, string title = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !System.IO.File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the audio file at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendAudioAsync(chatId, fileStream, fileName, duration, performer, title, replyToMessageId, replyMarkup, cancellationToken);
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
        public async Task<bool> SendChatActionAsync([NotNull] string chatId, ChatAction action, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            var parameters = new NameValueCollection { { "action", action.Value } };
            return await this.CallTelegramMethodAsync<bool>("sendChatAction", parameters, chatId, -1, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends a general file.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="documentId">
        /// A file id as string to resend a file that is already on the Telegram servers.
        /// </param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentAsync(long chatId, [NotNull] string documentId, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendDocumentAsync(chatId.ToString(), documentId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a general file.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="documentId">
        /// A file id as string to resend a file that is already on the Telegram servers.
        /// </param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public async Task<Message> SendDocumentAsync([NotNull] string chatId, [NotNull] string documentId, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(documentId) )
                throw new ArgumentNullException(nameof(documentId));

            var parameters = new NameValueCollection { { "document", documentId } };
            return await this.CallTelegramMethodAsync("sendDocument", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends a general file.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentAsync(long chatId, [NotNull] Stream documentStream, string fileName, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendDocumentAsync(chatId.ToString(), documentStream, fileName, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a general file.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public async Task<Message> SendDocumentAsync([NotNull] string chatId, [NotNull] Stream documentStream, string fileName, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( documentStream == null )
                throw new ArgumentNullException(nameof(documentStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add("document", documentStream, fileName);

            return await this.CallTelegramMethodAsync("sendDocument", content, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends a general file.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the file to send.</param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentFromFileAsync(long chatId, [NotNull] string filePath, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendDocumentFromFileAsync(chatId.ToString(), filePath, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a general file.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="filePath">Fully qualified path to the file to send.</param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendDocumentFromFileAsync([NotNull] string chatId, [NotNull] string filePath, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !System.IO.File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the document at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendDocumentAsync(chatId, fileStream, fileName, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a point on the map.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="latitude">Latitude of location</param>
        /// <param name="longitude">Longitude of location</param>
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
        public Task<Message> SendLocationAsync(long chatId, double latitude, double longitude, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendLocationAsync(chatId.ToString(), latitude, longitude, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a point on the map.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="latitude">Latitude of location</param>
        /// <param name="longitude">Longitude of location</param>
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
        public async Task<Message> SendLocationAsync([NotNull] string chatId, double latitude, double longitude, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            var parameters = new NameValueCollection { { "latitude", latitude.ToString(CultureInfo.InvariantCulture) }, { "longitude", longitude.ToString(CultureInfo.InvariantCulture) } };

            return await this.CallTelegramMethodAsync("sendLocation", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends a text message.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="text">Text of the message to be sent.</param>
        /// <param name="parseMode">
        /// Indicates the way that the Telegram should parse the sent message.
        /// </param>
        /// <param name="disableWebPagePreview">
        /// if set to <c>true</c> disables link previews for links in this message.
        /// </param>
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
        public Task<Message> SendMessageAsync(long chatId, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendMessageAsync(chatId.ToString(), text, parseMode, disableWebPagePreview, replyToMessageId, replyMarkup, cancellationToken);
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
        public async Task<Message> SendMessageAsync([NotNull] string chatId, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( text == null )
                throw new ArgumentNullException(nameof(text));

            var parameters = new NameValueCollection { { "text", text } };

            if( disableWebPagePreview )
                parameters.Add("disable_web_page_preview", true);

            if( parseMode != ParseMode.Normal )
                parameters.Add("parse_mode", ParseMode.Markdown.ToString());

            return await this.CallTelegramMethodAsync("sendMessage", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a text message and requests to hide the current custom keyboard by default. Optionally if the
        /// message is a reply, ID of the original message will be sent.
        /// </summary>
        /// <param name="message">The original received message.</param>
        /// <param name="text">Text of the message to be sent.</param>
        /// <param name="parseMode">
        /// Indicates the way that the Telegram should parse the sent message.
        /// </param>
        /// <param name="disableWebPagePreview">
        /// if set to <c>true</c> disables link previews for links in this message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user. Defaults to hide the current
        /// custom keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendMessageAsync(Message message, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendMessageAsync(message.Chat.Id.ToString(), text, parseMode, disableWebPagePreview, message.ReplyToMessage?.Id ?? 0, replyMarkup ?? new ReplyKeyboardHide(), cancellationToken);
        }

        /// <summary>Sends a photo.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
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
        public Task<Message> SendPhotoAsync(long chatId, [NotNull] string documentId, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendPhotoAsync(chatId.ToString(), documentId, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a photo.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
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
        public async Task<Message> SendPhotoAsync([NotNull] string chatId, [NotNull] string documentId, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(documentId) )
                throw new ArgumentNullException(nameof(documentId));

            var parameters = new NameValueCollection { { "photo", documentId } };

            if( !string.IsNullOrWhiteSpace(caption) )
                parameters.Add("caption", caption);

            return await this.CallTelegramMethodAsync("sendPhoto", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends a photo.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="photoStream">A <see cref="Stream" /> to the photo to upload.</param>
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
        public Task<Message> SendPhotoAsync(long chatId, [NotNull] Stream photoStream, string fileName, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendPhotoAsync(chatId.ToString(), photoStream, fileName, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a photo.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="photoStream">A <see cref="Stream" /> to the photo to upload.</param>
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
        public async Task<Message> SendPhotoAsync([NotNull] string chatId, [NotNull] Stream photoStream, string fileName, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( photoStream == null )
                throw new ArgumentNullException(nameof(photoStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add( new StreamContent( photoStream ), "photo", fileName );

            if( !string.IsNullOrWhiteSpace(caption) )
                content.Add("caption", caption);

            return await this.CallTelegramMethodAsync("sendPhoto", content, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends a photo.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">The fully qualified path to the photo to send.</param>
        /// <param name="caption">
        /// Photo caption (may also be used when resending photos by file id).
        /// </param>
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
        public Task<Message> SendPhotoFromFileAsync(long chatId, [NotNull] string filePath, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendPhotoFromFileAsync(chatId.ToString(), filePath, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a photo.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="filePath">The fully qualified path to the photo to send.</param>
        /// <param name="caption">
        /// Photo caption (may also be used when resending photos by file id).
        /// </param>
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
        public Task<Message> SendPhotoFromFileAsync([NotNull] string chatId, [NotNull] string filePath, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !System.IO.File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the photo at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendPhotoAsync(chatId, fileStream, fileName, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends <c>.webp</c> sticker.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="stickerId">
        /// A file id as string to resend a sticker that is already on the Telegram servers.
        /// </param>
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
        public Task<Message> SendStickerAsync(long chatId, [NotNull] string stickerId, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendStickerAsync(chatId.ToString(), stickerId, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends <c>.webp</c> sticker.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="stickerId">
        /// A file id as string to resend a sticker that is already on the Telegram servers.
        /// </param>
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
        public async Task<Message> SendStickerAsync([NotNull] string chatId, [NotNull] string stickerId, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(stickerId) )
                throw new ArgumentNullException(nameof(stickerId));

            var parameters = new NameValueCollection { { "sticker", stickerId } };
            return await this.CallTelegramMethodAsync("sendSticker", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends <c>.webp</c> sticker.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
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
        public Task<Message> SendStickerAsync(long chatId, [NotNull] Stream stickerStream, string fileName, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendStickerAsync(chatId.ToString(), stickerStream, fileName, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends <c>.webp</c> sticker.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
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
        public async Task<Message> SendStickerAsync([NotNull] string chatId, [NotNull] Stream stickerStream, string fileName, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( stickerStream == null )
                throw new ArgumentNullException(nameof(stickerStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add("sticker", stickerStream, fileName);

            return await this.CallTelegramMethodAsync("sendSticker", content, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends <c>.webp</c> sticker.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the sticker to send.</param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendStickerFromFileAsync(long chatId, [NotNull] string filePath, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendStickerFromFileAsync(chatId.ToString(), filePath, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends <c>.webp</c> sticker.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="filePath">Fully qualified path to the sticker to send.</param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendStickerFromFileAsync([NotNull] string chatId, [NotNull] string filePath, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !System.IO.File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the sticker at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendStickerAsync(chatId, fileStream, fileName, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a video file.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="videoId">
        /// A file id as string to resend a video that is already on the Telegram servers.
        /// </param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
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
        /// <remarks>
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />). Bots
        /// can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVideoAsync(long chatId, [NotNull] string videoId, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendVideoAsync(chatId.ToString(), videoId, duration, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a video file.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
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
        /// <remarks>
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />). Bots
        /// can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public async Task<Message> SendVideoAsync([NotNull] string chatId, [NotNull] string videoId, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(videoId) )
                throw new ArgumentNullException(nameof(videoId));

            var parameters = new NameValueCollection { { "video", videoId } };
            if( duration > 0 )
                parameters.Add("duration", duration);

            if( caption != null )
                parameters.Add("caption", caption);

            return await this.CallTelegramMethodAsync("sendVideo", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends a video file.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
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
        /// <remarks>
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />). Bots
        /// can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVideoAsync(long chatId, [NotNull] Stream videoStream, string fileName, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendVideoAsync(chatId.ToString(), videoStream, fileName, duration, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a video file.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
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
        /// <remarks>
        /// Telegram clients support mp4 videos (other formats may be sent as <see cref="Document" />). Bots
        /// can currently send video files of up to 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public async Task<Message> SendVideoAsync([NotNull] string chatId, [NotNull] Stream videoStream, string fileName, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( videoStream == null )
                throw new ArgumentNullException(nameof(videoStream));

            var content = new MultipartFormDataContent { { new StreamContent(videoStream), "video", fileName } };
            if( duration > 0 )
                content.Add("duration", duration);

            if( caption != null )
                content.Add("caption", caption);

            return await this.CallTelegramMethodAsync("sendVideo", content, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Sends a video file.</summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the video file to send.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendVideoFromFileAsync(long chatId, [NotNull] string filePath, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendVideoFromFileAsync(chatId.ToString(), filePath, duration, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>Sends a video file.</summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="filePath">Fully qualified path to the video file to send.</param>
        /// <param name="duration">Duration of sent video in seconds.</param>
        /// <param name="caption">Video caption.</param>
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
        /// <remarks>
        /// Bots can currently send files of any type of up to 50 MB in size, this limit may be changed in the
        /// future.
        /// </remarks>
        public Task<Message> SendVideoFromFileAsync([NotNull] string chatId, [NotNull] string filePath, int duration = 0, string caption = null, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !System.IO.File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the video file at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendVideoAsync(chatId, fileStream, fileName, duration, caption, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="voice">
        /// Id of an audio file that is already on the Telegram servers to resend it.
        /// </param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceAsync(long chatId, [NotNull] string voice, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendVoiceAsync(chatId.ToString(), voice, duration, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="voice">
        /// Id of an audio file that is already on the Telegram servers to resend it.
        /// </param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public async Task<Message> SendVoiceAsync([NotNull] string chatId, [NotNull] string voice, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(voice) )
                throw new ArgumentNullException(nameof(voice));

            var parameters = new NameValueCollection { { "voice", voice } };

            if( duration > 0 )
                parameters.Add("duration", duration);

            return await this.CallTelegramMethodAsync("sendVoice", parameters, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="voiceStream">A <see cref="Stream" /> to the audio file to send.</param>
        /// <param name="fileName">
        /// A name for the file to be sent using <paramref name="voiceStream" />.
        /// </param>
        /// <param name="duration">Duration of sent audio in seconds</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceAsync(long chatId, [NotNull] Stream voiceStream, string fileName, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendVoiceAsync(chatId.ToString(), voiceStream, fileName, duration, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public async Task<Message> SendVoiceAsync([NotNull] string chatId, [NotNull] Stream voiceStream, string fileName, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( voiceStream == null )
                throw new ArgumentNullException(nameof(voiceStream));

            // ReSharper disable once UseObjectOrCollectionInitializer
            var content = new MultipartFormDataContent();
            content.Add("voice", voiceStream, fileName);

            if( duration > 0 )
                content.Add("duration", duration);

            return await this.CallTelegramMethodAsync("sendVoice", content, chatId, replyToMessageId, replyMarkup, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceFromFileAsync(long chatId, [NotNull] string filePath, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendVoiceFromFileAsync(chatId.ToString(), filePath, duration, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends an audio file to be displayed as a playable voice message on Telegram clients.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).
        /// </param>
        /// <param name="filePath">Fully qualified path to the audio file.</param>
        /// <param name="duration">Duration of sent audio in seconds.</param>
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
        /// <remarks>
        /// For this to work, your audio must be in an .ogg file encoded with OPUS (other formats may be sent
        /// as <see cref="Audio" /> or <see cref="Document" />). Bots can currently send audio files of up to
        /// 50 MB in size, this limit may be changed in the future.
        /// </remarks>
        public Task<Message> SendVoiceFromFileAsync([NotNull] string chatId, [NotNull] string filePath, int duration = 0, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( chatId == null )
                throw new ArgumentNullException(nameof(chatId));

            if( string.IsNullOrWhiteSpace(filePath) )
                throw new ArgumentNullException(nameof(filePath));

            if( !System.IO.File.Exists(filePath) )
                throw new FileNotFoundException("Unable to find the audio file at the specified location.", filePath);

            var fileName = Path.GetFileName(filePath);
            var fileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return this.SendVoiceAsync(chatId, fileStream, fileName, duration, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Use this method to specify a url and receive incoming updates via an outgoing webhook.
        /// </summary>
        /// <param name="url">
        /// HTTPS url to send updates to. Use an empty string to remove webhook integration
        /// </param>
        /// <param name="certificatePath">
        /// The fully qualified path to the certificate path so that the root certificate in use can be
        /// checked.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Whenever there is an update for the bot, Telegram will send an HTTPS POST request to the specified
        /// url, containing a JSON-serialized <see cref="Update" />. In case of an unsuccessful request, it
        /// will give up after a reasonable amount of attempts.
        /// <para>
        /// If you'd like to make sure that the Webhook request comes from Telegram, we recommend using a
        /// secret path in the URL, e.g. <c>www.example.com/YOUR_TOKEN</c>. Since nobody else knows your bot's
        /// token, you can be pretty sure it’s Telegram.
        /// </para>
        /// <list type="bullet">
        ///     <item>
        ///         <description>
        /// You will not be able to receive updates using <see cref="GetUpdatesAsync" /> for as long as an
        /// outgoing webhook is set up.
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
                if( !System.IO.File.Exists(certificatePath) )
                    throw new FileNotFoundException("Unable to find the certificate file at the specified location.", certificatePath);

                using( var content = new MultipartFormDataContent() )
                {
                    var fileName = Path.GetFileName(certificatePath);
                    var fileStream = System.IO.File.Open(certificatePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    content.Add("certificate", fileStream, fileName);
                    content.Add("url", url);

                    var response = await this.Client.PostAsync("setWebhook", content, cancellationToken).ConfigureAwait(false);
                    EnsureSuccessStatusCode(response);
                }
            }
        }

        #endregion
    }
}