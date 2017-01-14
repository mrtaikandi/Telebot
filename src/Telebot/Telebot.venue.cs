namespace Taikandi.Telebot
{
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Taikandi.Telebot.Types;

    public partial class Telebot
    {
        #region Public Methods and Operators

        /// <summary>Send information about a venue.</summary>
        /// <param name="chatId">Unique identifier for the target chat.</param>
        /// <param name="latitude">Latitude of the venue.</param>
        /// <param name="longitude">Longitude of the venue.</param>
        /// <param name="title">Name of the venue.</param>
        /// <param name="address">Address of the venue.</param>
        /// <param name="forsquareId">Foursquare identifier of the venue.</param>
        /// <param name="disableNotification">
        /// If set to <c>true</c> sends the message silently. iOS users will not receive a notification,
        /// Android users will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message.
        /// </param>
        /// <param name="replyMarkup">
        /// Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains sent
        /// <see cref="Message" /> on success.
        /// </returns>
        public Task<Message> SendVenueAsync(long chatId, float latitude, float longitude, [NotNull] string title, [NotNull] string address, string forsquareId = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero(chatId, nameof(chatId));

            return this.SendVenueAsync(chatId.ToString(), latitude, longitude, title, address, forsquareId, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Send information about a venue.
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="latitude">Latitude of the venue.</param>
        /// <param name="longitude">Longitude of the venue.</param>
        /// <param name="title">Name of the venue.</param>
        /// <param name="address">Address of the venue.</param>
        /// <param name="forsquareId">Foursquare identifier of the venue.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification,
        /// Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to
        /// complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task results contains sent
        /// <see cref="Message" /> on success.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">chatId cannot be null -or- title cannot be null -or- address cannot be null.</exception>
        public Task<Message> SendVenueAsync([NotNull] string chatId, float latitude, float longitude, [NotNull] string title, [NotNull] string address, string forsquareId = null, bool disableNotification = false, long replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));
            Contracts.EnsureNotNull(title, nameof(title));
            Contracts.EnsureNotNull(address, nameof(address));
            
            var parameters = new NameValueCollection
                                 {
                                     { "chat_id", chatId }, 
                                     { "latitude", latitude.ToString(CultureInfo.InvariantCulture) }, 
                                     { "longitude", longitude.ToString(CultureInfo.InvariantCulture) }, 
                                     { "title", title }, 
                                     { "address", address }
                                 };

            parameters.AddIf(!string.IsNullOrWhiteSpace(forsquareId), "foursquare_id", forsquareId);
            parameters.AddIf(!disableNotification, "disable_notification", true);
            parameters.AddIf(replyToMessageId > 0, "reply_to_message_id", replyToMessageId);

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendVenue", parameters, replyMarkup: replyMarkup);
        }

        #endregion
    }
}