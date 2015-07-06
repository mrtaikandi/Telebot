namespace Taikandi.Telebot
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    
    using Newtonsoft.Json;

    using Types;

    public class Telebot : IDisposable
    {
        #region Constants and Fields

        protected string ApiKey { get; }

        private HttpClient _client;

        private volatile bool _disposedValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of <see cref="Telebot"/>.
        /// </summary>
        /// <param name="apiKey">Telegram API key.</param>
        public Telebot(string apiKey)
        {
            this.ApiKey = apiKey;
        }

        #endregion

        #region Public Properties

        public HttpClient Client => this._client ?? (this._client = this.CreateHttpClient());

        #endregion

        #region Public Methods

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
        /// A simple method for testing your bot's auth token. Requires no parameters. 
        /// Returns basic information about the bot in form of a User object.
        /// </summary>
        /// <returns>Basic information about the bot in form of a User object.</returns>
        public async Task<TelegramResult<User>> GetMeAsync()
        {
            var response = await this.Client.GetAsync("getMe").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TelegramResult<User>>().ConfigureAwait(false);
        }

        /// <summary>
        /// Use this method to specify a url and receive incoming updates via an outgoing webhook.
        /// </summary>
        /// <param name="url">HTTPS url to send updates to. Use an empty string to remove webhook integration</param>
        /// <returns>
        /// Continues task.
        /// </returns>
        /// <remarks>
        /// Whenever there is an update for the bot, we will send an HTTPS POST request to the specified url, 
        /// containing a JSON-serialized <see cref="Update"/>. In case of an unsuccessful request, 
        /// we will give up after a reasonable amount of attempts.
        /// <para>
        /// If you'd like to make sure that the Webhook request comes from Telegram, we recommend using 
        /// a secret path in the URL, e.g. <c>www.example.com/YOUR_TOKEN</c>. Since nobody else knows your 
        /// bot‘s token, you can be pretty sure it’s us.
        /// </para>        
        /// </remarks>
        public async Task SetWebhookAsync(string url)
        {
            await this.Client.PostAsync(url, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Use this method to receive incoming updates using long polling. 
        /// <para>Note: In order to avoid getting duplicate updates, recalculate offset after each server response.</para>
        /// </summary>
        /// <param name="offset">Identifier of the first update to be returned. Must be greater by one than the highest among
        /// the identifiers of previously received updates. By default, updates starting with the earliest
        /// unconfirmed update are returned. An update is considered confirmed as soon as getUpdates is
        /// called with an offset higher than its update_id.</param>
        /// <param name="limit">Limits the number of updates to be retrieved. Values between 1—100 are accepted. Defaults to 100.</param>
        /// <param name="timeout">Timeout in seconds for long polling. Defaults to 0, i.e. usual short polling.</param>
        /// <returns>
        /// An Array of Update objects.
        /// </returns>
        public async Task<TelegramResult<IList<Update>>> GetUpdatesAsync(int offset = 0, int limit = 100, int timeout = 0)
        {
            var response = await this.Client
                                     .GetAsync($"getUpdates?offset={offset}&limit={limit}&timeout={timeout}")
                                     .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TelegramResult<IList<Update>>>().ConfigureAwait(false);
        }

        /// <summary>
        /// Use this method to send text messages. On success, the sent Message is returned.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient — <see cref="User"/> or <see cref="GroupChat"/> id.</param>
        /// <param name="text">Text of the message to be sent.</param>
        /// <param name="disableWebPagePreview">if set to <c>true</c> disables link previews for links in this message.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply"/> object for a custom reply keyboard, instructions to hide keyboard or to force a reply from the user.</param>
        /// <returns>
        /// The sent Message.
        /// </returns>
        public async Task<Message> SendMessage(int chatId, string text, bool disableWebPagePreview = false, int replyToMessageId = -1, IReply replyMarkup = null)
        {
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("chat_id", chatId.ToString()),
                    new KeyValuePair<string, string>("text", text),
                };

            if(disableWebPagePreview)
                parameters.Add(new KeyValuePair<string, string>("disable_web_page_preview", "true"));

            if(replyToMessageId >= 0)
                parameters.Add(new KeyValuePair<string, string>("reply_to_message_id", replyToMessageId.ToString()));

            if( replyMarkup != null )
                parameters.Add(new KeyValuePair<string, string>("reply_markup", JsonConvert.SerializeObject(replyMarkup)));

            var content = new FormUrlEncodedContent(parameters);
            var response = await this.Client.PostAsync("sendMessage", content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Message>().ConfigureAwait(false);
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of <see cref="HttpClient"/> to connect to the Telegram bot API.
        /// </summary>
        /// <returns>A new instance of <see cref="HttpClient"/> configured to connect to the Telegram bot API.</returns>
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

        #endregion
    }
}