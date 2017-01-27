namespace Taikandi.Telebot
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Types;

    public partial class Telebot
    {
        #region Public Methods and Operators

        /// <summary>
        /// Use this method to send a game.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="gameShortName">Short name of the game, serves as the unique identifier for the game. Set up your games via Botfather.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendGameAsync(long chatId, [NotNull] string gameShortName, bool disableNotification = false, long replyToMessageId = 0, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(gameShortName, nameof(gameShortName));

            return this.SendGameAsync(chatId.ToString(),gameShortName, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
        }

        /// <summary>
        /// Use this method to send a game.
        /// </summary>
        /// <param name="chatId">Unique identifier for the message recipient or username of the target channel (in the format
        /// @channelusername).</param>
        /// <param name="gameShortName">Short name of the game, serves as the unique identifier for the game. Set up your games via Botfather.</param>
        /// <param name="disableNotification">If set to <c>true</c> sends the message silently. iOS users will not receive a notification, Android users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message.</param>
        /// <param name="replyMarkup">Additional interface options. An <see cref="IReply" /> object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<Message> SendGameAsync([NotNull] string chatId, [NotNull] string gameShortName, bool disableNotification = false, long replyToMessageId = 0, InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotNull(gameShortName, nameof(gameShortName));

            var parameters = new NameValueCollection { { "game_short_name", gameShortName } };

            return this.CallTelegramMethodAsync<Message>(cancellationToken, "sendGame", parameters, chatId, replyToMessageId, replyMarkup, disableNotification);
        }

        /// <summary>
        /// Use this method to set the score of the specified user in a game.
        /// </summary>
        /// <param name="disableEditMessage">Pass True, if the game message should not be automatically edited to include the current scoreboard</param>
        /// <param name="chatId">Required if inline_message_id is not specified. Unique identifier for the target chat</param>
        /// <param name="inlineMessageId">Required if chat_id and message_id are not specified. Identifier of the inline message</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <param name="userId">User identifier</param>
        /// <param name="score">New score, must be non-negative</param>
        /// <param name="force">Pass True, if the high score is allowed to decrease. This can be useful when fixing mistakes or banning cheaters</param>
        /// <param name="messageId">Required if inline_message_id is not specified. Identifier of the sent message</param>
        /// <returns>
        /// On success, if the message was sent by the bot, returns the edited <see cref="Message" />., otherwise returns True. Returns an error, if the new score is not greater than the user's current score in the chat and force is False.
        /// </returns>
        public Task<object> SetGameScoreAsync(long userId, long score, bool force = false, bool disableEditMessage = false, long chatId = 0, long messageId = 0, string inlineMessageId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.SetGameScoreAsync(userId, score, force, disableEditMessage, chatId.ToString(), messageId, inlineMessageId, cancellationToken);
        }

        /// <summary>
        /// Use this method to set the score of the specified user in a game.
        /// </summary>
        /// <param name="disableEditMessage">Pass True, if the game message should not be automatically edited to include the current scoreboard</param>
        /// <param name="chatId">Required if inline_message_id is not specified. Unique identifier for the target chat</param>
        /// <param name="inlineMessageId">Required if chat_id and message_id are not specified. Identifier of the inline message</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <param name="userId">User identifier</param>
        /// <param name="score">New score, must be non-negative</param>
        /// <param name="force">Pass True, if the high score is allowed to decrease. This can be useful when fixing mistakes or banning cheaters</param>
        /// <param name="messageId">Required if inline_message_id is not specified. Identifier of the sent message</param>
        /// <returns>
        /// On success, if the message was sent by the bot, returns the edited <see cref="Message" />., otherwise returns True. Returns an error, if the new score is not greater than the user's current score in the chat and force is False.
        /// </returns>
        public Task<object> SetGameScoreAsync(long userId, long score, bool force = false, bool disableEditMessage = false, string chatId = null, long messageId = 0, string inlineMessageId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Contracts.EnsureNotZero( userId, nameof( userId ) );
            Contracts.EnsureNotNegativeNumber( score, nameof( score ) );

            var parameters = new NameValueCollection();
            if ( string.IsNullOrWhiteSpace( chatId ) && messageId == 0 )
            {
                Contracts.EnsureNotNull( inlineMessageId, nameof( inlineMessageId ) );
                parameters.Add( "inline_message_id", inlineMessageId );
            }
            else if( string.IsNullOrWhiteSpace( inlineMessageId ) )
            {
                Contracts.EnsureNotNull( chatId, nameof( chatId ) );
                Contracts.EnsureNotZero( messageId, nameof( messageId ) );
                parameters.Add( "chat_id", chatId );
                parameters.Add( "message_id", messageId );
            }

            parameters.Add( "user_id", userId );
            parameters.Add( "score", score );
            parameters.AddIf( force == true, "force", force );
            parameters.AddIf( disableEditMessage == true, "disable_edit_message", disableEditMessage );

            return this.CallTelegramMethodAsync<object>(cancellationToken, "setGameScore", parameters);
        }

        /// <summary>
        /// Use this method to get data for high score tables. Will return the score of the specified user and
        /// several of his neighbors in a game. On success, returns an Array of GameHighScore objects.
        /// </summary>
        /// <param name="userId">Target user id</param>
        /// <param name="chatId">Required if inline_message_id is not specified. Unique identifier for the target chat.</param>
        /// <param name="inlineMessageId">Required if chat_id and message_id are not specified. Identifier of the inline message.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <param name="messageId">Required if inline_message_id is not specified. Identifier of the sent message.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<GameHighScore[]> GetGameHighScoresAsync(long userId, long chatId = 0, long messageId = 0, string inlineMessageId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.GetGameHighScoresAsync(userId, chatId.ToString(), messageId, inlineMessageId, cancellationToken);
        }

        /// <summary>
        /// Use this method to get data for high score tables. Will return the score of the specified user and
        /// several of his neighbors in a game. On success, returns an Array of GameHighScore objects.
        /// </summary>
        /// <param name="userId">Target user id</param>
        /// <param name="chatId">Required if inline_message_id is not specified. Unique identifier for the target chat.</param>
        /// <param name="inlineMessageId">Required if chat_id and message_id are not specified. Identifier of the inline message.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <param name="messageId">Required if inline_message_id is not specified. Identifier of the sent message.</param>
        /// <returns>
        /// On success, returns the sent <see cref="Message" />.
        /// </returns>
        public Task<GameHighScore[]> GetGameHighScoresAsync(long userId, string chatId = null, long messageId = 0, string inlineMessageId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new NameValueCollection();
            if( string.IsNullOrWhiteSpace( chatId ) && messageId == 0 )
            {
                Contracts.EnsureNotNull( inlineMessageId, nameof( inlineMessageId ) );
                parameters.Add( "inline_message_id", inlineMessageId );
            }
            else if( string.IsNullOrWhiteSpace( inlineMessageId ) )
            {
                Contracts.EnsureNotNull( chatId, nameof( chatId ) );
                Contracts.EnsureNotZero( messageId, nameof( messageId ) );
                parameters.Add( "chat_id", chatId );
                parameters.Add( "message_id", messageId );
            }

            parameters.Add("user_id", userId);

            return this.CallTelegramMethodAsync<GameHighScore[]>(cancellationToken, "getGameHighScores", parameters, chatId);
        }

        #endregion
    }
}