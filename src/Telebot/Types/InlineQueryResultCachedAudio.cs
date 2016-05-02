namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to an mp3 audio file stored on the Telegram servers. By default, this audio file
    /// will be sent by the user. Alternatively, you can use
    /// <see cref="InlineQueryResult.MessageContent" /> to send a message with the specified content
    /// instead of the audio.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultCachedAudio : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a valid file identifier of the photo file.
        /// </summary>
        [Required]
        [JsonProperty("audio_file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Audio;

        #endregion
    }
}