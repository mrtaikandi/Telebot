namespace Taikandi.Telebot.Types
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// This object represent a user's profile pictures.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class UserProfilePhotos
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the requested profile pictures (in up to 4 sizes each).
        /// </summary>
        [JsonProperty("photos")]
        public IList<PhotoSize> Photos { get; set; }

        /// <summary>
        /// Gets or sets the total number of profile pictures the target user has.
        /// </summary>
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        #endregion
    }
}