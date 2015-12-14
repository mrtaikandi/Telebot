namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will display a reply interface to the
    /// user (act as if the user has selected the bot‘s message and tapped 'Reply'). This can be extremely
    /// useful if you want to create user-friendly step-by-step interfaces without having to sacrifice
    /// privacy mode.
    /// <para>
    /// More information: https://core.telegram.org/bots/api#forcereply
    /// </para>
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ForceReply : IReply
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to shows reply interface to the user, as if they manually
        /// selected the bot's message and tapped 'Reply'.
        /// </summary>
        [JsonProperty("force_reply")]
        public bool ReplyForce { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether you want to force reply from specific users only
        /// (Optional).
        /// </summary>
        /// <remarks>
        /// Use this parameter if you want to force reply from specific users only.
        /// <para>
        /// Targets: 1) users that are @mentioned in the text of the Message object; 2) if the bot's message is
        /// a reply (has reply_to_message_id), sender of the original message.
        /// </para>
        /// </remarks>
        [JsonProperty("selective")]
        public bool Selective { get; set; }

        #endregion
    }
}