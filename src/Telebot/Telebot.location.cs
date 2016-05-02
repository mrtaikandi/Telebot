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

        /// <summary>
        /// Sends a point on the map.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient.</param>
        /// <param name="latitude">Latitude of location</param>
        /// <param name="longitude">Longitude of location</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendLocationAsync(long chatId, double latitude, double longitude, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SendLocationAsync(chatId.ToString(), latitude, longitude, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Sends a point on the map.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="latitude">Latitude of location</param>
        /// <param name="longitude">Longitude of location</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendLocationAsync([NotNull] string chatId, double latitude, double longitude, bool disableNotification = false, int replyToMessageId = 0, IReply replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(chatId, nameof(chatId));

            var parameters = new NameValueCollection
                                 {
                                     { "latitude", latitude.ToString(CultureInfo.InvariantCulture) },
                                     { "longitude", longitude.ToString(CultureInfo.InvariantCulture) }
                                 };

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendLocation", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        #endregion
    }
}