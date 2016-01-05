namespace Taikandi.Telebot.Types
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a result which is a link to an article or web page.
    /// </summary>
    /// <seealso cref="Taikandi.Telebot.Types.InlineQueryResult" />
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultArticle : InlineQueryResult
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineQueryResultArticle" /> class.
        /// </summary>
        public InlineQueryResultArticle()
            : this(null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineQueryResultArticle" /> class.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of this result. Must be less than 64 bytes.
        /// </param>
        /// <param name="title">The title of the result.</param>
        /// <param name="messageText">The text of a message to be sent.</param>
        public InlineQueryResultArticle(string id, string title, string messageText)
        {
            ExceptionHelper.ValidateArgumentByteCount(nameof(id), id);

            this.Id = id;
            this.Title = title;
            this.MessageText = messageText;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a short description of the result (Optional).
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the URL in the message.
        /// </summary>
        [JsonProperty("hide_url")]
        public bool HideUrl { get; set; } = false;

        /// <summary>Gets or sets the text of a message to be sent.</summary>
        [Required]
        [StringLength(512)]
        [JsonProperty("message_text", Required = Required.Always)]
        public string MessageText { get; set; }

        /// <summary>
        /// Gets or sets the height of the thumbnail (Optional).
        /// </summary>
        [JsonProperty("thumb_height")]
        public string ThumbnailHeight { get; set; }

        /// <summary>
        /// Gets or sets the URL of the thumbnail for the result either in <c>.jpeg</c> or <c>.gif</c> format
        /// (Optional).
        /// </summary>
        [JsonProperty("thumb_url")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the width of the thumbnail (Optional).
        /// </summary>
        [JsonProperty("thumb_width")]
        public string ThumbnailWidth { get; set; }

        /// <summary>Gets or sets the title of the result.</summary>
        [Required]
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Article;

        /// <summary>Gets or sets the URL of the result.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        #endregion
    }
}