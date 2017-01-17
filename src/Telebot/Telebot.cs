namespace Taikandi.Telebot
{
    using System;
    using System.Collections.Generic;
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
            Contracts.EnsureNotNull(apiKey, nameof(apiKey));

            this.ApiKey = apiKey;
            this.Timeout = TimeSpan.FromMinutes(1);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether disable all notifications. If set to <c>true</c> Sends the
        /// message silently. iOS users will not receive a notification, Android users will receive a
        /// notification with no sound.
        /// <para>
        /// Note that this property - if <c>true</c> - will overwrite <c>disableNotification</c> parameter of
        /// send messages.
        /// </para>
        /// </summary>
        public bool DisableNotifications { get; set; }

        /// <summary>
        /// Gets or sets the timespan to wait before the request to Telegram APIs times out.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        #endregion

        #region Properties

        protected string ApiKey { get; }

        protected HttpClient Client => this._client ?? (this._client = this.CreateHttpClient());

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Sends answers to callback queries sent from inline keyboards. The answer will be displayed to the
        /// user as a notification at the top of the chat screen or as an alert.
        /// </summary>
        /// <param name="callbackQueryId">Unique identifier for the query to be answered.</param>
        /// <param name="text">
        /// Text of the notification. If not specified, nothing will be shown to the user.
        /// </param>
        /// <param name="showAlert">
        /// If <c>true</c>, an alert will be shown by the client instead of a notification at the top of the
        /// chat screen. Defaults to <c>false</c>.
        /// </param>
        /// <param name="url">
        /// URL that will be opened by the user's client. If you have created a Game and accepted the conditions
        /// via <c>@Botfather</c>, specify the URL that opens your game – note that this will only work if the query
        /// comes from a <c>callback_game</c> button.
        /// <para>
        /// Otherwise, you may use links like telegram.me/your_bot?start=XXXX that open your bot with a parameter.
        /// </para>
        /// </param>
        /// <param name="cacheTime">
        /// The maximum amount of time in seconds that the result of the callback query may be cached client-side.
        /// Telegram apps will support caching starting in version 3.14. Defaults to 0.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>true</c> on
        /// success; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Task<bool> AnswerCallbackQueryAsync([NotNull] string callbackQueryId, string text = null, bool showAlert = false, string url = null, int cacheTime = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(callbackQueryId, nameof(callbackQueryId));
            Contracts.EnsurePositiveNumber(cacheTime, nameof(cacheTime));

            var parameters = new NameValueCollection
                                 {
                                     { "callback_query_id", callbackQueryId },
                                     { "show_alert", showAlert.ToString() }
                                 };

            parameters.AddIf(!string.IsNullOrWhiteSpace(text), "text", text);
            parameters.AddIf(!string.IsNullOrWhiteSpace(url), "url", url);
            parameters.AddIf(cacheTime!=0, "cache_time", cacheTime);

            return this.CallTelegramMethodAsync<bool>(cancellationToken, "answerCallbackQuery", parameters);
        }

        /// <summary>Sends answers to an inline query.</summary>
        /// <param name="inlineQueryId">Unique identifier for answered query.</param>
        /// <param name="results">Results of the inline query.</param>
        /// <param name="cacheTime">
        /// The maximum amount of time in seconds the result of the inline query may be cached on the Telegram
        /// server. Default is 300.
        /// </param>
        /// <param name="isPersonal">
        /// <c>true</c>, to cache the results on the Telegram server only for the user that sent the query. By
        /// default, results may be returned to any user who sends the same query.
        /// </param>
        /// <param name="nextOffset">
        /// The offset that a client should send in the next query with the same text to receive more results.
        /// <c>null</c> or <see cref="string.Empty" /> if there are no more results or if you don't support
        /// pagination. Offset length can't exceed 64 bytes.
        /// </param>
        /// <param name="switchPrivateMessageText">
        /// If passed, clients will display a button with specified text that switches the user to a private
        /// chat with the bot and sends the bot a start message with the parameter
        /// <paramref name="switchPrivateMessageParameter" />.
        /// </param>
        /// <param name="switchPrivateMessageParameter">
        /// Parameter for the start message sent to the bot when user presses the switch button.
        /// <para>
        /// Example: An inline bot that sends YouTube videos can ask the user to connect the bot to their
        /// YouTube account to adapt search results accordingly. To do this, it displays a ‘Connect your
        /// YouTube account’ button above the results, or even before showing any. The user presses the button,
        /// switches to a private chat with the bot and, in doing so, passes a start parameter that instructs
        /// the bot to return an oauth link. Once done, the bot can offer a switch_inline button so that the
        /// user can easily return to the chat where they wanted to use the bot's inline capabilities.
        /// </para>
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>true</c> if the
        /// answer successfully sent; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="inlineQueryId" /> is null -or- <paramref name="results" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The nextOffset argument must be less than 64 bytes.
        /// </exception>
        /// <exception cref="ArgumentException">No more than 50 results per query are allowed.</exception>
        public Task<bool> AnswerInlineQueryAsync([NotNull] string inlineQueryId, [NotNull] IEnumerable<InlineQueryResult> results, int cacheTime = 300, bool isPersonal = false, string nextOffset = null, string switchPrivateMessageText = null, string switchPrivateMessageParameter = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(inlineQueryId, nameof(inlineQueryId));
            Contracts.EnsureNotNull(results, nameof(results));

            var inlineQueryResults = results as IList<InlineQueryResult> ?? results.ToList();
            if( inlineQueryResults.Count > 50 )
                throw new ArgumentException("No more than 50 results per query are allowed.", nameof(results));

            var parameters = new MultipartFormDataContent();

            if( !string.IsNullOrWhiteSpace(nextOffset) )
            {
                Contracts.EnsureByteCount(nextOffset, nameof(nextOffset));
                parameters.Add("next_offset", nextOffset);
            }

            parameters.Add("inline_query_id", inlineQueryId);
            parameters.Add("results", inlineQueryResults);
            parameters.AddIf(cacheTime != 300, "cache_time", cacheTime);
            parameters.AddIf(isPersonal, "is_peronal", isPersonal);
            parameters.AddIf(!string.IsNullOrWhiteSpace(switchPrivateMessageText), "switch_pm_text", switchPrivateMessageText);
            parameters.AddIf(!string.IsNullOrWhiteSpace(switchPrivateMessageParameter), "switch_pm_parameter", switchPrivateMessageParameter);

            return this.CallTelegramMethodAsync<bool>(cancellationToken, "answerInlineQuery", parameters);
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
        /// Downloads the file requested by the
        /// <see cref="GetFileAsync(string,System.Threading.CancellationToken)" /> method.
        /// </summary>
        /// <param name="file">
        /// The file info received by calling
        /// <see cref="GetFileAsync(string,System.Threading.CancellationToken)" />.
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
            Contracts.EnsureNotNull(file, nameof(file));
            Contracts.EnsureNotNull(fullPath, nameof(fullPath));

            if( System.IO.File.Exists(fullPath) )
            {
                if( !overwrite )
                    throw new IOException($"File '{fullPath}' already exists.");

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
        /// Downloads the file requested by the
        /// <see cref="GetFileAsync(string,System.Threading.CancellationToken)" /> method.
        /// </summary>
        /// <param name="file">
        /// The file info received by calling
        /// <see cref="GetFileAsync(string,System.Threading.CancellationToken)" />.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <exception cref="ArgumentNullException">File cannot be null.</exception>
        /// <returns>
        /// Returns a task containing the downloaded file as a stream.
        /// </returns>
        public Task<Stream> DownloadFileAsync([NotNull] File file, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(file, nameof(file));

            var url = file.GetDownloadUrl(this.ApiKey);
            return this.Client.GetStreamAsync(url);
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
        public Task<File> GetFileAsync([NotNull] string fileId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(fileId, nameof(fileId));
            return this.CallTelegramMethodAsync<File>(cancellationToken, $"getFile?file_id={fileId}");
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
        public Task<User> GetMeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.CallTelegramMethodAsync<User>(cancellationToken, "getMe");
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
        public Task<IEnumerable<Update>> GetUpdatesAsync(long offset = 0, int limit = 100, int timeout = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.CallTelegramMethodAsync<IEnumerable<Update>>(cancellationToken, $"getUpdates?offset={offset}&limit={limit}&timeout={timeout}");
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
        public Task<UserProfilePhotos> GetUserProfilePhotosAsync(long userId, int offset = -1, int limit = 100, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsurePositiveNumber(userId, nameof(userId));

            var builder = new StringBuilder();
            builder.AppendFormat("getUserProfilePhotos?user_id={0}&limit={1}", userId, limit);

            if( offset >= 0 )
                builder.AppendFormat("offset={0}", offset);

            return this.CallTelegramMethodAsync<UserProfilePhotos>(cancellationToken, builder.ToString());
        }

        /// <summary>
        /// Kicks a user from a group or a supergroup. In the case of supergroups, the user will not be able to
        /// return to the group on their own using invite links, etc., unless unbanned first using
        /// <see cref="UnbanChatMemberAsync(string, long, CancellationToken)" />. The bot must be an
        /// administrator in the group for this to work.
        /// </summary>
        /// <param name="chatId">Unique identifier for the target group.</param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>true</c> on
        /// success.
        /// </returns>
        /// <remarks>
        /// Note: This will method only work if the ‘All Members Are Admins’ setting is off in the target
        /// group. Otherwise members may only be removed by the group's creator or by the member that added
        /// them.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        /// <exception cref="System.ArgumentException">userId must be a number greater than zero.</exception>
        public Task<bool> KickChatMemberAsync(long chatId, long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.KickChatMemberAsync(chatId.ToString(), userId, cancellationToken);
        }

        /// <summary>
        /// Kicks a user from a group or a supergroup. In the case of supergroups, the user will not be able to
        /// return to the group on their own using invite links, etc., unless unbanned first using
        /// <see cref="UnbanChatMemberAsync(string, long, CancellationToken)" />. The bot must be an
        /// administrator in the group for this to work.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target group or username of the target supergroup (in the format
        /// @supergroupusername).
        /// </param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>true</c> on
        /// success.
        /// </returns>
        /// <remarks>
        /// Note: This will method only work if the ‘All Members Are Admins’ setting is off in the target
        /// group. Otherwise members may only be removed by the group's creator or by the member that added
        /// them.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        /// <exception cref="System.ArgumentException">userId must be a number greater than zero.</exception>
        public Task<bool> KickChatMemberAsync([NotNull] string chatId, long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsurePositiveNumber(userId, nameof(userId));

            var parameters = new NameValueCollection { { "chat_id", chatId }, { "user_id", userId } };
            return this.CallTelegramMethodAsync<bool>(cancellationToken, "kickChatMember", parameters);
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
        /// <param name="maxConnections">
        /// Maximum allowed number of simultaneous HTTPS connections to the webhook for update delivery,
        /// 1-100. Defaults to 40. Use lower values to limit the load on your bot‘s server, and higher
        /// values to increase your bot’s throughput.
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
        public async Task SetWebhookAsync(string url, string certificatePath = null, int maxConnections = 40, UpdateType[] allowedUpdates = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if( !string.IsNullOrWhiteSpace(certificatePath) )
                Contracts.EnsureFileExists(certificatePath);

            using( var content = new MultipartFormDataContent() )
            {
                if( !string.IsNullOrWhiteSpace(certificatePath) )
                {
                    var fileName = Path.GetFileName(certificatePath);
                    var fileStream = System.IO.File.Open(certificatePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    content.Add("certificate", fileStream, fileName);
                }

                if( maxConnections != 40 )
                    content.Add( "max_connections", maxConnections );
                content.Add("url", url);

                var response = await this.Client.PostAsync("setWebhook", content, cancellationToken).ConfigureAwait(false);
                EnsureSuccessStatusCode(response);
            }
        }

        /// <summary>
        /// Use this method to get current webhook status.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// On success, returns a WebhookInfo object.
        /// <para>If the bot is using getUpdates, will return an object with the url field empty.</para>
        /// </returns>
        public Task<WebhookInfo> GetWebhookInfoAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.CallTelegramMethodAsync<WebhookInfo>(cancellationToken, "getWebhookInfo");
        }

        /// <summary>
        /// Use this method to remove webhook integration if you decide to switch back to <c>getUpdates</c>.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// Returns True on success.
        /// </returns>
        public Task<bool> DeleteWebhookAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.CallTelegramMethodAsync<bool>(cancellationToken, "deleteWebhook");
        }

        /// <summary>
        /// Unbans a previously kicked user in a supergroup. The user will not return to the group
        /// automatically, but will be able to join via link, etc. The bot must be an administrator in the
        /// group for this to work.
        /// </summary>
        /// <param name="chatId">Unique identifier for the target group.</param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>true</c> on
        /// success.
        /// </returns>
        /// <remarks>
        /// Note: This will method only work if the ‘All Members Are Admins’ setting is off in the target
        /// group. Otherwise members may only be removed by the group's creator or by the member that added
        /// them.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        /// <exception cref="System.ArgumentException">userId must be a number greater than zero.</exception>
        public Task<bool> UnbanChatMemberAsync(long chatId, long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));
            return this.KickChatMemberAsync(chatId.ToString(), userId, cancellationToken);
        }

        /// <summary>
        /// Unbans a previously kicked user in a supergroup. The user will not return to the group
        /// automatically, but will be able to join via link, etc. The bot must be an administrator in the
        /// group for this to work.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target group or username of the target supergroup (in the format
        /// @supergroupusername).
        /// </param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>true</c> on
        /// success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        /// <exception cref="System.ArgumentException">userId must be a number greater than zero.</exception>
        public Task<bool> UnbanChatMemberAsync([NotNull] string chatId, long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsurePositiveNumber(userId, nameof(userId));

            var parameters = new NameValueCollection { { "chat_id", chatId }, { "user_id", userId } };
            return this.CallTelegramMethodAsync<bool>(cancellationToken, "unbanChatMember", parameters);
        }

        /// <summary>
        /// Use this method for your bot to leave a group, supergroup or channel
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup or channel
        /// (in the format @channelusername).
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>true</c> on
        /// success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        public Task<bool> LeaveChatAsync([NotNull] string chatId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));

            var parameters = new NameValueCollection { { "chat_id", chatId } };
            return this.CallTelegramMethodAsync<bool>(cancellationToken, "leaveChat", parameters);
        }

        /// <summary>
        /// Use this method to get up to date information about the chat (current name of the user
        /// for one-on-one conversations, current username of a user, group or channel, etc.).
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup or channel
        /// (in the format @channelusername).
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>Chat</c> object on
        /// success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        public Task<Chat> GetChatAsync([NotNull] string chatId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));

            var parameters = new NameValueCollection { { "chat_id", chatId } };
            return this.CallTelegramMethodAsync<Chat>(cancellationToken, "getChat", parameters);
        }

        /// <summary>
        /// Use this method to get a list of administrators in a chat. On success, returns an Array of ChatMember objects that
        /// contains information about all chat administrators except other bots. If the chat is a group or a supergroup and no
        /// administrators were appointed, only the creator will be returned.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup or channel
        /// (in the format @channelusername).
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>Chat</c> object on
        /// success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        public Task<ChatMember> GetChatAdministratorsAsync([NotNull] string chatId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));

            var parameters = new NameValueCollection { { "chat_id", chatId } };
            return this.CallTelegramMethodAsync<ChatMember>(cancellationToken, "getChatAdministrators", parameters);
        }

        /// <summary>
        /// Use this method to get information about a member of a chat.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup or channel (in the format
        /// @channelusername)
        /// </param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>ChatMember</c> object on
        /// success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        /// <exception cref="System.ArgumentException">userId must be a number greater than zero.</exception>
        public Task<ChatMember> GetChatMemberAsync([NotNull] string chatId, long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsurePositiveNumber(userId, nameof(userId));

            var parameters = new NameValueCollection { { "chat_id", chatId }, { "user_id", userId } };
            return this.CallTelegramMethodAsync<ChatMember>(cancellationToken, "getChatMember", parameters);
        }

        /// <summary>
        /// Use this method to get the number of members in a chat.
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup or channel
        /// (in the format @channelusername).
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains <c>Chat</c> object on
        /// success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null.</exception>
        public Task<int> GetChatMembersCountAsync([NotNull] string chatId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));

            var parameters = new NameValueCollection { { "chat_id", chatId } };
            return this.CallTelegramMethodAsync<int>(cancellationToken, "getChatMembersCount", parameters);
        }

        #endregion
    }
}