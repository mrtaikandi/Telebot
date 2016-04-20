namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to an animated GIF file stored on the Telegram servers. By default, this animated
    /// GIF file will be sent by the user with an optional caption. Alternatively, you can use
    /// <see cref="InlineQueryResult.MessageContent" /> to send a message with specified content instead of
    /// the animation.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    public class InlineQueryResultCachedGif : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the maximum of 200 character caption of the GIF file to be sent (Optional).
        /// </summary>
        [StringLength(200)]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets a valid file identifier of the GIF file.
        /// </summary>
        [Required]
        [JsonProperty("gif_file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>Gets or sets the title of the result (Optional).</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Gif;

        #endregion
    }
}