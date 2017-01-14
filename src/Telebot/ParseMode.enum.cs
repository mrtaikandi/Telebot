namespace Taikandi.Telebot
{
    public enum ParseMode
    {
        /// <summary>
        /// Indicates that the normal parser should be used.
        /// </summary>
        Normal,

        /// <summary>
        /// Indicates that the message should be parsed using Markdown parser. Use this if you want Telegram apps to show bold, italic and inline URLs.
        /// </summary>
        Markdown,

        /// <summary>
        /// Indicates that the message should be parsed using HTML parser. Use this if you want Telegram apps to show bold, italic and inline URLs.
        /// </summary>
        Html
    }
}