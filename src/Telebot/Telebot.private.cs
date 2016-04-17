namespace Taikandi.Telebot
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Newtonsoft.Json;

    using Taikandi.Telebot.Types;

    public partial class Telebot
    {
        #region Methods

        /// <summary>
        /// Creates a new instance of <see cref="HttpClient" /> to connect to the Telegram bot API.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="HttpClient" /> configured to connect to the Telegram bot API.
        /// </returns>
        protected virtual HttpClient CreateHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri($"https://api.telegram.org/bot{this.ApiKey}/", UriKind.Absolute) };

            return client;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged
        /// resources.
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
            if( response.IsSuccessStatusCode )
                return;

            if( response.StatusCode == HttpStatusCode.BadGateway )
                throw new ServiceUnavailableException();

            throw new HttpRequestException(response);
        }

        private static async Task<T> ReadTelegramResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if( response.IsSuccessStatusCode )
            {
                var telegramResponse = await response.Content.ReadAsAsync<TelegramResponse<T>>(cancellationToken).ConfigureAwait(false);
                return telegramResponse.Result;
            }

            if( response.StatusCode == HttpStatusCode.BadGateway )
                throw new ServiceUnavailableException();

            var error = await response.Content.ReadAsAsync<Error>(cancellationToken).ConfigureAwait(false);
            if( error != null )
                throw new HttpRequestException($"Error '{error.ErrorCode}': {error.Description}");

            throw new HttpRequestException(response);
        }

        private Task<Message> CallTelegramMethodAsync(string url, NameValueCollection parameters, string chatId, long replyToMessageId, IReply replyMarkup, CancellationToken cancellationToken)
        {
            return this.CallTelegramMethodAsync<Message>(url, parameters, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        private Task<Message> CallTelegramMethodAsync(string url, NameValueCollection parameters, string chatId, CancellationToken cancellationToken)
        {
            return this.CallTelegramMethodAsync<Message>(url, parameters, chatId, -1, null, cancellationToken);
        }

        private Task<T> CallTelegramMethodAsync<T>(string url, CancellationToken cancellationToken)
        {
            return this.CallTelegramMethodAsync<T>(url, (NameValueCollection)null, null, -1, null, cancellationToken);
        }

        private async Task<T> CallTelegramMethodAsync<T>(string url, NameValueCollection parameters, string chatId, long replyToMessageId, IReply replyMarkup, CancellationToken cancellationToken)
        {
            if( parameters == null )
            {
                using( var response = await this.Client.GetAsync(url, cancellationToken).ConfigureAwait(false) )
                {
                    return await ReadTelegramResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
                }
            }

            parameters.AddIf(!string.IsNullOrWhiteSpace(chatId), "chat_id", chatId);
            parameters.AddIf(replyToMessageId > 0, "reply_to_message_id", replyToMessageId.ToString());
            parameters.AddIf(replyMarkup != null, "reply_markup", JsonConvert.SerializeObject(replyMarkup));

            using( var content = new FormUrlEncodedContent(parameters) )
            {
                using( var response = await this.Client.PostAsync(url, content, cancellationToken).ConfigureAwait(false) )
                {
                    return await ReadTelegramResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        private Task<bool> CallTelegramMethodAsync([NotNull] string url, MultipartFormDataContent content, CancellationToken cancellationToken)
        {
            return this.CallTelegramMethodAsync<bool>(url, content, null, -1, null, cancellationToken);
        }

        private Task<Message> CallTelegramMethodAsync([NotNull] string url, MultipartFormDataContent content, string chatId, long replyToMessageId, IReply replyMarkup, CancellationToken cancellationToken)
        {
            return this.CallTelegramMethodAsync<Message>(url, content, chatId, replyToMessageId, replyMarkup, cancellationToken);
        }

        private async Task<TResult> CallTelegramMethodAsync<TResult>([NotNull] string url, MultipartFormDataContent content, string chatId, long replyToMessageId, IReply replyMarkup, CancellationToken cancellationToken)
        {
            using( content )
            {
                content.AddIf(!string.IsNullOrWhiteSpace(chatId), "chat_id", chatId);
                content.AddIf(replyToMessageId > 0, "reply_to_message_id", replyToMessageId.ToString());
                content.AddIf(replyMarkup != null, "reply_markup", replyMarkup);

                using( var response = await this.Client.PostAsync(url, content, cancellationToken).ConfigureAwait(false) )
                {
                    return await ReadTelegramResponseAsync<TResult>(response, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        #endregion
    }
}