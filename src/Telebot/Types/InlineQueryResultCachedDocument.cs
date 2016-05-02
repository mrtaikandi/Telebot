namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to a file stored on the Telegram servers. By default, this file will be sent by the user with an optional caption. Alternatively, you can use <see cref="InlineQueryResult.MessageContent" /> to send a message with the specified content instead of the file. Currently, only pdf-files and zip archives can be sent using this method.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    public class InlineQueryResultCachedDocument : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the maximum of 200 character caption of the document to be sent (Optional).
        /// </summary>
        [StringLength(200)]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the short description of the result (Optional).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a valid file identifier of the photo file.
        /// </summary>
        [Required]
        [JsonProperty("document_file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>Gets or sets the title of the result.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Document;

        #endregion
    }
}