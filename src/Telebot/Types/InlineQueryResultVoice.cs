namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to a voice recording in an .ogg container encoded with OPUS. By default, this
    /// voice recording will be sent by the user. Alternatively, you can use
    /// <see cref="InlineQueryResult.MessageContent" /> to send a message with the specified content
    /// instead of the the voice message.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore
    /// them.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultVoice : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the recording duration in seconds (Optional).
        /// </summary>
        [JsonProperty("voice_duration", Required = Required.Always)]
        public int Duration { get; set; }

        /// <summary>Gets or sets the recording title.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>Gets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Voice;

        /// <summary>
        /// Gets or sets a valid URL for the voice recording.
        /// </summary>
        [Required]
        [JsonProperty("voice_url", Required = Required.Always)]
        public string Url { get; set; }

        #endregion
    }
}