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

        /// <summary>
        /// Edits text messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format @channelusername). Required if <paramref name="inlineMessageId"/> is not specified.</param>
        /// <param name="messageId">Unique identifier of the sent message. Required if <paramref name="inlineMessageId"/> is not specified.</param>
        /// <param name="inlineMessageId">The identifier of the inline message. Required if <paramref name="chatId"/> and <paramref name="messageId"/> are not specified.</param>
        /// <param name="text">New text of the message</param>
        /// <param name="parseMode">
        /// A value from <see cref="ParseMode"/> enum indicates the way that the Telegram should parse the sent message.
        /// Send <see cref="ParseMode.Markdown"/>, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in your bot's message.
        /// </param>
        /// <param name="disableWebPagePreview">Disables link previews for links in this message</param>
        /// <param name="replyMarkup">An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains the edited <see cref="Message" /> on success.
        /// </returns>
        private Task<Message> EditMessageTextAsync(string chatId, long messageId, string inlineMessageId, [NotNull] string text, ParseMode parseMode = ParseMode.Normal, bool disableWebPagePreview = false, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(text, nameof(text));

            var parameters = new NameValueCollection();
            parameters.AddIf(!string.IsNullOrWhiteSpace(chatId), "chat_id", chatId);
            parameters.AddIf(messageId > 0, "message_id", messageId.ToString());
            parameters.AddIf(!string.IsNullOrWhiteSpace(inlineMessageId), "inline_message_id", inlineMessageId);
            parameters.Add("text", text);
            parameters.AddIf(parseMode != ParseMode.Normal, "parse_mode", ParseMode.Markdown.ToString());
            parameters.AddIf(disableWebPagePreview, "disable_web_page_preview", true);
            
            return this.CallTelegramMethodAsync<Message>(cancellationToken, "editMessageText", parameters, replyMarkup: replyMarkup);
        }

        /// <summary>
        /// Edits captions of the message with the provided identifier sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format @channelusername). Required if <paramref name="inlineMessageId" /> is not specified.</param>
        /// <param name="messageId">Unique identifier of the sent message. Required if <paramref name="inlineMessageId" /> is not specified.</param>
        /// <param name="inlineMessageId">Identifier of the inline message. Required if <paramref name="chatId"/> and <paramref name="messageId"/> are not specified. </param>
        /// <param name="caption">New caption of the message.</param>
        /// <param name="replyMarkup">An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. On success the task results contains the edited Message is returned.
        /// </returns>
        private Task<Message> EditMessageCaptionAsync(string chatId, long messageId, string inlineMessageId, [NotNull] string caption, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(caption, nameof(caption));

            var parameters = new NameValueCollection();
            parameters.AddIf(!string.IsNullOrWhiteSpace(chatId), "chat_id", chatId);
            parameters.AddIf(messageId > 0, "message_id", messageId.ToString());
            parameters.AddIf(!string.IsNullOrWhiteSpace(inlineMessageId), "inline_message_id", inlineMessageId);
            parameters.Add("caption", caption);
            
            return this.CallTelegramMethodAsync<Message>(cancellationToken, "editMessageText", parameters, replyMarkup: replyMarkup);
        }

        /// <summary>
        /// Edit only the reply markup of messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format @channelusername). Required if <paramref name="inlineMessageId" /> is not specified.</param>
        /// <param name="messageId">Unique identifier of the sent message. Required if <paramref name="inlineMessageId" /> is not specified.</param>
        /// <param name="inlineMessageId">Identifier of the inline message. Required if <paramref name="chatId" /> and <paramref name="messageId" /> are not specified.</param>
        /// <param name="replyMarkup">An <see cref="InlineKeyboardMarkup" /> object for a custom reply keyboard.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. On success the task results contains the edited Message is returned.
        /// </returns>
        private Task<Message> EditMessageReplyMarkupAsync(string chatId, long messageId, string inlineMessageId, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new NameValueCollection();
            parameters.AddIf(!string.IsNullOrWhiteSpace(chatId), "chat_id", chatId);
            parameters.AddIf(messageId > 0, "message_id", messageId.ToString());
            parameters.AddIf(!string.IsNullOrWhiteSpace(inlineMessageId), "inline_message_id", inlineMessageId);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "editMessageReplyMarkup", parameters, replyMarkup: replyMarkup);
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

        private async Task<TResult> CallTelegramMethodAsync<TResult>(CancellationToken cancellationToken, string url, NameValueCollection parameters = null, string chatId = null, long replyToMessageId = 0, IReply replyMarkup = null, bool disableNotification = false)
        {
            if( parameters == null )
            {
                using( var response = await this.Client.GetAsync(url, cancellationToken).ConfigureAwait(false) )
                {
                    return await ReadTelegramResponseAsync<TResult>(response, cancellationToken).ConfigureAwait(false);
                }
            }

            parameters.AddIf(!string.IsNullOrWhiteSpace(chatId), "chat_id", chatId);
            parameters.AddIf(replyToMessageId > 0, "reply_to_message_id", replyToMessageId.ToString());
            parameters.AddIf(replyMarkup != null, "reply_markup", JsonConvert.SerializeObject(replyMarkup));
            parameters.AddIf(this.DisableNotifications || disableNotification, "disable_notification ", true);
            
            using( var content = new FormUrlEncodedContent(parameters) )
            {
                using( var response = await this.Client.PostAsync(url, content, cancellationToken).ConfigureAwait(false) )
                {
                    return await ReadTelegramResponseAsync<TResult>(response, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        private async Task<TResult> CallTelegramMethodAsync<TResult>(CancellationToken cancellationToken, [NotNull] string url, [NotNull] MultipartFormDataContent content, string chatId = null, long replyToMessageId = 0, IReply replyMarkup = null, bool disableNotification = false)
        {
            using( content )
            {
                content.AddIf(!string.IsNullOrWhiteSpace(chatId), "chat_id", chatId);
                content.AddIf(replyToMessageId > 0, "reply_to_message_id", replyToMessageId.ToString());
                content.AddIf(replyMarkup != null, "reply_markup", replyMarkup);
                content.AddIf(this.DisableNotifications || disableNotification, "disable_notification ", true);

                using( var response = await this.Client.PostAsync(url, content, cancellationToken).ConfigureAwait(false) )
                {
                    return await ReadTelegramResponseAsync<TResult>(response, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        #endregion
    }
}