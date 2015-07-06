namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// This object represents a group chat.
    /// </summary>
    [JsonObject]
    public class GroupChat : IConversation
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier for this group chat.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        #endregion
    }
}