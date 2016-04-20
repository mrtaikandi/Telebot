namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to an mp3 audio file. By default, this audio file will be sent by the user.
    /// Alternatively, you can use <see cref="InlineQueryResult.MessageContent" /> to send a message with
    /// the specified content instead of the audio.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultAudio : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the audio duration in seconds (Optional).
        /// </summary>
        [JsonProperty("audio_duration", Required = Required.Always)]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the performer of the audio (Optional).
        /// </summary>
        [JsonProperty("performer", Required = Required.Default)]
        public string Performer { get; set; }

        /// <summary>Gets or sets the title of the result.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>Gets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Audio;

        /// <summary>Gets or sets a valid URL for the audio file.</summary>
        [Required]
        [JsonProperty("audio_url", Required = Required.Always)]
        public string Url { get; set; }

        #endregion
    }
}