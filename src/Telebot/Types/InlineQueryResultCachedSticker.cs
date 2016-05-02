namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to a sticker stored on the Telegram servers. By default, this sticker will be
    /// sent by the user. Alternatively, you can use <see cref="InlineQueryResult.MessageContent" /> to
    /// send a message with the specified content instead of the sticker.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    public class InlineQueryResultCachedSticker : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a valid file identifier of the sticker file.
        /// </summary>
        [Required]
        [JsonProperty("sticker_file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Sticker;

        #endregion
    }
}